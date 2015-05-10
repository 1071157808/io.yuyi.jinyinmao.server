// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-10  12:22 PM
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
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Domain.Sagas;
using Yuyi.Jinyinmao.Helper;
using Yuyi.Jinyinmao.Packages.Helper;
using Yuyi.Jinyinmao.Service;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class User.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public partial class User : EntityGrain<IUserState>, IUser
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
        ///     Adds the bank card.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task AddBankCardAsync(AddBankCard command)
        {
            if (!this.State.Verified)
            {
                return;
            }

            if (this.BankCards.ContainsKey(command.BankCardNo))
            {
                return;
            }

            if (this.BankCards.Count >= 10)
            {
                return;
            }

            await this.BeginProcessCommandAsync(command);

            this.State.BankCards.Add(new BankCard
            {
                BankCardNo = command.BankCardNo,
                BankName = command.BankName,
                Cellphone = this.State.Cellphone,
                CityName = command.CityName,
                IsDefault = false,
                Verified = false,
                VerifiedByYilian = false,
                VerifiedTime = null
            });

            await this.State.WriteStateAsync();

            IAddBankCardSaga saga = AddBankCardSagaFactory.GetGrain(Guid.NewGuid());
            await saga.BeginProcessAsync(new AddBankCardSagaInitDto
            {
                Command = command,
                UserInfo = await this.GetUserInfoAsync()
            });
        }

        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="dto">The add bank card saga initialize dto.</param>
        /// <param name="result">The result.</param>
        /// <returns>Task.</returns>
        public async Task AddBankCardResultedAsync(AddBankCardSagaInitDto dto, YilianRequestResult result)
        {
            if (!this.State.Verified)
            {
                return;
            }

            if (this.State.BankCards.All(c => c.BankCardNo != dto.Command.BankCardNo))
            {
                return;
            }

            bool isDefault = !this.State.BankCards.Any(c => c.IsDefault && c.Verified);
            if (result.Result)
            {
                this.State.BankCards.Where(c => c.BankCardNo == dto.Command.BankCardNo).ForEach(
                    c =>
                    {
                        c.IsDefault = isDefault;
                        c.Verified = true;
                        c.VerifiedByYilian = true;
                        c.VerifiedTime = DateTime.UtcNow.AddHours(8);
                    });
            }
            else
            {
                this.State.BankCards.RemoveAll(c => !c.Verified && c.BankCardNo == dto.Command.BankCardNo);
            }

            await this.State.WriteStateAsync();
            this.ReloadBankCardsData();

            await this.RaiseAddBankCardResultedEvent(dto, result, isDefault);
        }

        /// <summary>
        ///     Authenticatings the user information asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task AuthenticateAsync(Authenticate command)
        {
            if (this.State.Verified)
            {
                return;
            }

            if (this.BankCards.ContainsKey(command.BankCardNo))
            {
                return;
            }

            await this.BeginProcessCommandAsync(command);

            this.State.RealName = command.RealName;
            this.State.Credential = command.Credential;
            this.State.CredentialNo = command.CredentialNo;
            this.State.BankCards.Add(new BankCard
            {
                BankCardNo = command.BankCardNo,
                BankName = command.BankName,
                Cellphone = this.State.Cellphone,
                CityName = command.CityName,
                IsDefault = false,
                Verified = false,
                VerifiedByYilian = false,
                VerifiedTime = null
            });

            await this.State.WriteStateAsync();

            IAuthenticateSaga saga = AuthenticateSagaFactory.GetGrain(Guid.NewGuid());
            await saga.BeginProcessAsync(new AuthenticateSagaInitDto
            {
                Command = command,
                UserInfo = await this.GetUserInfoAsync()
            });
        }

        /// <summary>
        ///     Authenticates the resulted asynchronous.
        /// </summary>
        /// <param name="dto">The authenticate saga initialize dto.</param>
        /// <param name="result">The result.</param>
        /// <returns>Task.</returns>
        public async Task AuthenticateResultedAsync(AuthenticateSagaInitDto dto, YilianRequestResult result)
        {
            DateTime now = DateTime.UtcNow.AddHours(8);
            if (this.State.Verified)
            {
                return;
            }

            if (this.State.RealName != dto.Command.RealName || this.State.CredentialNo != dto.Command.CredentialNo
                || this.State.Credential != dto.Command.Credential)
            {
                return;
            }

            if (result.Result)
            {
                this.State.Verified = true;
                this.State.VerifiedTime = now;
                this.State.BankCards.Where(c => c.BankCardNo == dto.Command.BankCardNo).ForEach(
                    c =>
                    {
                        c.IsDefault = true;
                        c.Verified = true;
                        c.VerifiedByYilian = true;
                        c.VerifiedTime = now;
                    });
            }
            else
            {
                this.State.RealName = string.Empty;
                this.State.Credential = Credential.None;
                this.State.CredentialNo = string.Empty;
                this.State.Verified = false;
                this.State.VerifiedTime = null;
                this.State.BankCards.RemoveAll(c => !c.Verified && c.BankCardNo == dto.Command.BankCardNo);
            }

            await this.State.WriteStateAsync();
            this.ReloadBankCardsData();
            await this.RaiseApplyForAuthenticationResultedEvent(dto.Command, result);
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

            this.State.BankCards.RemoveAll(c => !c.Verified);

            await this.State.WriteStateAsync();
            this.ReloadBankCardsData();
        }

        /// <summary>
        ///     Deposits from the settle account asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task DepositAsync(DepositFromYilian command)
        {
            if (!this.State.Verified)
            {
                return;
            }

            BankCard value;
            if (!this.BankCards.TryGetValue(command.BankCardNo, out value) || !value.VerifiedByYilian)
            {
                return;
            }

            if (this.State.SettleAccount.Any(t => t.TransactionId == command.CommandId))
            {
                return;
            }

            await this.BeginProcessCommandAsync(command);

            this.State.SettleAccount.Add(new Transcation
            {
                AgreementsInfo = new Dictionary<string, object>(),
                Amount = command.Amount,
                BankCardNo = command.BankCardNo,
                ChannelCode = ChannelCodeHelper.Yilian,
                ResultCode = 0,
                ResultTime = null,
                Trade = Trade.Debit,
                TradeCode = TradeCodeHelper.TC1005051001,
                TransactionId = command.CommandId,
                TransactionTime = DateTime.UtcNow.AddHours(8),
                TransDesc = "充值申请"
            });

            await this.State.WriteStateAsync();
            this.ReloadSettleAccountData();

            IDepositByYilianSaga saga = DepositByYilianSagaFactory.GetGrain(Guid.NewGuid());
            await saga.BeginProcessAsync(new DepositFromYilianSagaInitDto
            {
                BackCardInfo = await this.GetBankCardInfoAsync(command.BankCardNo),
                Command = command,
                TranscationInfo = await this.GetSettleAccountTranscationInfoAsync(command.CommandId),
                UserInfo = await this.GetUserInfoAsync()
            });
        }

        /// <summary>
        ///     Deposits the resulted asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="result">The result.</param>
        /// <returns>Task.</returns>
        public async Task DepositResultedAsync(DepositFromYilianSagaInitDto dto, YilianRequestResult result)
        {
            if (this.State.SettleAccount.Any(t => t.TransactionId == dto.TranscationInfo.TransactionId && t.ResultTime.HasValue))
            {
                return;
            }

            this.State.SettleAccount.Where(t => t.TransactionId == dto.TranscationInfo.TransactionId).ForEach(
                t =>
                {
                    t.ResultCode = result.Result ? 1 : -1;
                    t.ResultTime = DateTime.UtcNow.AddHours(8);
                    t.TransDesc = result.Result ? "充值成功" : result.Message;
                });

            await this.State.WriteStateAsync();
            this.ReloadSettleAccountData();
            await this.RaiseDepositResultedEvent(dto, result);
        }

        /// <summary>
        ///     Gets the bank card information asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        public Task<BankCardInfo> GetBankCardInfoAsync(string bankCardNo)
        {
            BankCard card;
            if (this.BankCards.TryGetValue(bankCardNo, out card))
            {
                int withdrawAmount = this.SettleAccountBalance - this.BankCards.Values.Sum(c => c.WithdrawAmount);
                withdrawAmount = withdrawAmount > 0 ? withdrawAmount + card.WithdrawAmount : card.WithdrawAmount;

                return Task.FromResult(new BankCardInfo
                {
                    BankCardNo = card.BankCardNo,
                    BankName = card.BankName,
                    CanBeUsedForYilian = card.VerifiedByYilian,
                    CityName = card.CityName,
                    IsDefault = card.IsDefault,
                    Verified = card.Verified,
                    VerifiedTime = card.VerifiedTime.GetValueOrDefault(DateTime.MaxValue),
                    WithdrawAmount = withdrawAmount
                });
            }
            return Task.FromResult<BankCardInfo>(null);
        }

        /// <summary>
        ///     Gets the bank card infos asynchronous.
        /// </summary>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        public Task<List<BankCardInfo>> GetBankCardInfosAsync()
        {
            return Task.FromResult(this.BankCards.Values.Select(c => new BankCardInfo
            {
                BankCardNo = c.BankCardNo,
                BankName = c.BankName,
                CanBeUsedForYilian = c.VerifiedByYilian,
                CityName = c.CityName,
                IsDefault = c.IsDefault,
                Verified = c.Verified,
                VerifiedTime = c.VerifiedTime.GetValueOrDefault(DateTime.MaxValue),
                WithdrawAmount = c.WithdrawAmount
            }).ToList());
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
            IList<OrderInfo> orders = this.State.Orders.Where(o => categories.Contains(o.ProductCategory)).ToList();
            int totalCount = orders.Count;
            if (ordersSortMode == OrdersSortMode.ByOrderTimeDesc)
            {
                orders = orders.OrderByDescending(o => o.OrderTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                orders = orders.OrderBy(o => o.SettleDate).ThenByDescending(o => o.OrderTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            return Task.FromResult(new PaginatedList<OrderInfo>(pageIndex, pageSize, totalCount, orders));
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
        ///     Gets the transcation information asynchronous.
        /// </summary>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task&lt;TranscationInfo&gt;.</returns>
        public Task<TranscationInfo> GetSettleAccountTranscationInfoAsync(Guid transcationId)
        {
            Transcation transcation = this.State.SettleAccount.FirstOrDefault(t => t.TransactionId == transcationId);
            if (transcation == null)
            {
                return Task.FromResult<TranscationInfo>(null);
            }

            return Task.FromResult(transcation.ToInfo());
        }

        /// <summary>
        ///     Gets the settle account transcation infos asynchronous.
        /// </summary>
        /// <returns>Task&lt;PaginatedList&lt;TranscationInfo&gt;&gt;.</returns>
        public Task<PaginatedList<TranscationInfo>> GetSettleAccountTranscationInfosAsync(int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;

            int totalCount = this.State.SettleAccount.Count;
            IList<TranscationInfo> items = this.State.SettleAccount.OrderByDescending(t => t.TransactionTime).Skip(pageIndex * pageSize).Take(pageSize)
                .Select(t => t.ToInfo()).ToList();

            return Task.FromResult(new PaginatedList<TranscationInfo>(pageIndex, pageSize, totalCount, items));
        }

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetUserInfoAsync()
        {
            BankCard card = this.BankCards.Values.FirstOrDefault(c => c.IsDefault);

            return Task.FromResult(new UserInfo
            {
                Balance = this.SettleAccountBalance,
                BankCardNo = card == null ? string.Empty : card.BankCardNo,
                BankCardsCount = this.BankCards.Count,
                BankName = card == null ? string.Empty : card.BankName,
                Cellphone = this.State.Cellphone,
                ContractId = this.State.ContractId,
                Credential = this.State.Credential,
                CredentialNo = this.State.CredentialNo,
                Crediting = this.CreditingSettleAccountAmount,
                Debiting = this.DebitingSettleAccountAmount,
                HasSetPaymentPassword = this.State.EncryptedPaymentPassword.IsNotNullOrEmpty(),
                HasSetPassword = this.State.EncryptedPassword.IsNotNullOrEmpty(),
                InvestingInterest = this.InvestingInterest,
                InvestingPrincipal = this.InvestingPrincipal,
                InviteBy = this.State.InviteBy,
                LoginNames = this.State.LoginNames,
                MonthWithdrawalCount = this.MonthWithdrawalCount,
                PasswordErrorCount = this.PasswordErrorCount,
                RealName = this.State.RealName,
                RegisterTime = this.State.RegisterTime,
                TodayWithdrawalCount = this.TodayWithdrawalCount,
                TotalInterest = this.TotalInterest,
                TotalPrincipal = this.TotalPrincipal,
                UserId = this.State.Id,
                Verified = this.State.Verified,
                VerifiedTime = this.State.VerifiedTime
            });
        }

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task<OrderInfo> InvestingAsync(RegularInvesting command)
        {
            if (this.SettleAccount.ContainsKey(command.CommandId))
            {
                return null;
            }

            if (this.SettleAccountBalance < command.Amount)
            {
                return null;
            }

            await this.BeginProcessCommandAsync(command);

            int tradeCode = ProductCategoryCodeHelper.IsJinyinmaoProduct(command.ProductCategory) ? TradeCodeHelper.TC1005012004 : TradeCodeHelper.TC1005022004;
            DateTime now = DateTime.UtcNow.AddHours(8);
            Transcation transcation = new Transcation
            {
                AgreementsInfo = new Dictionary<string, object>(),
                Amount = command.Amount,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                ResultCode = 1,
                ResultTime = now,
                Trade = Trade.Credit,
                TradeCode = tradeCode,
                TransDesc = "支付成功",
                TransactionId = command.CommandId,
                TransactionTime = now
            };

            TranscationInfo transcationInfo = transcation.ToInfo();

            IRegularProduct product = RegularProductFactory.GetGrain(command.ProductId);
            OrderInfo order = await product.BuildOrderAsync(await this.GetUserInfoAsync(), transcationInfo);
            if (order == null)
            {
                return null;
            }

            this.State.Orders.Add(order);

            this.State.SettleAccount.Add(transcation);

            await this.State.WriteStateAsync();
            this.ReloadSettleAccountData();
            this.ReloadOrderInfosData();

            await this.RaiseOrderPaidEvent(order, transcationInfo);

            return order;
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
        public async Task RegisterAsync(UserRegister command)
        {
            if (this.State.Id == command.UserId)
            {
                return;
            }

            if (this.State.Id != Guid.Empty)
            {
                this.GetLogger().Warn(1, "Conflict user id: UserId {0}, UserRegisterCommand.UserId {1}", this.State.Id, command.UserId);
                return;
            }

            await this.BeginProcessCommandAsync(command);

            DateTime registerTime = DateTime.UtcNow.AddHours(8);
            this.State.Id = command.UserId;
            this.State.Args = command.Args;
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
            this.State.JBYAccount = new List<Transcation>();
            this.State.SettleAccount = new List<Transcation>();

            await this.State.WriteStateAsync();

            await this.RaiseUserRegisteredEvent(command);
        }

        /// <summary>
        ///     Repays the order asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="repaidTime">The repaid time.</param>
        /// <returns>Task.</returns>
        public Task RepayOrderAsync(Guid orderId, DateTime repaidTime)
        {
            DateTime now = DateTime.UtcNow.AddHours(8);

            this.State.Orders.Where(o => o.OrderId == orderId).ForEach(o =>
            {
                o.IsRepaid = true;
                o.RepaidTime = repaidTime;

                int principalTradeCode;
                int interestTradeCode;
                if (ProductCategoryCodeHelper.IsJinyinmaoRegularProduct(o.ProductCategory))
                {
                    principalTradeCode = TradeCodeHelper.TC1005011104;
                    interestTradeCode = TradeCodeHelper.TC1005011105;
                }
                else
                {
                    principalTradeCode = TradeCodeHelper.TC1005021104;
                    interestTradeCode = TradeCodeHelper.TC1005021105;
                }

                Transcation principalTranscation = new Transcation
                {
                    AgreementsInfo = new Dictionary<string, object>(),
                    Amount = o.Principal,
                    BankCardNo = string.Empty,
                    Cellphone = this.State.Cellphone,
                    ChannelCode = ChannelCodeHelper.Jinyinmao,
                    ResultCode = 1,
                    ResultTime = now,
                    Trade = Trade.Debit,
                    TradeCode = principalTradeCode,
                    TransactionId = Guid.NewGuid(),
                    TransDesc = "本金还款",
                    TransactionTime = now,
                    UserId = this.State.Id
                };

                Transcation interestTranscation = new Transcation
                {
                    AgreementsInfo = new Dictionary<string, object>(),
                    Amount = o.Interest + o.ExtraInterest,
                    BankCardNo = string.Empty,
                    Cellphone = this.State.Cellphone,
                    ChannelCode = ChannelCodeHelper.Jinyinmao,
                    ResultCode = 1,
                    ResultTime = now,
                    Trade = Trade.Debit,
                    TradeCode = interestTradeCode,
                    TransactionId = Guid.NewGuid(),
                    TransDesc = "产品结息",
                    TransactionTime = now,
                    UserId = this.State.Id
                };

                this.State.SettleAccount.Add(principalTranscation);
                this.State.SettleAccount.Add(interestTranscation);

                this.RaiseOrderRepaidEvent(o, principalTranscation.ToInfo(), interestTranscation.ToInfo());
            });

            return TaskDone.Done;
        }

        /// <summary>
        ///     Resets the login password.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task ResetLoginPasswordAsync(ResetLoginPassword command)
        {
            await this.BeginProcessCommandAsync(command);

            this.State.Salt = command.Salt;
            this.State.EncryptedPassword = CryptographyHelper.Encrypting(command.Password, command.Salt);

            await this.State.WriteStateAsync();

            await this.RaiseLoginPasswordResetEvent(command);
        }

        /// <summary>
        ///     Sets the default bank card asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task.</returns>
        public Task SetDefaultBankCardAsync(string bankCardNo)
        {
            BankCard card;
            if (this.BankCards.TryGetValue(bankCardNo, out card) && !card.IsDefault
                && card.Verified)
            {
                this.State.BankCards.ForEach(c => c.IsDefault = false);
                card.IsDefault = true;
            }

            return this.State.WriteStateAsync();
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

            await this.BeginProcessCommandAsync(command);

            this.State.PaymentSalt = command.Salt;
            this.State.EncryptedPaymentPassword = CryptographyHelper.Encrypting(command.PaymentPassword, command.Salt);

            await this.State.WriteStateAsync();

            await this.RaisePaymentPasswordSetEvent(command);
        }

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task WithdrawalAsync(Withdrawal command)
        {
            if (!this.State.Verified)
            {
                return;
            }

            if (!this.BankCards.ContainsKey(command.BankCardNo))
            {
                return;
            }

            if (this.State.SettleAccount.Any(t => t.TransactionId == command.CommandId))
            {
                return;
            }

            if (this.SettleAccountBalance < command.Amount)
            {
                return;
            }

            BankCardInfo info = await this.GetBankCardInfoAsync(command.BankCardNo);
            if (info.WithdrawAmount < command.Amount)
            {
                return;
            }

            if (this.TodayWithdrawalCount >= DailyWithdrawalLimitCount)
            {
                return;
            }

            await this.BeginProcessCommandAsync(command);

            Transcation transcation = new Transcation
            {
                AgreementsInfo = new Dictionary<string, object>(),
                Amount = command.Amount,
                BankCardNo = command.BankCardNo,
                ChannelCode = ChannelCodeHelper.Yilian,
                ResultCode = 0,
                ResultTime = null,
                Trade = Trade.Credit,
                TradeCode = TradeCodeHelper.TC1005052001,
                TransactionId = command.CommandId,
                TransactionTime = DateTime.UtcNow.AddHours(8),
                TransDesc = "取现申请"
            };

            Transcation chargeTranscation = this.BuildChargeTranscation(transcation);

            this.State.SettleAccount.Add(transcation);
            this.State.SettleAccount.Add(chargeTranscation);

            await this.State.WriteStateAsync();
            this.ReloadSettleAccountData();

            using (JYMDBContext db = new JYMDBContext())
            {
                AccountTranscation accountTranscation = new AccountTranscation
                {
                    AgreementsInfo = transcation.AgreementsInfo.ToJson(),
                    Amount = transcation.Amount,
                    Args = command.Args,
                    BankCardInfo = info.ToJson(),
                    Cellphone = this.State.Cellphone,
                    ChannelCode = transcation.ChannelCode,
                    Info = JsonHelper.NewDictionary,
                    ResultCode = 0,
                    ResultTime = null,
                    TradeCode = transcation.TradeCode,
                    TransDesc = transcation.TransDesc,
                    TranscationIdentifier = transcation.TransactionId.ToGuidString(),
                    TranscationTime = transcation.TransactionTime,
                    UserIdentifier = this.State.Id.ToGuidString(),
                    UserInfo = (await this.GetUserInfoAsync()).ToJson()
                };

                AccountTranscation accountChargeTranscation = new AccountTranscation
                {
                    AgreementsInfo = chargeTranscation.AgreementsInfo.ToJson(),
                    Amount = chargeTranscation.Amount,
                    Args = command.Args,
                    BankCardInfo = JsonHelper.NewDictionary,
                    Cellphone = this.State.Cellphone,
                    ChannelCode = chargeTranscation.ChannelCode,
                    Info = JsonHelper.NewDictionary,
                    ResultCode = 1,
                    ResultTime = chargeTranscation.ResultTime,
                    TradeCode = chargeTranscation.TradeCode,
                    TransDesc = chargeTranscation.TransDesc,
                    TranscationIdentifier = chargeTranscation.TransactionId.ToGuidString(),
                    TranscationTime = chargeTranscation.TransactionTime,
                    UserIdentifier = this.State.Id.ToGuidString(),
                    UserInfo = (await this.GetUserInfoAsync()).ToJson()
                };

                db.Add(accountTranscation);
                db.Add(accountChargeTranscation);

                await db.ExecuteSaveChangesAsync();
            }
        }

        /// <summary>
        ///     Withdrawals the resulted asynchronous.
        /// </summary>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task.</returns>
        public async Task WithdrawalResultedAsync(Guid transcationId)
        {
            Transcation transcation = this.State.SettleAccount.FirstOrDefault(t => t.TransactionId == transcationId);
            if (transcation == null || transcation.ResultCode != 0 || transcation.TradeCode != TradeCodeHelper.TC1005052001)
            {
                return;
            }

            transcation.ResultCode = 1;
            transcation.ResultTime = DateTime.UtcNow.AddHours(8);
            transcation.TransDesc = "取现成功";

            await this.State.WriteStateAsync();
            this.ReloadSettleAccountData();

            await this.RaiseWithdrawalResultedEvent(transcation);
        }

        #endregion IUser Members

        /// <summary>
        ///     Builds the charge transcation.
        /// </summary>
        /// <param name="transcation">The transcation.</param>
        /// <returns>Transcation.</returns>
        private Transcation BuildChargeTranscation(Transcation transcation)
        {
            DateTime now = DateTime.UtcNow.AddHours(8);
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

            return new Transcation
            {
                AgreementsInfo = new Dictionary<string, object>(),
                Amount = chargeAmount,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                ResultCode = 1,
                ResultTime = now,
                Trade = Trade.Credit,
                TradeCode = TradeCodeHelper.TC1005012102,
                TransDesc = "账户取现手续费",
                TransactionId = Guid.NewGuid(),
                TransactionTime = now
            };
        }
    }
}