// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  6:40 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  4:50 AM
// ***********************************************************************
// <copyright file="DepositByYilianSaga.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Service;

namespace Yuyi.Jinyinmao.Domain.Sagas
{
    /// <summary>
    ///     DepositByYilianSaga.
    /// </summary>
    public class DepositByYilianSaga : SagaGrain<IDepositByYilianSagaState>, IDepositByYilianSaga
    {
        private IYilianPaymentGatewayService Service { get; set; }

        #region IDepositByYilianSaga Members

        /// <summary>
        ///     Begins the process.
        /// </summary>
        /// <param name="initData">The initialize data.</param>
        /// <returns>Task.</returns>
        public async Task BeginProcessAsync(DepositFromYilianSagaInitDto initData)
        {
            this.State.InitData = initData;
            this.InitSagaEntity();

            AccountTranscation transcation = new AccountTranscation
            {
                AgreementsInfo = initData.TranscationInfo.AgreementsInfo.ToJson(),
                Amount = initData.TranscationInfo.Amount,
                Args = initData.Command.Args,
                BankCardInfo = initData.BackCardInfo.ToJson(),
                Cellphone = initData.UserInfo.Cellphone,
                ChannelCode = initData.TranscationInfo.ChannelCode,
                Info = new object().ToJson(),
                ResultCode = initData.TranscationInfo.ResultCode,
                ResultTime = initData.TranscationInfo.ResultTime,
                TradeCode = initData.TranscationInfo.TradeCode,
                TransDesc = initData.TranscationInfo.TransDesc,
                TranscationIdentifier = initData.TranscationInfo.TransactionId.ToGuidString(),
                TranscationTime = initData.TranscationInfo.TransactionTime,
                UserIdentifier = initData.UserInfo.UserId.ToGuidString(),
                UserInfo = initData.UserInfo.ToJson()
            };

            using (JYMDBContext db = new JYMDBContext())
            {
                if (await db.ReadonlyQuery<AccountTranscation>().AnyAsync(t => t.TranscationIdentifier == transcation.TranscationIdentifier))
                {
                    return;
                }

                await db.SaveAsync(transcation);
            }

            PaymentRequestParameter parameter = await this.BuildRequestParameter();
            YilianRequestResult result = await this.Service.PaymentRequestAsync(parameter);
            this.SagaEntity.Info.Add("Request", new { result.Message, result.ResponseString });
            if (!result.Result)
            {
                this.SagaEntity.State = -1;
            }
            else
            {
                await this.RegisterOrUpdateReminder("Saga", TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30));
            }
        }

        #endregion IDepositByYilianSaga Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.Service = new YilianPaymentGatewayService();

            return base.OnActivateAsync();
        }

        /// <summary>
        ///     Processes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public override async Task ProcessAsync()
        {
            YilianRequestResult result = await this.Service.QueryRequestAsync(this.State.SagaId.ToGuidString(), false);

            TableResult tableResult = await SiloClusterConfig.SagasTable.ExecuteAsync(TableOperation.Retrieve<SagaEntity>(this.State.SagaType, this.State.SagaId.ToGuidString()));

            this.SagaEntity = (SagaEntity)tableResult.Result;

            if (result == null)
            {
                this.SagaEntity.Info["Query"] = new { Message = "Processing" };
            }
            else
            {
                this.SagaEntity.Info["Query"] = new { result.Message, result.ResponseString };
                this.SagaEntity.State = 1;

                await this.UnregisterReminder(await this.GetReminder("Saga"));

                IUser user = UserFactory.GetGrain(this.State.InitData.UserInfo.UserId);
                await user.DepositResultedAsync(this.State.InitData, result.Result, result.ResponseString.Remove(0, result.ResponseString.IndexOf(":", StringComparison.InvariantCulture)));
            }

            await this.StoreSagaEntityAsync();
        }

        /// <summary>
        ///     Initializes the saga entity.
        /// </summary>
        protected override void InitSagaEntity()
        {
            base.InitSagaEntity();
            this.SagaEntity.InitData = this.State.InitData.ToJson();
        }

        private async Task<PaymentRequestParameter> BuildRequestParameter()
        {
            ISequenceGenerator sequenceGenerator = SequenceGeneratorFactory.GetGrain(Guid.Empty);
            string sequenceNo = await sequenceGenerator.GenerateNoAsync('D');
            string[] address = this.State.InitData.BackCardInfo.CityName.Split('|');
            return new PaymentRequestParameter(this.State.SagaId.ToGuidString(), sequenceNo,
                this.State.InitData.Command.BankCardNo, this.State.InitData.UserInfo.RealName,
                address[0], address[1], this.State.InitData.BackCardInfo.BankName,
                (int)this.State.InitData.UserInfo.Credential, this.State.InitData.UserInfo.CredentialNo,
                this.State.InitData.UserInfo.Cellphone, this.State.InitData.UserInfo.UserId.ToGuidString(),
                "D" + DateTime.UtcNow.AddHours(8).Date.ToString("yyMMdd"), decimal.Divide(this.State.InitData.TranscationInfo.Amount, 100));
        }
    }
}