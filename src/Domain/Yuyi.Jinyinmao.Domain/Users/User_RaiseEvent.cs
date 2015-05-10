// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-07  12:20 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-10  7:12 PM
// ***********************************************************************
// <copyright file="User_RaiseEvent.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.EventProcessor;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Packages.Helper;
using Yuyi.Jinyinmao.Service;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     User.
    /// </summary>
    public partial class User
    {
        /// <summary>
        ///     Raises the add bank card resulted event.
        /// </summary>
        /// <param name="dto">The AddBankCardSagaInitDto.</param>
        /// <param name="result">The result.</param>
        /// <param name="isDefault">if set to <c>true</c> [is default].</param>
        /// <returns>Task.</returns>
        private async Task RaiseAddBankCardResultedEvent(AddBankCardSagaInitDto dto, YilianRequestResult result, bool isDefault)
        {
            AddBankCardResulted @event = new AddBankCardResulted
            {
                Args = dto.Command.Args,
                BankCardNo = dto.Command.BankCardNo,
                BankName = dto.Command.BankName,
                CanBeUsedByYilian = true,
                Cellphone = this.State.Cellphone,
                CityName = dto.Command.CityName,
                IsDefault = isDefault,
                Result = result.Result,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                TranDesc = result.Message,
                UserId = this.State.Id,
                Verified = result.Result,
                VerifiedTime = DateTime.UtcNow.AddHours(8)
            };
            await this.StoreEventAsync(@event);

            await AddBankCardResultedProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the apply for authentication resulted event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The result.</param>
        /// <returns>Task.</returns>
        private async Task RaiseApplyForAuthenticationResultedEvent(Authenticate command, YilianRequestResult result)
        {
            DateTime now = DateTime.UtcNow.AddHours(8);

            AuthenticateResulted @event = new AuthenticateResulted
            {
                Args = command.Args,
                Cellphone = command.Cellphone,
                Credential = command.Credential,
                CredentialNo = command.CredentialNo,
                RealName = command.RealName,
                Result = result.Result,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                TranDesc = result.Message,
                UserId = this.State.Id,
                Verified = result.Result,
                VerifiedTime = now
            };

            await this.StoreEventAsync(@event);

            await AuthenticateResultedProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);

            await this.RaiseAddBankCardResultedEvent(new AddBankCardSagaInitDto
            {
                Command = new AddBankCard
                {
                    Args = command.Args,
                    BankCardNo = command.BankCardNo,
                    BankName = command.BankName,
                    CityName = command.CityName,
                    CommandId = command.CommandId,
                    UserId = this.State.Id
                },
                UserInfo = await this.GetUserInfoAsync()
            }, result, true);
        }

        /// <summary>
        ///     Raises the deposit resulted event.
        /// </summary>
        /// <param name="dto">The DepositFromYilianSagaInitDto.</param>
        /// <param name="result">The result.</param>
        /// <returns>Task.</returns>
        private async Task RaiseDepositResultedEvent(DepositFromYilianSagaInitDto dto, YilianRequestResult result)
        {
            Transcation transcation = this.State.SettleAccount.First(t => t.TransactionId == dto.TranscationInfo.TransactionId);

            DepositFromYilianResulted @event = new DepositFromYilianResulted
            {
                Amount = dto.Command.Amount,
                Args = dto.Command.Args,
                BankCardNo = dto.BackCardInfo.BankCardNo,
                BankName = dto.BackCardInfo.BankName,
                Cellphone = dto.UserInfo.Cellphone,
                ChannalCode = transcation.ChannelCode,
                CityName = dto.BackCardInfo.CityName,
                Credential = dto.UserInfo.Credential,
                CredentialNo = dto.UserInfo.CredentialNo,
                RealName = dto.UserInfo.RealName,
                Result = result.Result,
                ResultCode = transcation.ResultCode,
                ResultTime = transcation.ResultTime.GetValueOrDefault(),
                SettleAccountBalance = this.SettleAccountBalance,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                Trade = transcation.Trade,
                TranscationId = transcation.TransactionId,
                TranscationTime = transcation.TransactionTime,
                TransDesc = transcation.TransDesc,
                UserId = this.State.Id
            };

            await this.StoreEventAsync(@event);

            await DepositFromYilianResultedProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the login password reset event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        private async Task RaiseLoginPasswordResetEvent(ResetLoginPassword command)
        {
            LoginPasswordReset @event = new LoginPasswordReset
            {
                Args = command.Args,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name
            };

            await this.StoreEventAsync(@event);

            await LoginPasswordResetProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the order built event.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="transcation">The transcation.</param>
        /// <returns>Task.</returns>
        private async Task RaiseOrderPaidEvent(OrderInfo order, TranscationInfo transcation)
        {
            OrderPaid @event = new OrderPaid
            {
                Args = JsonHelper.NewDictionary,
                Order = order,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                Transcation = transcation
            };

            await this.StoreEventAsync(@event);

            await OrderPaidProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the order repaid event.
        /// </summary>
        /// <param name="orderInfo">The order information.</param>
        /// <param name="principalTranscationInfo">The principal transcation information.</param>
        /// <param name="interestTranscationInfo">The interest transcation information.</param>
        private async void RaiseOrderRepaidEvent(OrderInfo orderInfo, TranscationInfo principalTranscationInfo, TranscationInfo interestTranscationInfo)
        {
            OrderRepaid @event = new OrderRepaid
            {
                Args = JsonHelper.NewDictionary,
                InterestTranscationInfo = interestTranscationInfo,
                OrderInfo = orderInfo,
                PriIntSumAmount = principalTranscationInfo.Amount + interestTranscationInfo.Amount,
                PrincipalTranscationInfo = principalTranscationInfo,
                RepaidTime = orderInfo.ResultTime.GetValueOrDefault(),
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name
            };

            await this.StoreEventAsync(@event);

            await OrderRepaidProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the payment password set event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        private async Task RaisePaymentPasswordSetEvent(SetPaymentPassword command)
        {
            if (command.Override)
            {
                PaymentPasswordReset @event = new PaymentPasswordReset
                {
                    Args = command.Args,
                    SourceId = this.State.Id.ToGuidString(),
                    SourceType = this.GetType().Name
                };

                await this.StoreEventAsync(@event);

                await PaymentPasswordResetProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
            }
            else
            {
                PaymentPasswordSet @event = new PaymentPasswordSet
                {
                    Args = command.Args,
                    SourceId = this.State.Id.ToGuidString(),
                    SourceType = this.GetType().Name
                };

                await this.StoreEventAsync(new PaymentPasswordSet
                {
                    Args = command.Args,
                    SourceId = this.State.Id.ToGuidString(),
                    SourceType = this.GetType().Name
                });

                await PaymentPasswordSetProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
            }
        }

        /// <summary>
        ///     Raises the user registered event.
        /// </summary>
        /// <param name="userRegister">The user register.</param>
        /// <returns>Task.</returns>
        private async Task RaiseUserRegisteredEvent(UserRegister userRegister)
        {
            UserRegistered @event = new UserRegistered
            {
                Args = userRegister.Args,
                Cellphone = this.State.Cellphone,
                ClientType = this.State.ClientType,
                ContractId = this.State.ContractId,
                InviteBy = this.State.InviteBy,
                LoginNames = this.State.LoginNames,
                OutletCode = userRegister.OutletCode,
                RegisterTime = this.State.RegisterTime,
                UserId = userRegister.UserId,
                SourceId = this.GetPrimaryKey().ToGuidString(),
                SourceType = this.GetType().Name
            };

            await this.StoreEventAsync(@event);

            await UserRegisteredProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the withdrawal resulted event.
        /// </summary>
        /// <param name="transcation">The transcation.</param>
        /// <returns>Task.</returns>
        private async Task RaiseWithdrawalResultedEvent(Transcation transcation)
        {
            BankCardInfo info = await this.GetBankCardInfoAsync(transcation.BankCardNo);

            WithdrawalResulted @event = new WithdrawalResulted
            {
                Amount = transcation.Amount,
                Args = JsonHelper.NewDictionary,
                BankCardNo = transcation.BankCardNo,
                BankName = info.BankName,
                Cellphone = this.State.Cellphone,
                CityName = info.CityName,
                Credential = this.State.Credential,
                CredentialNo = this.State.CredentialNo,
                RealName = this.State.RealName,
                Result = transcation.ResultCode == 1,
                ResultCode = transcation.ResultCode,
                ResultTime = transcation.ResultTime.GetValueOrDefault(),
                SettleAccountBalance = this.SettleAccountBalance,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                Trade = transcation.Trade,
                TransDesc = transcation.TransDesc,
                TranscationId = transcation.TransactionId,
                TranscationTime = transcation.TransactionTime,
                UserId = this.State.Id
            };

            await this.StoreEventAsync(@event);

            await WithdrawalResultedProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }
    }
}