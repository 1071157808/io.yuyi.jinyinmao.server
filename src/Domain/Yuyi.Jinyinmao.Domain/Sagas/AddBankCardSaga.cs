// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  2:17 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  11:15 PM
// ***********************************************************************
// <copyright file="AddBankCardSaga.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service;

namespace Yuyi.Jinyinmao.Domain.Sagas
{
    /// <summary>
    ///     AddBankCardSaga.
    /// </summary>
    public class AddBankCardSaga : SagaGrain<IAddBankCardSagaState>, IAddBankCardSaga
    {
        private IYilianPaymentGatewayService Service { get; set; }

        #region IAddBankCardSaga Members

        /// <summary>
        ///     Begins the process.
        /// </summary>
        /// <param name="initData">The initData.</param>
        /// <returns>Task.</returns>
        public async Task BeginProcessAsync(AddBankCardSagaInitDto initData)
        {
            this.State.InitData = initData;
            this.InitSagaEntity();

            AuthRequestParameter parameter = await this.BuildRequestParameter();
            YilianRequestResult result = await this.Service.AuthRequestAsync(parameter);
            this.SagaEntity.Info.Add("Reuqest", new { result.Message, result.ResponseString });
            if (!result.Result)
            {
                this.SagaEntity.State = 1;
            }

            if (result.Result)
            {
                await this.RegisterOrUpdateReminder("Saga", TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1));
            }

            await this.StoreSagaEntityAsync();
        }

        /// <summary>
        ///     Processes the saga task asynchronous.
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

                IUser user = UserFactory.GetGrain(this.State.InitData.UserInfo.UserId);
                await user.AddBankCardResultedAsync(this.State.InitData.Command, result.Result);
            }

            await this.StoreSagaEntityAsync();
        }

        #endregion IAddBankCardSaga Members

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
        ///     Initializes the saga entity.
        /// </summary>
        protected override void InitSagaEntity()
        {
            base.InitSagaEntity();
            this.SagaEntity.InitData = this.State.InitData.ToJson();
        }

        private async Task<AuthRequestParameter> BuildRequestParameter()
        {
            ISequenceGenerator sequenceGenerator = SequenceGeneratorFactory.GetGrain(Guid.NewGuid());
            string sequenceNo = await sequenceGenerator.GenerateNoAsync('B');
            string[] address = this.State.InitData.Command.CityName.Split('|');
            return new AuthRequestParameter(this.State.SagaId.ToGuidString(), sequenceNo,
                this.State.InitData.Command.BankCardNo, this.State.InitData.UserInfo.RealName,
                address[0], address[1], this.State.InitData.Command.BankName,
                (int)this.State.InitData.UserInfo.Credential, this.State.InitData.UserInfo.CredentialNo,
                this.State.InitData.UserInfo.Cellphone, this.State.InitData.UserInfo.UserId.ToGuidString());
        }
    }
}
