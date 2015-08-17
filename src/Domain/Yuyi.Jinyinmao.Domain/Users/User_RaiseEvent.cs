// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : User_RaiseEvent.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  12:00
// ***********************************************************************
// <copyright file="User_RaiseEvent.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     User.
    /// </summary>
    public partial class User
    {
        /// <summary>
        ///     Raises the bank card added event.
        /// </summary>
        /// <param name="addBankCardCommand">The add bank card command.</param>
        /// <param name="bankCardInfo">The bank card information.</param>
        /// <returns>Task.</returns>
        public async Task RaiseBankCardAddedEvent(AddBankCard addBankCardCommand, BankCardInfo bankCardInfo)
        {
            BankCardAdded @event = new BankCardAdded
            {
                Args = addBankCardCommand.Args,
                BankCardInfo = bankCardInfo,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private Func<IEvent, Task> GetEventProcessing(Type evenType)
        {
            Dictionary<Type, Func<IEvent, Task>> eventProcessing = new Dictionary<Type, Func<IEvent, Task>>
            {
                { typeof(JBYAccountTransactionCanceled), e => this.GrainFactory.GetGrain<IJBYAccountTransactionCanceledProcessor>(e.EventId).ProcessEventAsync((JBYAccountTransactionCanceled)e) },
                { typeof(BankCardAdded), e => this.GrainFactory.GetGrain<IBankCardAddedProcessor>(e.EventId).ProcessEventAsync((BankCardAdded)e) },
                { typeof(AuthenticateResulted), e => this.GrainFactory.GetGrain<IAuthenticateResultedProcessor>(e.EventId).ProcessEventAsync((AuthenticateResulted)e) },
                { typeof(VerifyBankCardResulted), e => this.GrainFactory.GetGrain<IVerifyBankCardResultedProcessor>(e.EventId).ProcessEventAsync((VerifyBankCardResulted)e) },
                { typeof(PayingByYilian), e => this.GrainFactory.GetGrain<IPayingByYilianProcessor>(e.EventId).ProcessEventAsync((PayingByYilian)e) },
                { typeof(DepositResulted), e => this.GrainFactory.GetGrain<IDepositResultedProcessor>(e.EventId).ProcessEventAsync((DepositResulted)e) },
                { typeof(JBYPurchased), e => this.GrainFactory.GetGrain<IJBYPurchasedProcessor>(e.EventId).ProcessEventAsync((JBYPurchased)e) },
                { typeof(OrderPaid), e => this.GrainFactory.GetGrain<IOrderPaidProcessor>(e.EventId).ProcessEventAsync((OrderPaid)e) },
                { typeof(UserRegistered), e => this.GrainFactory.GetGrain<IUserRegisteredProcessor>(e.EventId).ProcessEventAsync((UserRegistered)e) },
                { typeof(OrderRepaid), e => this.GrainFactory.GetGrain<IOrderRepaidProcessor>(e.EventId).ProcessEventAsync((OrderRepaid)e) },
                { typeof(LoginPasswordReset), e => this.GrainFactory.GetGrain<ILoginPasswordResetProcessor>(e.EventId).ProcessEventAsync((LoginPasswordReset)e) },
                { typeof(PaymentPasswordReset), e => this.GrainFactory.GetGrain<IPaymentPasswordResetProcessor>(e.EventId).ProcessEventAsync((PaymentPasswordReset)e) },
                { typeof(PaymentPasswordSet), e => this.GrainFactory.GetGrain<IPaymentPasswordSetProcessor>(e.EventId).ProcessEventAsync((PaymentPasswordSet)e) },
                { typeof(WithdrawalAccepted), e => this.GrainFactory.GetGrain<IWithdrawalAcceptedProcessor>(e.EventId).ProcessEventAsync((WithdrawalAccepted)e) },
                { typeof(WithdrawalResulted), e => this.GrainFactory.GetGrain<IWithdrawalResultedProcessor>(e.EventId).ProcessEventAsync((WithdrawalResulted)e) },
                { typeof(JBYWithdrawalAccepted), e => this.GrainFactory.GetGrain<IJBYWithdrawalAcceptedProcessor>(e.EventId).ProcessEventAsync((JBYWithdrawalAccepted)e) },
                { typeof(JBYWithdrawalResulted), e => this.GrainFactory.GetGrain<IJBYWithdrawalResultedProcessor>(e.EventId).ProcessEventAsync((JBYWithdrawalResulted)e) },
                { typeof(JBYReinvested), e => this.GrainFactory.GetGrain<IJBYReinvestedProcessor>(e.EventId).ProcessEventAsync((JBYReinvested)e) },
                { typeof(BankCardHiden), e => this.GrainFactory.GetGrain<IBankCardHidenProcessor>(e.EventId).ProcessEventAsync((BankCardHiden)e) },
                { typeof(SettleAccountTransactionInserted), e => this.GrainFactory.GetGrain<ISettleAccountTransactionInsertedProcessor>(e.EventId).ProcessEventAsync((SettleAccountTransactionInserted)e) },
                { typeof(JBYAccountTransactionInserted), e => this.GrainFactory.GetGrain<IJBYAccountTransactionInsertedProcessor>(e.EventId).ProcessEventAsync((JBYAccountTransactionInserted)e) },
                { typeof(SettleAccountTransactionResulted), e => this.GrainFactory.GetGrain<ISettleAccountTransactionResultedProcessor>(e.EventId).ProcessEventAsync((SettleAccountTransactionResulted)e) },
                { typeof(JBYAccountTransactionResulted), e => this.GrainFactory.GetGrain<IJBYAccountTransactionResultedProcessor>(e.EventId).ProcessEventAsync((JBYAccountTransactionResulted)e) },
                { typeof(SettleAccountTransactionCanceled), e => this.GrainFactory.GetGrain<ISettleAccountTransactionCanceledProcessor>(e.EventId).ProcessEventAsync((SettleAccountTransactionCanceled)e) },
                { typeof(OrderTransfered), e => this.GrainFactory.GetGrain<IOrderTransferedProcessor>(e.EventId).ProcessEventAsync((OrderTransfered)e) },
                { typeof(JBYTransactionTransfered), e => this.GrainFactory.GetGrain<IJBYTransactionTransferedProcessor>(e.EventId).ProcessEventAsync((JBYTransactionTransfered)e) },
                { typeof(OrderCanceled), e => this.GrainFactory.GetGrain<IOrderCanceledProcessor>(e.EventId).ProcessEventAsync((OrderCanceled)e) },
                { typeof(UserSigned), e=> this.GrainFactory.GetGrain<IUserSignedProcessor>(e.EventId).ProcessEventAsync((UserSigned)e) }
            };

            return eventProcessing[evenType];
        }

        /// <summary>
        ///     Stores the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        private async Task ProcessEventAsync(Event @event)
        {
            @event.SourceId = this.State.UserId.ToGuidString();
            @event.SourceType = this.GetType().Name;
            @event.TimeStamp = DateTime.UtcNow;

            this.StoreEventAsync(@event);

            await this.GetEventProcessing(@event.GetType()).Invoke(@event);
        }

        /// <summary>
        ///     Raises the authentication resulted event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="bankCardInfo">The bank card information.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        private async Task RaiseAuthenticationResultedEvent(Authenticate command, BankCardInfo bankCardInfo, bool result, string message)
        {
            AuthenticateResulted @event = new AuthenticateResulted
            {
                Args = command.Args,
                BankCardInfo = bankCardInfo,
                Result = result,
                TranDesc = message,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the bank card hiden event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="bankCardInfo">The bank card information.</param>
        /// <returns>Task.</returns>
        private async Task RaiseBankCardHidenEvent(HideBankCard command, BankCardInfo bankCardInfo)
        {
            BankCardHiden @event = new BankCardHiden
            {
                Args = command.Args,
                BankCardInfo = bankCardInfo,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the deposit resulted event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="info">The information.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        private async Task RaiseDepositResultedEvent(Command command, SettleAccountTransactionInfo info, bool result, string message)
        {
            DepositResulted @event = new DepositResulted
            {
                Args = command.Args,
                Result = result,
                TransDesc = message,
                TransactionInfo = info,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        /// Raises the settle account transaction resulted event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="info">The information.</param>
        /// <returns>Task.</returns>
        private async Task RaiseJBYAccountTransactionResultedEvent(SetJBYAccountTransactionResult command, JBYAccountTransactionInfo info)
        {
            JBYAccountTransactionResulted @event = new JBYAccountTransactionResulted
            {
                Args = command.Args,
                Result = command.Result,
                TransDesc = command.TransDesc,
                TransactionInfo = info,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseJBYPurchasedEvent(JBYInvesting command, JBYAccountTransactionInfo jbyTransaction, SettleAccountTransactionInfo settleTransaction)
        {
            JBYPurchased @event = new JBYPurchased
            {
                Args = command.Args,
                JBYTransactionInfo = jbyTransaction,
                SettleTransactionInfo = settleTransaction,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseJBYReinvestedEvent(JBYAccountTransactionInfo jbyAccountTransactionInfo)
        {
            JBYReinvested @event = new JBYReinvested
            {
                Args = new Dictionary<string, object>(),
                TransactionInfo = jbyAccountTransactionInfo,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the jby transaction transfered event.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="jbyInfo">The jby information.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <returns>Task.</returns>
        private async Task RaiseJBYTransactionTransferedEvent(Dictionary<string, object> args, JBYAccountTransactionInfo jbyInfo, SettleAccountTransactionInfo transactionInfo)
        {
            JBYTransactionTransfered @event = new JBYTransactionTransfered
            {
                Args = args,
                JBYInfo = jbyInfo,
                TransactionInfo = transactionInfo,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseJBYWithdrawalAcceptedEvent(JBYWithdrawal command, JBYAccountTransactionInfo info)
        {
            JBYWithdrawalAccepted @event = new JBYWithdrawalAccepted
            {
                Args = command.Args,
                TransactionInfo = info,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseJBYWithdrawalResultedEvent(JBYAccountTransactionInfo jbyAccountTransactionInfo, SettleAccountTransactionInfo settleAccountTransactionInfo)
        {
            JBYWithdrawalResulted @event = new JBYWithdrawalResulted
            {
                Args = new Dictionary<string, object>(),
                JBYAccountTransactionInfo = jbyAccountTransactionInfo,
                SettleAccountTransactionInfo = settleAccountTransactionInfo,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
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
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the order canceled event.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="orderInfo">The order information.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <returns>Task.</returns>
        private async Task RaiseOrderCanceledEvent(Dictionary<string, object> args, OrderInfo orderInfo, SettleAccountTransactionInfo transactionInfo)
        {
            OrderCanceled @event = new OrderCanceled
            {
                Args = args,
                OrderInfo = orderInfo,
                TransactionInfo = transactionInfo,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the order paid event.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="orderInfo">The order information.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <returns>Task.</returns>
        private async Task RaiseOrderPaidEvent(Dictionary<string, object> args, OrderInfo orderInfo, SettleAccountTransactionInfo transactionInfo)
        {
            OrderPaid @event = new OrderPaid
            {
                Args = args,
                OrderInfo = orderInfo,
                TransactionInfo = transactionInfo,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the order repaid event.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="orderInfo">The order information.</param>
        /// <param name="principalTransactionInfo">The principal transaction information.</param>
        /// <param name="interestTransactionInfo">The interest transaction information.</param>
        private async Task RaiseOrderRepaidEvent(Dictionary<string, object> args, OrderInfo orderInfo, SettleAccountTransactionInfo principalTransactionInfo, SettleAccountTransactionInfo interestTransactionInfo)
        {
            OrderRepaid @event = new OrderRepaid
            {
                Args = args,
                InterestTransactionInfo = interestTransactionInfo,
                OrderInfo = orderInfo,
                PriIntSumAmount = principalTransactionInfo.Amount + interestTransactionInfo.Amount,
                PrincipalTransactionInfo = principalTransactionInfo,
                RepaidTime = orderInfo.ResultTime.GetValueOrDefault(),
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the order built event.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="orderInfo">The order information.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <returns>Task.</returns>
        private async Task RaiseOrderTransferedEvent(Dictionary<string, object> args, OrderInfo orderInfo, SettleAccountTransactionInfo transactionInfo)
        {
            OrderTransfered @event = new OrderTransfered
            {
                Args = args,
                OrderInfo = orderInfo,
                TransactionInfo = transactionInfo,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the paying by yilian event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <returns>Task.</returns>
        private async Task RaisePayingByYilianEvent(Command command, SettleAccountTransactionInfo transactionInfo)
        {
            PayingByYilian @event = new PayingByYilian
            {
                Args = command.Args,
                TransactionInfo = transactionInfo,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the payment password set event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        private async Task RaisePaymentPasswordSetEvent(SetPaymentPassword command)
        {
            Event @event;
            if (command.Override)
            {
                @event = new PaymentPasswordReset
                {
                    Args = command.Args,
                    UserInfo = await this.GetUserInfoAsync()
                };
            }
            else
            {
                @event = new PaymentPasswordSet
                {
                    Args = command.Args,
                    UserInfo = await this.GetUserInfoAsync()
                };
            }

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the settle account transaction resulted event.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="info">The information.</param>
        /// <returns>Task.</returns>
        private async Task RaiseSettleAccountTransactionCanceledEvent(Dictionary<string, object> args, SettleAccountTransactionInfo info)
        {
            SettleAccountTransactionCanceled @event = new SettleAccountTransactionCanceled
            {
                Args = args,
                TransactionInfo = info,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        /// Raises the settle account transaction resulted event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="info">The information.</param>
        /// <returns>Task.</returns>
        private async Task RaiseSettleAccountTransactionResultedEvent(SetSettleAccountTransactionResult command, SettleAccountTransactionInfo info)
        {
            SettleAccountTransactionResulted @event = new SettleAccountTransactionResulted
            {
                Args = command.Args,
                Result = command.Result,
                TransDesc = command.TransDesc,
                TransactionInfo = info,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseTransactionInsertdEvent(InsertSettleAccountTransaction command, SettleAccountTransactionInfo transactionInfo)
        {
            SettleAccountTransactionInserted @event = new SettleAccountTransactionInserted
            {
                Args = command.Args,
                TransactionInfo = transactionInfo
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseTransactionInsertdEvent(InsertJBYAccountTransaction command, JBYAccountTransactionInfo transactionInfo)
        {
            JBYAccountTransactionInserted @event = new JBYAccountTransactionInserted
            {
                Args = command.Args,
                TransactionInfo = transactionInfo
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the user registered event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        private async Task RaiseUserRegisteredEvent(UserRegister command)
        {
            UserRegistered @event = new UserRegistered
            {
                Args = command.Args,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseUserSignedEvent(Sign command, SettleAccountTransactionInfo settleAccountTransactionInfo)
        {
            UserSigned @event = new UserSigned
            {
                Args = command.Args,
                TransactionInfo = settleAccountTransactionInfo,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the verify bank card resulted event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="bankCardInfo">The bank card information.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        private async Task RaiseVerifyBankCardResultedEvent(VerifyBankCard command, BankCardInfo bankCardInfo, bool result, string message)
        {
            VerifyBankCardResulted @event = new VerifyBankCardResulted
            {
                Args = command.Args,
                BankCardInfo = bankCardInfo,
                Result = result,
                TranDesc = message,
                UserInfo = await this.GetUserInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseWithdrawalAcceptedEvent(Withdrawal command, SettleAccountTransactionInfo transaction, SettleAccountTransactionInfo chargeTransaction)
        {
            WithdrawalAccepted @event = new WithdrawalAccepted
            {
                Args = command.Args,
                ChargeTransaction = chargeTransaction,
                UserInfo = await this.GetUserInfoAsync(),
                WithdrawalTransaction = transaction
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the withdrawal resulted event.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>Task.</returns>
        private async Task RaiseWithdrawalResultedEvent(SettleAccountTransactionInfo transaction)
        {
            WithdrawalResulted @event = new WithdrawalResulted
            {
                Args = new Dictionary<string, object>(),
                UserInfo = await this.GetUserInfoAsync(),
                WithdrawalTransactionInfo = transaction
            };

            await this.ProcessEventAsync(@event);
        }
    }
}