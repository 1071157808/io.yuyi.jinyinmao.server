// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  11:48 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-17  5:44 PM
// ***********************************************************************
// <copyright file="DepositSaga.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.InfoDtos;
using Yuyi.Jinyinmao.Service;

namespace Yuyi.Jinyinmao.Domain.Sagas
{
    /// <summary>
    ///     DepositSaga.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public class DepositSaga : SagaGrain<IDepositSagaState>, IDepositSaga
    {
        private Dictionary<string, object> Info { get; set; }
        private string Message { get; set; }
        private IUser User { get; set; }
        private bool Waiting { get; set; }
        private IYilianPaymentGatewayService YilianService { get; set; }

        #region IDepositSaga Members

        /// <summary>
        ///     Begins the process.
        /// </summary>
        /// <param name="initData">The initData.</param>
        /// <returns>Task.</returns>
        public Task BeginProcessAsync(DepositSagaInitData initData)
        {
            this.State.InitData = initData;
            this.State.Status = DepositSagaStatus.Init;
            this.State.BeginTime = DateTime.UtcNow;

            this.ProcessAsync().Forget();

            return TaskDone.Done;
        }

        #endregion IDepositSaga Members

        /// <summary>
        ///     Processes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public override async Task ProcessAsync()
        {
            try
            {
                do
                {
                    switch (this.State.Status)
                    {
                        case DepositSagaStatus.Init:
                            await this.InitAsync();
                            break;

                        case DepositSagaStatus.AddBankCard:
                            await this.ProcessAddBankCardAsync();
                            break;

                        case DepositSagaStatus.Authenticate:
                            await this.ProcessAuthenticateAsync();
                            break;

                        case DepositSagaStatus.QueryAuthenticateResult:
                            await this.QueryAuthenticateResultAsync();
                            break;

                        case DepositSagaStatus.VerifyBankCard:
                            await this.ProcessVerifyBankCardAsync();
                            break;

                        case DepositSagaStatus.QueryBankCardVerifiedResult:
                            await this.QueryBankCardVerifiedResultAsync();
                            break;

                        case DepositSagaStatus.PayByYilian:
                            await this.ProcessPayByYilianAsync();
                            break;

                        case DepositSagaStatus.QueryYilianPaymentResult:
                            await this.QueryYilianPaymentResultAsync();
                            break;

                        case DepositSagaStatus.Finished:
                            await this.FinishAsync();
                            return;

                        default:
                            throw new ApplicationException("Incorrect saga status, {0}".FormatWith(this.State.Status));
                    }

                    await this.StoreSagaStateAsync((int)this.State.Status, this.Message, this.Info);
                } while (!this.Waiting);
            }
            catch (Exception e)
            {
                this.State.Status = DepositSagaStatus.Fault;
                this.RunIntoError(e, this.Message, (int)this.State.Status, this.Info).Forget();
            }
        }

        private async Task FinishAsync()
        {
            this.Waiting = false;
            this.Message = "Finish";
            DateTime now = DateTime.UtcNow;
            int duration = (now - this.State.BeginTime).Seconds;

            this.Info = new Dictionary<string, object>
            {
                { "Duration", duration },
                { "FinishTime", DateTime.UtcNow }
            };
            await this.UnregisterReminder();

            await this.StoreSagaStateAsync((int)this.State.Status, this.Message, this.Info, 1);
        }

        private async Task QueryYilianPaymentResultAsync()
        {
            if (this.State.InitData.PayByYilianCommand == null)
            {
                throw new ApplicationException("Missing PayByYilianCommand SagaId-{0}.".FormatWith(this.State.SagaId.ToGuidString()));
            }

            YilianRequestResult result = await this.YilianService.QueryRequestAsync(this.State.SagaId.ToGuidString(), true);

            if (result == null)
            {
                this.Info = new Dictionary<string, object> { { "Query", new { Message = "Processing" } } };
            }
            else
            {
                this.Info = new Dictionary<string, object> { { "Query", new { result.Message, result.ResponseString } } };

                await this.UnregisterReminder();

                this.Waiting = false;
                this.State.Status = DepositSagaStatus.Finished;

                await this.User.DepositResultedAsync(this.State.InitData.PayByYilianCommand, result);
            }
        }

        private async Task<AuthRequestParameter> BuildRequestParameterAsync(string cityName, string bankCardNo, string realName, string bankName, int credential, string credentialNo, string cellphone, string userId, char sequencePrefix = 'A')
        {
            ISequenceGenerator sequenceGenerator = SequenceGeneratorFactory.GetGrain(Guid.Empty);
            string sequenceNo = await sequenceGenerator.GenerateNoAsync(sequencePrefix);
            string[] address = cityName.Split('|');
            return new AuthRequestParameter(this.State.SagaId.ToGuidString().ToUpperInvariant(), sequenceNo.ToUpperInvariant(),
                bankCardNo, realName, address[0], address[1], bankName, credential, credentialNo, cellphone, userId);
        }

        private Task InitAsync()
        {
            this.User = UserFactory.GetGrain(this.State.InitData.InitUserInfo.UserId);
            return TaskDone.Done;
        }

        private async Task ProcessAddBankCardAsync()
        {
            if (this.State.InitData.AddBankCardCommand != null)
            {
                await this.User.AddBankCardAsync(this.State.InitData.AddBankCardCommand);
            }
            this.State.Status = DepositSagaStatus.Authenticate;
        }

        private async Task ProcessAuthenticateAsync()
        {
            if (this.State.InitData.AuthenticateCommand != null)
            {
                UserInfo userInfo = await this.User.AuthenticateAsync(this.State.InitData.AuthenticateCommand);

                if (!userInfo.Verified)
                {
                    AuthRequestParameter parameter = await this.BuildRequestParameterAsync(this.State.InitData.AuthenticateCommand.CityName,
                        this.State.InitData.AuthenticateCommand.BankCardNo, userInfo.RealName, this.State.InitData.AuthenticateCommand.BankName, (int)userInfo.Credential,
                        userInfo.CredentialNo, this.State.InitData.AuthenticateCommand.Cellphone, userInfo.UserId.ToGuidString());

                    YilianRequestResult result = await this.YilianService.AuthRequestAsync(parameter);
                    this.Info.Add("Request-{0}".FormatWith(DateTime.UtcNow.ToString("O")), new { result.Message, result.ResponseString });

                    if (!result.Result)
                    {
                        throw new ApplicationException("Yilian request failed.");
                    }

                    await this.RegisterReminder();
                    this.Waiting = true;
                    this.State.Status = DepositSagaStatus.QueryAuthenticateResult;

                    return;
                }
            }

            this.State.Status = DepositSagaStatus.VerifyBankCard;
        }

        private async Task ProcessPayByYilianAsync()
        {
            if (this.State.InitData.PayByYilianCommand != null)
            {
                Tuple<UserInfo, SettleAccountTranscationInfo> info = await this.User.DepositAsync(this.State.InitData.PayByYilianCommand);

                UserInfo userInfo = info.Item1;
                SettleAccountTranscationInfo transcationInfo = info.Item2;

                if (transcationInfo != null)
                {
                    PaymentRequestParameter parameter = await this.BuildRequestParameterAsync(transcationInfo.TransactionId.ToGuidString(),
                    transcationInfo.BankCardInfo.CityName, transcationInfo.BankCardNo, userInfo.RealName, transcationInfo.BankCardInfo.BankName,
                    (int)userInfo.Credential, userInfo.CredentialNo, transcationInfo.BankCardInfo.Cellphone,
                    userInfo.UserId.ToGuidString(), transcationInfo.Amount);

                    YilianRequestResult result = await this.YilianService.PaymentRequestAsync(parameter);
                    this.Info.Add("Request-{0}".FormatWith(DateTime.UtcNow.ToString("O")), new { result.Message, result.ResponseString });

                    if (!result.Result)
                    {
                        throw new ApplicationException("Yilian request failed.");
                    }

                    await this.RegisterReminder();
                    this.Waiting = true;
                    this.State.Status = DepositSagaStatus.QueryYilianPaymentResult;

                    return;
                }
            }

            this.State.Status = DepositSagaStatus.Finished;
        }

        private async Task ProcessVerifyBankCardAsync()
        {
            if (this.State.InitData.VerifyBankCardCommand != null)
            {
                Tuple<UserInfo, BankCardInfo> info = await this.User.VerifyBankCardAsync(this.State.InitData.VerifyBankCardCommand);
                UserInfo userInfo = info.Item1;
                BankCardInfo bankCardInfo = info.Item2;

                if (userInfo.Verified && bankCardInfo.VerifiedByYilian)
                {
                    AuthRequestParameter parameter = await this.BuildRequestParameterAsync(bankCardInfo.CityName,
                        bankCardInfo.BankCardNo, userInfo.RealName, bankCardInfo.BankName, (int)userInfo.Credential,
                        userInfo.CredentialNo, bankCardInfo.Cellphone, userInfo.UserId.ToGuidString(), 'B');

                    YilianRequestResult result = await this.YilianService.AuthRequestAsync(parameter);
                    this.Info.Add("Request-{0}".FormatWith(DateTime.UtcNow.ToString("O")), new { result.Message, result.ResponseString });

                    if (!result.Result)
                    {
                        throw new ApplicationException("Yilian request failed.");
                    }

                    await this.RegisterReminder();
                    this.Waiting = true;
                    this.State.Status = DepositSagaStatus.QueryBankCardVerifiedResult;

                    return;
                }
            }

            this.State.Status = DepositSagaStatus.PayByYilian;
        }

        private async Task QueryAuthenticateResultAsync()
        {
            if (this.State.InitData.AuthenticateCommand == null)
            {
                throw new ApplicationException("Missing AuthenticateCommand SagaId-{0}.".FormatWith(this.State.SagaId.ToGuidString()));
            }

            YilianRequestResult result = await this.YilianService.QueryRequestAsync(this.State.SagaId.ToGuidString(), false);

            if (result == null)
            {
                this.Info = new Dictionary<string, object> { { "Query", new { Message = "Processing" } } };
            }
            else
            {
                this.Info = new Dictionary<string, object> { { "Query", new { result.Message, result.ResponseString } } };

                await this.UnregisterReminder();

                this.Waiting = false;
                this.State.Status = DepositSagaStatus.PayByYilian;

                await this.User.AuthenticateResultedAsync(this.State.InitData.AuthenticateCommand, result.Result, result.Message);
            }
        }

        private async Task QueryBankCardVerifiedResultAsync()
        {
            if (this.State.InitData.VerifyBankCardCommand == null)
            {
                throw new ApplicationException("Missing VerifyBankCardCommand SagaId-{0}.".FormatWith(this.State.SagaId.ToGuidString()));
            }

            YilianRequestResult result = await this.YilianService.QueryRequestAsync(this.State.SagaId.ToGuidString(), false);

            if (result == null)
            {
                this.Info = new Dictionary<string, object> { { "Query", new { Message = "Processing" } } };
            }
            else
            {
                this.Info = new Dictionary<string, object> { { "Query", new { result.Message, result.ResponseString } } };

                await this.UnregisterReminder();
                this.Waiting = false;
                this.State.Status = DepositSagaStatus.PayByYilian;

                await this.User.VerifyBankCardAsync(this.State.InitData.VerifyBankCardCommand, result.Result, result.Message);
            }
        }

        private async Task<PaymentRequestParameter> BuildRequestParameterAsync(string batchNo, string cityName, string bankCardNo, string realName, string bankName, int credential, string credentialNo, string cellphone, string userId, int amount)
        {
            ISequenceGenerator sequenceGenerator = SequenceGeneratorFactory.GetGrain(Guid.Empty);
            string sequenceNo = await sequenceGenerator.GenerateNoAsync('D');
            string[] address = cityName.Split('|');
            return new PaymentRequestParameter(batchNo, sequenceNo, bankCardNo, realName, address[0],
                address[1], bankName, credential, credentialNo, cellphone, userId,
                "YL" + DateTime.UtcNow.AddHours(8).Date.ToString("yyyyMMdd"), decimal.Divide(amount, 100));
        }
    }
}