// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  11:34 PM
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Products;
using Yuyi.Jinyinmao.Helper;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class User.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public partial class User : EntityGrain<IUserState>, IUser, IRemindable
    {
        /// <summary>
        ///     The daily withdrawal limit count
        /// </summary>
        private static readonly int DailyWithdrawalLimitCount = 100;

        /// <summary>
        ///     The month free withrawal limit count
        /// </summary>
        private static readonly int MonthFreeWithrawalLimitCount = 4;

        /// <summary>
        ///     The withdrawal charge fee
        /// </summary>
        private static readonly int WithdrawalChargeFee = 200;

        #region IUser Members

        /// <summary>
        ///     authenticate as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> AuthenticateAsync(Authenticate command)
        {
            if (!this.State.Verified)
            {
                this.BeginProcessCommandAsync(command).Forget();

                this.State.RealName = command.RealName;
                this.State.Credential = command.Credential;
                this.State.CredentialNo = command.CredentialNo;

                await this.State.WriteStateAsync();
            }

            return await this.GetUserInfoAsync();
        }

        /// <summary>
        ///     authenticate resulted as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        /// <exception cref="System.ApplicationException">
        ///     Invalid command {0}..FormatWith(command.CommandId)
        ///     or
        ///     Missing bank card information {0}..FormatWith(command.BankCardNo)
        /// </exception>
        public async Task<UserInfo> AuthenticateResultedAsync(Authenticate command, bool result, string message)
        {
            if (!this.State.Verified)
            {
                BankCard card;
                if (!this.State.BankCards.TryGetValue(command.BankCardNo, out card))
                {
                    throw new ApplicationException("Missing bank card information {0}.".FormatWith(command.BankCardNo));
                }

                DateTime now = DateTime.UtcNow.AddHours(8);

                if (result)
                {
                    this.State.RealName = command.RealName;
                    this.State.Credential = command.Credential;
                    this.State.CredentialNo = command.CredentialNo;
                    this.State.Verified = true;
                    this.State.VerifiedTime = now;
                    card.Verified = true;
                    card.VerifiedByYilian = true;
                    card.VerifiedTime = now;
                }
                else
                {
                    this.State.RealName = string.Empty;
                    this.State.Credential = Credential.None;
                    this.State.CredentialNo = string.Empty;
                    this.State.Verified = false;
                    this.State.VerifiedTime = null;
                }

                await this.State.WriteStateAsync();
                await this.RaiseAuthenticationResultedEvent(command, card.ToInfo(), result, message);
            }

            return await this.GetUserInfoAsync();
        }

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;CheckPasswordResult&gt;.</returns>
        public Task<CheckPasswordResult> CheckPasswordAsync(string loginName, string password)
        {
            if (this.State.Cellphone.IsNullOrEmpty())
            {
                return Task.FromResult(new CheckPasswordResult
                {
                    Success = false,
                    UserExist = false
                });
            }

            if (this.State.Cellphone == loginName || this.State.LoginNames.Contains(loginName))
            {
                if (CryptographyHelper.Check(password, this.State.Salt, this.State.EncryptedPassword))
                {
                    this.PasswordErrorCount = 0;
                    return Task.FromResult(new CheckPasswordResult
                    {
                        Cellphone = this.State.Cellphone,
                        ErrorCount = 0,
                        Success = true,
                        UserExist = true,
                        UserId = this.State.Id
                    });
                }
            }

            this.PasswordErrorCount += 1;

            return Task.FromResult(new CheckPasswordResult
            {
                Cellphone = this.State.Cellphone,
                ErrorCount = this.PasswordErrorCount,
                Success = false,
                UserExist = true,
                UserId = this.State.Id
            });
        }

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> CheckPasswordAsync(string password)
        {
            return Task.FromResult(CryptographyHelper.Check(password, this.State.Salt, this.State.EncryptedPassword));
        }

        /// <summary>
        ///     Checks the payment password asynchronous.
        /// </summary>
        /// <param name="paymentPassword">The payment password.</param>
        /// <returns>Task&lt;CheckPaymentPasswordResult&gt;.</returns>
        public Task<CheckPaymentPasswordResult> CheckPaymentPasswordAsync(string paymentPassword)
        {
            if (this.State.EncryptedPaymentPassword.IsNullOrEmpty())
            {
                return Task.FromResult(new CheckPaymentPasswordResult
                {
                    Success = false,
                    RemainCount = -1
                });
            }

            if (CryptographyHelper.Check(paymentPassword, this.State.PaymentSalt, this.State.EncryptedPaymentPassword))
            {
                this.PaymentPasswordErrorCount = 0;
                return Task.FromResult(new CheckPaymentPasswordResult
                {
                    Success = true,
                    RemainCount = 5 - this.PaymentPasswordErrorCount
                });
            }

            this.PasswordErrorCount += 1;

            return Task.FromResult(new CheckPaymentPasswordResult
            {
                Success = false,
                RemainCount = 5 - this.PaymentPasswordErrorCount
            });
        }

        /// <summary>
        ///     Clears the unauthenticated information.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task ClearUnauthenticatedInfo()
        {
            if (!this.State.Verified)
            {
                this.State.RealName = string.Empty;
                this.State.Credential = Credential.None;
                this.State.CredentialNo = string.Empty;
                this.State.VerifiedTime = null;
            }

            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     deposit resulted as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ApplicationException">
        ///     Missing SettleAccountTranscation {0}..FormatWith(command.CommandId)
        ///     or
        ///     Missing BankCard {0}..FormatWith(transcation.BankCardNo)
        /// </exception>
        public async Task DepositResultedAsync(PayByYilian command, bool result, string message)
        {
            if (!this.State.Verified)
            {
                throw new ApplicationException("Invalid PayByYilian command. UserId-{0}, CommandId-{1}.".FormatWith(this.State.Id, command.CommandId));
            }

            BankCard card;
            if (!this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                throw new ApplicationException("Missing BankCard data. UserId-{0}, CommandId-{1}".FormatWith(this.State.Id, command.CommandId));
            }

            SettleAccountTranscation transcation;
            if (!this.State.SettleAccount.TryGetValue(command.CommandId, out transcation))
            {
                throw new ApplicationException("Missing SettleAccountTranscation. UserId-{0}, CommandId-{1}, SettleAccountTranscationId-{2} ."
                    .FormatWith(this.State.Id, command.CommandId, command.CommandId));
            }

            if (transcation.ResultCode != 0 && transcation.ResultTime.HasValue)
            {
                return;
            }

            transcation.ResultCode = result ? 1 : -1;
            transcation.ResultTime = DateTime.UtcNow.AddHours(8);
            transcation.TransDesc = result ? "充值成功" : message;

            await this.State.WriteStateAsync();
            this.ReloadSettleAccountData();
            await this.RaiseDepositResultedEvent(command, transcation.ToInfo(card.ToInfo()), result, message);
        }

        /// <summary>
        ///     deposit as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;Tuple&lt;UserInfo, SettleAccountTranscationInfo&gt;&gt;.</returns>
        /// <exception cref="System.ApplicationException">
        ///     Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}.FormatWith(this.State.Id, command.CommandId)
        ///     or
        ///     Missing BankCard data. UserId-{0}, CommandId-{1}.FormatWith(this.State.Id, command.CommandId)
        /// </exception>
        public async Task<Tuple<UserInfo, SettleAccountTranscationInfo>> DepositAsync(PayByYilian command)
        {
            if (!this.State.Verified)
            {
                throw new ApplicationException("Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}".FormatWith(this.State.Id, command.CommandId));
            }

            BankCard card;
            if (!this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                throw new ApplicationException("Missing BankCard data. UserId-{0}, CommandId-{1}".FormatWith(this.State.Id, command.CommandId));
            }

            UserInfo userInfo = await this.GetUserInfoAsync();

            SettleAccountTranscation transcation;
            SettleAccountTranscationInfo transcationInfo = null;
            if (!this.State.SettleAccount.TryGetValue(command.CommandId, out transcation))
            {
                this.BeginProcessCommandAsync(command).Forget();

                transcation = new SettleAccountTranscation
                {
                    Amount = command.Amount,
                    Args = command.Args,
                    BankCardNo = command.BankCardNo,
                    ChannelCode = ChannelCodeHelper.Jinyinmao,
                    ResultCode = 0,
                    ResultTime = null,
                    Trade = Trade.Debit,
                    TradeCode = TradeCodeHelper.TC1005051001,
                    TransactionId = command.CommandId,
                    TransactionTime = DateTime.UtcNow.AddHours(8),
                    TransDesc = "充值申请",
                    UserId = this.State.Id
                };

                this.State.SettleAccount.Add(transcation.TransactionId, transcation);

                await this.State.WriteStateAsync();
                this.ReloadSettleAccountData();

                transcationInfo = transcation.ToInfo(card.ToInfo());

                await this.RaisePayingByYilianEvent(command, transcationInfo);
            }

            return new Tuple<UserInfo, SettleAccountTranscationInfo>(userInfo, transcationInfo);
        }

        /// <summary>
        ///     Gets the bank card information asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        public Task<BankCardInfo> GetBankCardInfoAsync(string bankCardNo)
        {
            BankCard card;
            if (this.State.BankCards.TryGetValue(bankCardNo, out card))
            {
                int withdrawAmount = this.SettleAccountBalance - this.State.BankCards.Values.Where(c => c.VerifiedByYilian).Sum(c => c.WithdrawAmount);
                withdrawAmount = withdrawAmount > 0 ? withdrawAmount + card.WithdrawAmount : card.WithdrawAmount;

                return Task.FromResult(card.ToInfo(withdrawAmount));
            }
            return Task.FromResult<BankCardInfo>(null);
        }

        /// <summary>
        ///     Gets the bank card infos asynchronous.
        /// </summary>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        public Task<List<BankCardInfo>> GetBankCardInfosAsync()
        {
            return Task.FromResult(this.State.BankCards.Values.Select(c => c.ToInfo()).ToList());
        }

        /// <summary>
        ///     Gets the order infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="ordersSortMode">The orders sort mode.</param>
        /// <param name="categories">The categories.</param>
        /// <returns>Task&lt;PaginatedList&lt;OrderInfo&gt;&gt;.</returns>
        public Task<PaginatedList<OrderInfo>> GetOrderInfosAsync(int pageIndex, int pageSize, OrdersSortMode ordersSortMode, long[] categories)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;

            IList<OrderInfo> orders = this.State.Orders.Values.Where(o => categories.Contains(o.ProductCategory)).ToList();

            int totalCount = orders.Count;
            orders = ordersSortMode == OrdersSortMode.ByOrderTimeDesc ?
                orders.OrderByDescending(o => o.OrderTime).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                : orders.OrderBy(o => o.SettleDate).ThenByDescending(o => o.OrderTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return Task.FromResult(new PaginatedList<OrderInfo>(pageIndex, pageSize, totalCount, orders));
        }

        /// <summary>
        ///     Gets the transcation information asynchronous.
        /// </summary>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task&lt;TranscationInfo&gt;.</returns>
        public Task<SettleAccountTranscationInfo> GetSettleAccountTranscationInfoAsync(Guid transcationId)
        {
            SettleAccountTranscation transcation;
            if (this.State.SettleAccount.TryGetValue(transcationId, out transcation))
            {
                BankCard card = this.State.BankCards[transcation.BankCardNo];
                return Task.FromResult(transcation.ToInfo(card.ToInfo()));
            }

            return Task.FromResult<SettleAccountTranscationInfo>(null);
        }

        /// <summary>
        ///     Gets the settle account transcation infos asynchronous.
        /// </summary>
        /// <returns>Task&lt;PaginatedList&lt;TranscationInfo&gt;&gt;.</returns>
        public Task<PaginatedList<SettleAccountTranscationInfo>> GetSettleAccountTranscationInfosAsync(int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;

            int totalCount = this.State.SettleAccount.Count;
            IList<SettleAccountTranscationInfo> items = this.State.SettleAccount.Values.OrderByDescending(t => t.TransactionTime)
                .Skip(pageIndex * pageSize).Take(pageSize).Select(t =>
                {
                    BankCard card = this.State.BankCards[t.BankCardNo];
                    return t.ToInfo(card.ToInfo());
                }).ToList();

            return Task.FromResult(new PaginatedList<SettleAccountTranscationInfo>(pageIndex, pageSize, totalCount, items));
        }

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetUserInfoAsync()
        {
            UserInfo userInfo = new UserInfo
            {
                Args = this.State.Args,
                Balance = this.SettleAccountBalance,
                BankCardsCount = this.State.BankCards.Count,
                Cellphone = this.State.Cellphone,
                ClientType = this.State.ClientType,
                Closed = this.State.Closed,
                ContractId = this.State.ContractId,
                Credential = this.State.Credential,
                CredentialNo = this.State.CredentialNo,
                Crediting = this.CreditingSettleAccountAmount,
                Debiting = this.DebitingSettleAccountAmount,
                HasSetPassword = this.State.EncryptedPassword.IsNotNullOrEmpty(),
                HasSetPaymentPassword = this.State.EncryptedPaymentPassword.IsNotNullOrEmpty(),
                InvestingInterest = this.InvestingInterest,
                InvestingPrincipal = this.InvestingPrincipal,
                InviteBy = this.State.InviteBy,
                JBYAccrualAmount = this.JBYAccrualAmount,
                JBYWithdrawalableAmount = this.JBYWithdrawalableAmount,
                LoginNames = this.State.LoginNames,
                MonthWithdrawalCount = this.MonthWithdrawalCount,
                OutletCode = this.State.OutletCode,
                PasswordErrorCount = this.PasswordErrorCount,
                RealName = this.State.RealName,
                RegisterTime = this.State.RegisterTime,
                TodayJBYWithdrawalAmount = this.TodayJBYWithdrawalAmount,
                TodayWithdrawalCount = this.TodayWithdrawalCount,
                TotalInterest = this.TotalInterest,
                TotalPrincipal = this.TotalPrincipal,
                UserId = this.State.Id,
                Verified = this.State.Verified,
                VerifiedTime = this.State.VerifiedTime
            };

            return Task.FromResult(userInfo);
        }

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task<OrderInfo> InvestingAsync(RegularInvesting command)
        {
            if (this.State.SettleAccount.ContainsKey(command.CommandId))
            {
                return null;
            }

            if (this.SettleAccountBalance < command.Amount)
            {
                return null;
            }

            this.BeginProcessCommandAsync(command).Forget();

            int tradeCode = ProductCategoryCodeHelper.IsJinyinmaoProduct(command.ProductCategory) ? TradeCodeHelper.TC1005012004 : TradeCodeHelper.TC1005022004;
            DateTime now = DateTime.UtcNow.AddHours(8);
            SettleAccountTranscation transcation = new SettleAccountTranscation
            {
                Amount = command.Amount,
                Args = command.Args,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                ResultCode = 1,
                ResultTime = now,
                Trade = Trade.Credit,
                TradeCode = tradeCode,
                TransDesc = "支付成功",
                TransactionId = command.CommandId,
                TransactionTime = now,
                UserId = this.State.Id
            };

            SettleAccountTranscationInfo transcationInfo = transcation.ToInfo(null);

            IRegularProduct product = RegularProductFactory.GetGrain(command.ProductId);
            OrderInfo orderInfo = await product.BuildOrderAsync(command, await this.GetUserInfoAsync(), transcationInfo);

            if (orderInfo == null)
            {
                return null;
            }

            this.State.Orders.Add(orderInfo.OrderId, orderInfo);

            this.State.SettleAccount.Add(transcation.TransactionId, transcation);

            await this.State.WriteStateAsync();
            this.ReloadSettleAccountData();
            this.ReloadOrderInfosData();

            await this.RaiseOrderPaidEvent(orderInfo, transcationInfo);

            return orderInfo;
        }

        /// <summary>
        ///     Determines whether [is registered] asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> IsRegisteredAsync()
        {
            return Task.FromResult(this.State.Cellphone.IsNotNullOrEmpty() && this.State.RegisterTime > DateTime.MinValue);
        }

        /// <summary>
        ///     Registers the specified user register.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task<UserInfo> RegisterAsync(UserRegister command)
        {
            if (this.State.Id == command.UserId)
            {
                return null;
            }

            if (this.State.Id != Guid.Empty)
            {
                this.GetLogger().Warn(1, "Conflict user id: UserId {0}, UserRegisterCommand.UserId {1}", this.State.Id, command.UserId);
                return null;
            }

            await this.BeginProcessCommandAsync(command);

            DateTime registerTime = DateTime.UtcNow.AddHours(8);
            this.State.Id = command.UserId;
            this.State.Args = command.Args;
            this.State.BankCards = new Dictionary<string, BankCard>();
            this.State.Cellphone = command.Cellphone;
            this.State.ClientType = command.ClientType;
            this.State.ContractId = command.ContractId;
            this.State.Credential = Credential.None;
            this.State.CredentialNo = string.Empty;
            this.State.EncryptedPassword = CryptographyHelper.Encrypting(command.Password, command.UserId.ToGuidString());
            this.State.EncryptedPaymentPassword = string.Empty;
            this.State.InviteBy = command.InviteBy;
            this.State.LoginNames = new List<string> { command.Cellphone };
            this.State.OutletCode = command.OutletCode;
            this.State.PaymentSalt = string.Empty;
            this.State.RealName = string.Empty;
            this.State.RegisterTime = registerTime;
            this.State.Salt = command.UserId.ToGuidString();
            this.State.Verified = false;
            this.State.VerifiedTime = null;
            this.State.JBYAccount = new Dictionary<Guid, JBYAccountTranscation>();
            this.State.SettleAccount = new Dictionary<Guid, SettleAccountTranscation>();

            await this.State.WriteStateAsync();

            await this.RegisterOrUpdateReminder("DailyWork", TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(10));

            await this.RaiseUserRegisteredEvent(command);

            return await this.GetUserInfoAsync();
        }

        /// <summary>
        ///     Repays the order asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="repaidTime">The repaid time.</param>
        /// <returns>Task.</returns>
        public async Task RepayOrderAsync(Guid orderId, DateTime repaidTime)
        {
            OrderInfo order;
            if (!this.State.Orders.TryGetValue(orderId, out order))
            {
                throw new ApplicationException("Missing OrderInfo data. UserId-{0}, OrderId-{1}.".FormatWith(this.State.Id, orderId));
            }

            if (order.IsRepaid)
            {
                return;
            }

            DateTime now = DateTime.UtcNow.AddHours(8);

            order.IsRepaid = true;
            order.RepaidTime = repaidTime;

            int principalTradeCode;
            int interestTradeCode;
            if (ProductCategoryCodeHelper.IsJinyinmaoRegularProduct(order.ProductCategory))
            {
                principalTradeCode = TradeCodeHelper.TC1005011104;
                interestTradeCode = TradeCodeHelper.TC1005011105;
            }
            else
            {
                principalTradeCode = TradeCodeHelper.TC1005021104;
                interestTradeCode = TradeCodeHelper.TC1005021105;
            }

            SettleAccountTranscation principalTranscation = new SettleAccountTranscation
            {
                Amount = order.Principal,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                ResultCode = 1,
                ResultTime = now,
                Trade = Trade.Debit,
                TradeCode = principalTradeCode,
                TransactionId = Guid.NewGuid(),
                TransDesc = "本金还款",
                TransactionTime = now,
                UserId = this.State.Id,
                Args = new Dictionary<string, object>()
            };

            SettleAccountTranscation interestTranscation = new SettleAccountTranscation
            {
                Amount = order.Interest + order.ExtraInterest,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                ResultCode = 1,
                ResultTime = now,
                Trade = Trade.Debit,
                TradeCode = interestTradeCode,
                TransactionId = Guid.NewGuid(),
                TransDesc = "产品结息",
                TransactionTime = now,
                UserId = this.State.Id,
                Args = new Dictionary<string, object>()
            };

            this.State.SettleAccount.Add(principalTranscation.TransactionId, principalTranscation);
            this.State.SettleAccount.Add(interestTranscation.TransactionId, interestTranscation);

            await this.State.WriteStateAsync();
            this.ReloadSettleAccountData();
            this.ReloadOrderInfosData();

            this.RaiseOrderRepaidEvent(order, principalTranscation.ToInfo(null), interestTranscation.ToInfo(null));
        }

        /// <summary>
        ///     Resets the login password.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task ResetLoginPasswordAsync(ResetLoginPassword command)
        {
            this.BeginProcessCommandAsync(command).Forget();

            this.State.Salt = command.Salt;
            this.State.EncryptedPassword = CryptographyHelper.Encrypting(command.Password, command.Salt);

            await this.State.WriteStateAsync();

            await this.RaiseLoginPasswordResetEvent(command);
        }

        /// <summary>
        ///     Sets the payment password asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task SetPaymentPasswordAsync(SetPaymentPassword command)
        {
            if (this.State.EncryptedPaymentPassword.IsNotNullOrEmpty() && !command.Override)
            {
                return;
            }

            this.BeginProcessCommandAsync(command).Forget();

            this.State.PaymentSalt = command.Salt;
            this.State.EncryptedPaymentPassword = CryptographyHelper.Encrypting(command.PaymentPassword, command.Salt);

            await this.State.WriteStateAsync();

            await this.RaisePaymentPasswordSetEvent(command);
        }

        /// <summary>
        ///     verify bank card as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;Tuple&lt;UserInfo, BankCardInfo&gt;&gt;.</returns>
        /// <exception cref="System.ApplicationException">
        ///     Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}.FormatWith(this.State.Id, command.CommandId)
        ///     or
        ///     Missing BankCard data. UserId-{0}, CommandId-{1}.FormatWith(this.State.Id, command.CommandId)
        /// </exception>
        public async Task<Tuple<UserInfo, BankCardInfo>> VerifyBankCardAsync(VerifyBankCard command)
        {
            if (!this.State.Verified)
            {
                throw new ApplicationException("Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}".FormatWith(this.State.Id, command.CommandId));
            }

            BankCard card;
            if (!this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                throw new ApplicationException("Missing BankCard data. UserId-{0}, CommandId-{1}".FormatWith(this.State.Id, command.CommandId));
            }

            this.BeginProcessCommandAsync(command).Forget();

            UserInfo userInfo = await this.GetUserInfoAsync();
            BankCardInfo bankCardInfo = card.ToInfo();

            return new Tuple<UserInfo, BankCardInfo>(userInfo, bankCardInfo);
        }

        /// <summary>
        ///     verify bank card as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ApplicationException">
        ///     Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}..FormatWith(this.State.Id, command.CommandId)
        ///     or
        ///     Missing BankCard data. UserId-{0}, CommandId-{1}.FormatWith(this.State.Id, command.CommandId)
        /// </exception>
        public async Task VerifyBankCardAsync(VerifyBankCard command, bool result, string message)
        {
            if (!this.State.Verified)
            {
                throw new ApplicationException("Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}.".FormatWith(this.State.Id, command.CommandId));
            }

            BankCard card;
            if (!this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                throw new ApplicationException("Missing BankCard data. UserId-{0}, CommandId-{1}".FormatWith(this.State.Id, command.CommandId));
            }

            if (card.VerifiedByYilian)
            {
                return;
            }

            if (result)
            {
                card.Verified = true;
                card.VerifiedByYilian = true;
                card.VerifiedTime = DateTime.UtcNow.AddHours(8);
            }

            await this.State.WriteStateAsync();
            await this.RaiseVerifyBankCardResultedEvent(command, card.ToInfo(), result, message);
        }

        /// <summary>
        ///     Withdrawals the resulted asynchronous.
        /// </summary>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task.</returns>
        public async Task WithdrawalResultedAsync(Guid transcationId)
        {
            SettleAccountTranscation transcation;
            if (!this.State.SettleAccount.TryGetValue(transcationId, out transcation))
            {
                return;
            }

            if (transcation.ResultCode != 0 || transcation.TradeCode != TradeCodeHelper.TC1005052001)
            {
                return;
            }

            transcation.ResultCode = 1;
            transcation.ResultTime = DateTime.UtcNow.AddHours(8);
            transcation.TransDesc = "取现成功";

            await this.State.WriteStateAsync();
            this.ReloadSettleAccountData();

            await this.RaiseWithdrawalResultedEvent(transcation.ToInfo(this.State.BankCards[transcation.BankCardNo].ToInfo()));
        }

        /// <summary>
        /// jby withdrawal resulted as an asynchronous operation.
        /// </summary>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task.</returns>
        public async Task JBYWithdrawalResultedAsync(Guid transcationId)
        {
            JBYAccountTranscation transcation;
            if (!this.State.JBYAccount.TryGetValue(transcationId, out transcation))
            {
                return;
            }

            if (transcation.ResultCode != 0 || transcation.TradeCode != TradeCodeHelper.TC2001012002)
            {
                return;
            }

            DateTime now = DateTime.UtcNow.AddHours(8);

            transcation.ResultCode = 1;
            transcation.ResultTime = now;
            transcation.TransDesc = "赎回成功";

            SettleAccountTranscation settleAccountTranscation = new SettleAccountTranscation
            {
                Amount = transcation.Amount,
                Args = transcation.Args,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                ResultCode = 1,
                ResultTime = now,
                Trade = Trade.Debit,
                TradeCode = TradeCodeHelper.TC1005011103,
                TransactionId = transcation.SettleAccountTranscationId,
                TransactionTime = now,
                TransDesc = "金包银赎回资金到账",
                UserId = this.State.Id
            };

            this.State.SettleAccount.Add(settleAccountTranscation.TransactionId, settleAccountTranscation);

            await this.State.WriteStateAsync();
            this.ReloadJBYAccountData();
            this.ReloadSettleAccountData();

            await this.RaiseJBYWithdrawalResultedEvent(transcation.ToInfo(), settleAccountTranscation.ToInfo(null));
        }

        /// <summary>
        /// jby compute interest as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task JBYReinvestingAsync()
        {
            DateTime now = DateTime.UtcNow.AddHours(8);
            if (this.State.JBYAccount.Values.Any(t => t.TradeCode == TradeCodeHelper.TC2001011106 && t.ResultCode > 0
                && t.TransactionTime.Date == now.Date))
            {
                return;
            }

            int yield = DailyConfigHelper.GetDailyConfig(now.AddDays(-1)).JBYYield;

            int interest = this.JBYAccrualAmount * yield / 3600000;

            JBYAccountTranscation jbyTranscation = new JBYAccountTranscation
            {
                Amount = interest,
                Args = new Dictionary<string, object> { { "Yield", yield } },
                PredeterminedResultDate = now,
                ProductId = Guid.Empty,
                ResultCode = 1,
                ResultTime = now,
                SettleAccountTranscationId = Guid.Empty,
                Trade = Trade.Debit,
                TradeCode = TradeCodeHelper.TC2001011106,
                TransDesc = "利息复投成功",
                TransactionId = Guid.NewGuid(),
                TransactionTime = now,
                UserId = this.State.Id
            };

            this.State.JBYAccount.Add(jbyTranscation.TransactionId, jbyTranscation);

            await this.State.WriteStateAsync();
            this.ReloadJBYAccountData();

            await this.RaiseJBYReinvestedEvent(jbyTranscation.ToInfo());
        }

        /// <summary>
        ///     Adds bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        public async Task<BankCardInfo> AddBankCardAsync(AddBankCard command)
        {
            BankCard card;
            if (!this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                if (this.State.BankCards.Count >= 10)
                {
                    return null;
                }

                this.BeginProcessCommandAsync(command).Forget();

                card = new BankCard
                {
                    AddingTime = DateTime.Now.AddHours(8),
                    Args = command.Args,
                    BankCardNo = command.BankCardNo,
                    BankName = command.BankName,
                    Cellphone = this.State.Cellphone,
                    CityName = command.CityName,
                    Dispaly = true,
                    UserId = this.State.Id,
                    Verified = false,
                    VerifiedByYilian = false,
                    VerifiedTime = null,
                    WithdrawAmount = 0
                };

                this.State.BankCards.Add(card.BankCardNo, card);

                await this.State.WriteStateAsync();

                await this.RaiseBankCardAddedEvent(command, card.ToInfo());
            }

            return card.ToInfo();
        }

        /// <summary>
        ///     Gets the jby account information asynchronous.
        /// </summary>
        /// <returns>Task&lt;JBYAccountInfo&gt;.</returns>
        public Task<JBYAccountInfo> GetJBYAccountInfoAsync()
        {
            return Task.FromResult(new JBYAccountInfo
            {
                JBYAccrualAmount = this.JBYAccrualAmount,
                JBYTotalAmount = this.JBYTotalAmount,
                JBYTotalInterest = this.JBYTotalInterest,
                JBYTotalPricipal = this.JBYTotalPricipal,
                JBYWithdrawalableAmount = this.JBYWithdrawalableAmount,
                TodayJBYWithdrawalAmount = this.TodayJBYWithdrawalAmount
            });
        }

        public Task<JBYAccountTranscationInfo> GetJBYAccountTranscationInfoAsync(Guid transcationId)
        {
            JBYAccountTranscation transcation;
            if (this.State.JBYAccount.TryGetValue(transcationId, out transcation))
            {
                return Task.FromResult(transcation.ToInfo());
            }

            return Task.FromResult<JBYAccountTranscationInfo>(null);
        }

        /// <summary>
        ///     Gets the jby account transcation infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;TranscationInfo&gt;&gt;.</returns>
        public Task<PaginatedList<JBYAccountTranscationInfo>> GetJBYAccountTranscationInfosAsync(int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;

            int totalCount = this.State.JBYAccount.Count;
            IList<JBYAccountTranscationInfo> items = this.State.JBYAccount.Values.OrderByDescending(t => t.TransactionTime)
                .Skip(pageIndex * pageSize).Take(pageSize).Select(t => t.ToInfo()).ToList();

            return Task.FromResult(new PaginatedList<JBYAccountTranscationInfo>(pageIndex, pageSize, totalCount, items));
        }

        /// <summary>
        ///     Gets the settle account information asynchronous.
        /// </summary>
        /// <returns>Task&lt;SettleAccountInfo&gt;.</returns>
        public Task<SettleAccountInfo> GetSettleAccountInfoAsync()
        {
            return Task.FromResult(new SettleAccountInfo
            {
                Balance = this.SettleAccountBalance,
                Crediting = this.CreditingSettleAccountAmount,
                Debiting = this.DebitingSettleAccountAmount,
                MonthWithdrawalCount = this.MonthWithdrawalCount,
                TodayWithdrawalCount = this.TodayWithdrawalCount
            });
        }

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;TranscationInfo&gt;.</returns>
        public async Task<JBYAccountTranscationInfo> InvestingAsync(JBYInvesting command)
        {
            if (this.State.JBYAccount.ContainsKey(command.CommandId))
            {
                return null;
            }

            if (this.SettleAccountBalance < command.Amount)
            {
                return null;
            }

            this.BeginProcessCommandAsync(command).Forget();

            DateTime now = DateTime.UtcNow.AddHours(8);
            Guid jbyTranscationId = Guid.NewGuid();

            SettleAccountTranscation settleTranscation = new SettleAccountTranscation
            {
                Amount = command.Amount,
                Args = command.Args,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                ResultCode = 1,
                ResultTime = now,
                Trade = Trade.Credit,
                TradeCode = TradeCodeHelper.TC1005012003,
                TransDesc = "支付成功",
                TransactionId = command.CommandId,
                TransactionTime = now,
                UserId = this.State.Id
            };

            JBYAccountTranscation jbyTranscation = new JBYAccountTranscation
            {
                Amount = command.Amount,
                Args = command.Args,
                PredeterminedResultDate = now,
                ResultCode = 1,
                ResultTime = now,
                SettleAccountTranscationId = settleTranscation.TransactionId,
                Trade = Trade.Debit,
                TradeCode = TradeCodeHelper.TC2001051102,
                TransDesc = "申购成功",
                TransactionId = jbyTranscationId,
                TransactionTime = now,
                UserId = this.State.Id
            };

            IJBYProduct product = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
            Guid? result = await product.BuildJBYTranscationAsync(jbyTranscation.ToInfo());
            if (result.HasValue)
            {
                jbyTranscation.ProductId = result.Value;
                this.State.SettleAccount.Add(settleTranscation.TransactionId, settleTranscation);
                this.State.JBYAccount.Add(jbyTranscation.TransactionId, jbyTranscation);

                await this.State.WriteStateAsync();
                this.ReloadSettleAccountData();
                this.ReloadJBYAccountData();

                await this.RaiseJBYPurchasedEvent(command, jbyTranscation.ToInfo(), settleTranscation.ToInfo(null));

                return jbyTranscation.ToInfo();
            }

            return null;
        }

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task<SettleAccountTranscationInfo> WithdrawalAsync(Withdrawal command)
        {
            if (!this.State.Verified)
            {
                return null;
            }

            BankCard card;
            if (!this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                return null;
            }

            if (this.State.SettleAccount.ContainsKey(command.CommandId))
            {
                return null;
            }

            if (this.SettleAccountBalance < command.Amount)
            {
                return null;
            }

            BankCardInfo info = await this.GetBankCardInfoAsync(command.BankCardNo);
            if (info.WithdrawAmount < command.Amount)
            {
                return null;
            }

            if (!info.VerifiedByYilian)
            {
                return null;
            }

            if (this.TodayWithdrawalCount >= DailyWithdrawalLimitCount)
            {
                return null;
            }

            this.BeginProcessCommandAsync(command).Forget();

            SettleAccountTranscation transcation = new SettleAccountTranscation
            {
                Amount = command.Amount,
                Args = command.Args,
                BankCardNo = command.BankCardNo,
                ChannelCode = ChannelCodeHelper.Yilian,
                ResultCode = 0,
                ResultTime = null,
                Trade = Trade.Credit,
                TradeCode = TradeCodeHelper.TC1005052001,
                TransactionId = command.CommandId,
                TransactionTime = DateTime.UtcNow.AddHours(8),
                TransDesc = "取现申请",
                UserId = this.State.Id
            };

            SettleAccountTranscation chargeTranscation = this.BuildChargeTranscation(command, transcation);

            this.State.SettleAccount.Add(transcation.TransactionId, transcation);
            this.State.SettleAccount.Add(chargeTranscation.TransactionId, chargeTranscation);

            await this.State.WriteStateAsync();
            this.ReloadSettleAccountData();

            await this.RaiseWithdrawalAcceptedEvent(command, transcation.ToInfo(card.ToInfo()), chargeTranscation.ToInfo(null));

            return transcation.ToInfo(null);
        }

        public async Task<JBYAccountTranscationInfo> WithdrawalAsync(JBYWithdrawal command)
        {
            JBYAccountTranscation transcation;
            if (this.State.JBYAccount.TryGetValue(command.CommandId, out transcation))
            {
                return null;
            }

            if (this.JBYWithdrawalableAmount < command.Amount)
            {
                return null;
            }

            this.BeginProcessCommandAsync(command).Forget();

            transcation = new JBYAccountTranscation
            {
                Amount = command.Amount,
                Args = command.Args,
                PredeterminedResultDate = null,
                ProductId = Guid.NewGuid(),
                ResultCode = 0,
                ResultTime = null,
                SettleAccountTranscationId = Guid.NewGuid(),
                Trade = Trade.Credit,
                TradeCode = TradeCodeHelper.TC1005052001,
                TransactionId = command.CommandId,
                TransactionTime = DateTime.UtcNow.AddHours(8),
                TransDesc = "赎回申请",
                UserId = this.State.Id
            };

            IJBYProductWithdrawalManager manager = JBYProductWithdrawalManagerFactory.GetGrain(GrainTypeHelper.GetJBYProductWithdrawalManagerGrainTypeLongKey());
            DateTime? predeterminedResultDate = await manager.BuildWithdrawalTranscationAsync(transcation.ToInfo());

            if (predeterminedResultDate == null)
            {
                return null;
            }

            transcation.PredeterminedResultDate = predeterminedResultDate;

            this.State.JBYAccount.Add(transcation.TransactionId, transcation);

            await this.State.WriteStateAsync();
            this.ReloadJBYAccountData();

            await this.RaiseJBYWithdrawalAcceptedEvent(command, transcation.ToInfo());

            return transcation.ToInfo();
        }

        #endregion IUser Members

        /// <summary>
        ///     Builds the charge transcation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transcation">The transcation.</param>
        /// <returns>Transcation.</returns>
        private SettleAccountTranscation BuildChargeTranscation(Withdrawal command, SettleAccountTranscation transcation)
        {
            DateTime now = DateTime.UtcNow.AddHours(8).AddSeconds(1);
            int chargeAmount;

            if (this.MonthWithdrawalCount < MonthFreeWithrawalLimitCount)
            {
                chargeAmount = 0;
            }
            else
            {
                chargeAmount = this.SettleAccountBalance - transcation.Amount;
            }

            chargeAmount = chargeAmount > WithdrawalChargeFee ? WithdrawalChargeFee : chargeAmount;

            return new SettleAccountTranscation
            {
                Amount = chargeAmount,
                Args = command.Args,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                ResultCode = 1,
                ResultTime = now,
                Trade = Trade.Credit,
                TradeCode = TradeCodeHelper.TC1005012102,
                TransDesc = "账户取现手续费",
                TransactionId = Guid.NewGuid(),
                TransactionTime = now,
                UserId = this.State.Id
            };
        }
    }
}