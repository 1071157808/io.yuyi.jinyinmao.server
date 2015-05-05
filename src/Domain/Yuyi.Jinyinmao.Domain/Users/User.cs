// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  3:20 AM
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
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
using Yuyi.Jinyinmao.Domain.EventProcessor;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Domain.Sagas;
using Yuyi.Jinyinmao.Helper;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class User.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public class User : EntityGrain<IUserState>, IUser
    {
        /// <summary>
        ///     Gets or sets the error count.
        /// </summary>
        /// <value>The error count.</value>
        public int PasswordErrorCount { get; set; }

        /// <summary>
        ///     用户所有的银行卡，包括未通过认证的
        /// </summary>
        private Dictionary<string, BankCard> BankCards { get; set; }

        /// <summary>
        ///     Gets or sets the investing interest.
        /// </summary>
        /// <value>The investing interest.</value>
        private int InvestingInterest { get; set; }

        /// <summary>
        ///     Gets or sets the principal.
        /// </summary>
        /// <value>The principal.</value>
        private int InvestingPrincipal { get; set; }

        /// <summary>
        ///     Gets the jby account.
        /// </summary>
        /// <value>The jby account.</value>
        private List<Transcation> JBYAccount
        {
            get { return this.State.JBYAccount.Where(t => t.ResultCode == 1).ToList(); }
        }

        /// <summary>
        ///     Gets or sets the payment password error count.
        /// </summary>
        /// <value>The payment password error count.</value>
        private int PaymentPasswordErrorCount { get; set; }

        /// <summary>
        ///     Gets the settlement account.
        /// </summary>
        /// <value>The settlement account.</value>
        private List<Transcation> SettleAccount
        {
            get { return this.State.SettleAccount.Where(t => t.ResultCode == 1).ToList(); }
        }

        /// <summary>
        ///     Gets or sets the settlement account balance.
        /// </summary>
        /// <value>The settlement account balance.</value>
        private int SettleAccountBalance { get; set; }

        /// <summary>
        ///     Gets or sets the total interest.
        /// </summary>
        /// <value>The total interest.</value>
        private int TotalInterest { get; set; }

        /// <summary>
        ///     Gets or sets the total principal.
        /// </summary>
        /// <value>The total principal.</value>
        private int TotalPrincipal { get; set; }

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

            IAddBankCardSaga saga = AddBankCardSagaFactory.GetGrain(Guid.NewGuid());
            await saga.BeginProcessAsync(new AddBankCardSagaInitDto
            {
                Command = command,
                UserInfo = await this.GetUserInfoAsync()
            });

            await this.StoreCommandAsync(command);
            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="dto">The add bank card saga initialize dto.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>Task.</returns>
        public async Task AddBankCardResultedAsync(AddBankCardSagaInitDto dto, bool result)
        {
            bool isDefault = !this.State.BankCards.Any(c => c.IsDefault && c.Verified);

            if (result)
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

            await this.RaiseAddBankCardResultedEvent(dto, result, isDefault);
            await this.State.WriteStateAsync();
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

            IAuthenticateSaga saga = AuthenticateSagaFactory.GetGrain(Guid.NewGuid());
            await saga.BeginProcessAsync(new AuthenticateSagaInitDto
            {
                Command = command,
                UserInfo = await this.GetUserInfoAsync()
            });

            await this.StoreCommandAsync(command);
            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Authenticates the resulted asynchronous.
        /// </summary>
        /// <param name="dto">The authenticate saga initialize dto.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>Task.</returns>
        public async Task AuthenticateResultedAsync(AuthenticateSagaInitDto dto, bool result)
        {
            DateTime now = DateTime.UtcNow.AddHours(8);
            if (this.State.Verified)
            {
                return;
            }

            if (result)
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

            await this.RaiseApplyForAuthenticationResultedEvent(dto.Command, result);
            await this.State.WriteStateAsync();
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

            if (CryptographyHelper.Check(paymentPassword, this.State.PaymentSalt, this.State.EncryptedPassword))
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
            this.ReloadBankCardsData();

            await this.State.WriteStateAsync();
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
            if (!this.BankCards.TryGetValue(command.BankCardNo, out value) || value.VerifiedByYilian)
            {
                return;
            }

            if (this.State.SettleAccount.Any(t => t.TransactionId == command.CommandId))
            {
                return;
            }

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
                TransDesc = ""
            });

            IDepositByYilianSaga saga = DepositByYilianSagaFactory.GetGrain(Guid.NewGuid());
            await saga.BeginProcessAsync(new DepositFromYilianSagaInitDto
            {
                BackCardInfo = await this.GetBankCardInfoAsync(command.BankCardNo),
                Command = command,
                TranscationInfo = await this.GetSettleAccountTranscationInfoAsync(command.CommandId),
                UserInfo = await this.GetUserInfoAsync()
            });

            await this.StoreCommandAsync(command);
            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Deposits the resulted asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        public async Task DepositResultedAsync(DepositFromYilianSagaInitDto dto, bool result, string message)
        {
            if (this.State.SettleAccount.Any(t => t.TransactionId == dto.TranscationInfo.TransactionId && t.ResultTime.HasValue))
            {
                return;
            }

            this.State.SettleAccount.Where(t => t.TransactionId == dto.TranscationInfo.TransactionId).ForEach(
                t =>
                {
                    t.ResultCode = result ? 1 : -1;
                    t.ResultTime = DateTime.UtcNow.AddHours(8);
                    t.TransDesc += message;
                });

            this.ReloadSettleAccountData();

            await this.RaiseDepositResultedEvent(dto, result);
            await this.State.WriteStateAsync();
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
                    IsDefault = card.IsDefault,
                    Verified = card.Verified,
                    VerifiedTime = card.VerifiedTime.GetValueOrDefault(DateTime.MaxValue),
                    WithdrawAmount = withdrawAmount
                });
            }
            return null;
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
                VerifiedTime = c.VerifiedTime.GetValueOrDefault(DateTime.MaxValue),
                WithdrawAmount = c.WithdrawAmount
            }).ToList());
        }

        /// <summary>
        ///     Gets the settle account information asynchronous.
        /// </summary>
        /// <returns>Task&lt;SettleAccountInfo&gt;.</returns>
        public Task<SettleAccountInfo> GetSettleAccountInfoAsync()
        {
            return Task.FromResult(new SettleAccountInfo
            {
                Balance = this.SettleAccountBalance
            });
        }

        /// <summary>
        ///     Gets the transcation information asynchronous.
        /// </summary>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task&lt;TranscationInfo&gt;.</returns>
        public Task<TranscationInfo> GetSettleAccountTranscationInfoAsync(Guid transcationId)
        {
            var transcation = this.State.SettleAccount.FirstOrDefault(t => t.TransactionId == transcationId);
            if (transcation == null)
            {
                return null;
            }
            return Task.FromResult(new TranscationInfo
            {
                AgreementsInfo = transcation.AgreementsInfo,
                Amount = transcation.Amount,
                BankCardNo = transcation.BankCardNo,
                ChannelCode = transcation.ChannelCode,
                ResultCode = transcation.ResultCode,
                ResultTime = transcation.ResultTime,
                Trade = TradeCodeHelper.IsDebit(transcation.TradeCode) ? Trade.Debit : Trade.Credit,
                TradeCode = transcation.TradeCode,
                TransactionId = transcation.TransactionId,
                TransactionTime = transcation.TransactionTime,
                TransDesc = transcation.TransDesc
            });
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
                BankCardNo = card == null ? string.Empty : card.BankCardNo,
                BankCardsCount = this.BankCards.Count,
                BankName = card == null ? string.Empty : card.BankName,
                Cellphone = this.State.Cellphone,
                ContractId = this.State.ContractId,
                Credential = this.State.Credential,
                CredentialNo = this.State.CredentialNo,
                HaSetPaymentPassword = this.State.EncryptedPaymentPassword.IsNotNullOrEmpty(),
                HasSetPassword = this.State.EncryptedPassword.IsNotNullOrEmpty(),
                InviteBy = this.State.InviteBy,
                LoginNames = this.State.LoginNames,
                RealName = this.State.RealName,
                RegisterTime = this.State.RegisterTime,
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
        public async Task InvestingAsync(RegularInvesting command)
        {
            if (this.State.SettleAccount.Any(t => t.TransactionId == command.CommandId))
            {
                return;
            }

            if (this.SettleAccountBalance < command.Amount)
            {
                return;
            }

            int tradeCode = command.ProductCategory > 199999999 ? TradeCodeHelper.TC1005022004 : TradeCodeHelper.TC1005012004;

            DateTime now = DateTime.UtcNow.AddHours(8);
            this.State.SettleAccount.Add(new Transcation
            {
                AgreementsInfo = new Dictionary<string, object>(),
                Amount = command.Amount,
                BankCardNo = string.Empty,
                ChannelCode = 0,
                ResultCode = 1,
                ResultTime = now,
                Trade = Trade.Credit,
                TradeCode = tradeCode,
                TransDesc = string.Empty,
                TransactionId = command.CommandId,
                TransactionTime = now
            });

            IRegularProduct product = RegularProductFactory.GetGrain(command.ProductId);
            OrderInfo order = await product.BuildOrderAsync(await this.GetUserInfoAsync(), await this.GetSettleAccountTranscationInfoAsync(command.CommandId));
            if (order == null)
            {
                this.State.SettleAccount.RemoveAll(t => t.TransactionId == command.CommandId);
            }
            else
            {
                if (this.State.Orders.All(o => o.OrderId != order.OrderId))
                {
                    this.State.Orders.Add(order);
                }
            }

            await this.RaiseOrderBuiltEvent(order);

            await this.State.WriteStateAsync();

            this.ReloadSettleAccountData();
            this.ReloadOrderInfosData();
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
        /// <param name="command"></param>
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

            await this.StoreCommandAsync(command);
            await this.RaiseUserRegisteredEvent(command);

            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Repays the order asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="repaidTime">The repaid time.</param>
        /// <returns>Task.</returns>
        public Task RepayOrderAsync(Guid orderId, DateTime repaidTime)
        {
            this.State.Orders.Where(o => o.OrderId == orderId).ForEach(o =>
            {
                o.IsRepaid = true;
                o.RepaidTime = repaidTime;

                this.RaiseOrderRepaidEvent(o);
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
            this.State.Salt = command.Salt;
            this.State.EncryptedPassword = CryptographyHelper.Encrypting(command.Password, command.Salt);
            await this.StoreCommandAsync(command);
            await this.RaiseLoginPasswordResetEvent(command);

            await this.State.WriteStateAsync();
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
                this.State.BankCards.ForEach(c => c.IsDefault = true);
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

            this.State.PaymentSalt = command.Salt;
            this.State.EncryptedPaymentPassword = CryptographyHelper.Encrypting(command.PaymentPassword, command.Salt);

            await this.StoreCommandAsync(command);
            await this.RaisePaymentPasswordSetEvent(command);

            await this.State.WriteStateAsync();
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

            BankCardInfo info = await this.GetBankCardInfoAsync(command.BankCardNo);
            if (info.WithdrawAmount < command.Amount)
            {
                return;
            }

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
                TransDesc = ""
            };

            this.State.SettleAccount.Add(transcation);

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
                    Info = new object().ToJson(),
                    ResultCode = 0,
                    ResultTime = null,
                    TradeCode = TradeCodeHelper.TC1005052001,
                    TransDesc = transcation.TransDesc,
                    TranscationIdentifier = transcation.TransactionId.ToGuidString(),
                    TranscationTime = transcation.TransactionTime,
                    UserIdentifier = this.State.Id.ToGuidString(),
                    UserInfo = (await this.GetUserInfoAsync()).ToJson()
                };

                await db.SaveAsync(accountTranscation);
            }

            await this.StoreCommandAsync(command);
            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Withdrawals the resulted asynchronous.
        /// </summary>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task WithdrawalResultedAsync(Guid transcationId)
        {
            Transcation transcation = this.State.SettleAccount.FirstOrDefault(t => t.TransactionId == transcationId);
            if (transcation == null || transcation.ResultCode != 0 || transcation.TradeCode != TradeCodeHelper.TC1005052001)
            {
                return;
            }

            transcation.ResultCode = 1;
            transcation.ResultTime = DateTime.UtcNow.AddHours(8);
            transcation.TransDesc += "取现完成。";

            this.ReloadSettleAccountData();

            await this.RaiseWithdrawalResultedEvent(transcation);
            await this.State.WriteStateAsync();
        }

        #endregion IUser Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.ReloadBankCardsData();
            this.ReloadSettleAccountData();
            this.ReloadOrderInfosData();
            return base.OnActivateAsync();
        }

        /// <summary>
        ///     Raises the add bank card resulted event.
        /// </summary>
        /// <param name="dto">The AddBankCardSagaInitDto.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="isDefault">if set to <c>true</c> [is default].</param>
        /// <returns>Task.</returns>
        private async Task RaiseAddBankCardResultedEvent(AddBankCardSagaInitDto dto, bool result, bool isDefault)
        {
            AddBankCardResulted @event = new AddBankCardResulted
            {
                Args = dto.Command.Args,
                BankCardNo = dto.Command.BankCardNo,
                BankName = dto.Command.BankName,
                Cellphone = this.State.Cellphone,
                CityName = dto.Command.CityName,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                CanBeUsedByYilian = true,
                IsDefault = isDefault,
                Result = result,
                UserId = this.State.Id,
                Verified = result,
                VerifiedTime = DateTime.UtcNow.AddHours(8)
            };
            await this.StoreEventAsync(@event);

            await AddBankCardResultedProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        private async Task RaiseApplyForAuthenticationResultedEvent(Authenticate command, bool result)
        {
            DateTime now = DateTime.UtcNow.AddHours(8);

            AuthenticateResulted @event = new AuthenticateResulted
            {
                Args = command.Args,
                Cellphone = command.Cellphone,
                Credential = command.Credential,
                CredentialNo = command.CredentialNo,
                RealName = command.RealName,
                Result = result,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                UserId = this.State.Id,
                Verified = result,
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
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>Task.</returns>
        private async Task RaiseDepositResultedEvent(DepositFromYilianSagaInitDto dto, bool result)
        {
            DepositFromYilianResulted @event = new DepositFromYilianResulted
            {
                Amount = dto.Command.Amount,
                Args = dto.Command.Args,
                BankCardNo = dto.BackCardInfo.BankCardNo,
                BankName = dto.BackCardInfo.BankName,
                Cellphone = dto.UserInfo.Cellphone,
                ChannalCode = dto.TranscationInfo.ChannelCode,
                CityName = dto.BackCardInfo.CityName,
                Credential = dto.UserInfo.Credential,
                CredentialNo = dto.UserInfo.CredentialNo,
                RealName = dto.UserInfo.RealName,
                Result = result,
                ResultCode = dto.TranscationInfo.ResultCode,
                ResultTime = dto.TranscationInfo.ResultTime.GetValueOrDefault(),
                SettleAccountBalance = this.SettleAccountBalance,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                Trade = dto.TranscationInfo.Trade,
                TranscationId = dto.TranscationInfo.TransactionId,
                TranscationTime = dto.TranscationInfo.TransactionTime,
                TransDesc = dto.TranscationInfo.TransDesc,
                UserId = this.State.Id
            };

            await this.StoreEventAsync(@event);

            await DepositFromYilianResultedProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        /// <summary>
        ///     Raises the login password reset event.
        /// </summary>
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

        private async Task RaiseOrderBuiltEvent(OrderInfo order)
        {
            OrderBuilt @event = new OrderBuilt
            {
                AccountTranscationId = order.AccountTranscationId,
                Args = new object().ToJson(),
                Cellphone = order.Cellphone,
                ExtraInterest = order.ExtraInterest,
                ExtraYield = order.ExtraYield,
                Info = order.Info,
                Interest = order.Interest,
                IsRepaid = order.IsRepaid,
                OrderId = order.OrderId,
                OrderNo = order.OrderNo,
                OrderTime = order.OrderTime,
                Principal = order.Principal,
                ProductId = order.ProductId,
                ProductSnapshot = order.ProductSnapshot,
                RepaidTime = order.RepaidTime,
                ResultCode = order.ResultCode,
                ResultTime = order.ResultTime,
                SettleDate = order.SettleDate,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                TransDesc = order.TransDesc,
                UserId = order.UserId,
                UserInfo = order.UserInfo,
                ValueDate = order.ValueDate,
                Yield = order.Yield
            };
            await this.StoreEventAsync(@event);

            await OrderBuiltProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        private async void RaiseOrderRepaidEvent(OrderInfo orderInfo)
        {
            OrderRepaid @event = new OrderRepaid
            {
                AccountTranscationId = orderInfo.AccountTranscationId,
                Args = new object().ToJson(),
                Cellphone = orderInfo.Cellphone,
                ExtraInterest = orderInfo.ExtraInterest,
                ExtraYield = orderInfo.ExtraYield,
                Info = orderInfo.Info,
                Interest = orderInfo.Interest,
                IsRepaid = orderInfo.IsRepaid,
                OrderId = orderInfo.OrderId,
                OrderNo = orderInfo.OrderNo,
                OrderTime = orderInfo.OrderTime,
                Principal = orderInfo.Principal,
                ProductId = orderInfo.ProductId,
                ProductSnapshot = orderInfo.ProductSnapshot,
                RepaidTime = orderInfo.RepaidTime,
                ResultCode = orderInfo.ResultCode,
                ResultTime = orderInfo.ResultTime,
                SettleDate = orderInfo.SettleDate,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                TransDesc = string.Empty,
                UserId = orderInfo.UserId,
                UserInfo = orderInfo.UserInfo,
                ValueDate = orderInfo.ValueDate,
                Yield = orderInfo.Yield
            };

            await this.StoreEventAsync(@event);
        }

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

        private async Task RaiseWithdrawalResultedEvent(Transcation transcation)
        {
            BankCardInfo info = await this.GetBankCardInfoAsync(transcation.BankCardNo);

            WithdrawalResulted @event = new WithdrawalResulted
            {
                Amount = transcation.Amount,
                Args = new object().ToJson(),
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
        }

        private void ReloadBankCardsData()
        {
            this.BankCards = this.State.BankCards.ToDictionary(c => c.BankCardNo);
        }

        private void ReloadOrderInfosData()
        {
            List<OrderInfo> paidOrders = this.State.Orders.Where(o => o.ResultCode == 1).ToList();
            List<OrderInfo> investingOrders = paidOrders.Where(o => !o.IsRepaid).ToList();

            this.TotalPrincipal = paidOrders.Sum(o => o.Principal);
            this.TotalInterest = paidOrders.Sum(o => o.Interest + o.ExtraInterest);
            this.InvestingPrincipal = investingOrders.Sum(o => o.Principal);
            this.InvestingInterest = investingOrders.Sum(o => o.Interest + o.ExtraInterest);
        }

        private void ReloadSettleAccountData()
        {
            List<Transcation> debitTrans = this.SettleAccount.Where(t => t.Trade == Trade.Debit && t.TradeCode == 1).ToList();
            List<Transcation> creditTrans = this.SettleAccount.Where(t => t.Trade == Trade.Credit && t.TradeCode >= 0).ToList();

            this.SettleAccountBalance = debitTrans.Sum(t => t.Amount) - creditTrans.Sum(t => t.Amount);

            this.BankCards.Values.ForEach(c =>
            {
                c.WithdrawAmount = debitTrans.Where(t => t.BankCardNo == c.BankCardNo).Sum(t => t.Amount)
                                   - creditTrans.Where(t => t.BankCardNo == c.BankCardNo).Sum(t => t.Amount);
            });
        }
    }
}