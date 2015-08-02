// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : User.cs
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-02  8:26 AM
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Helper;
using Yuyi.Jinyinmao.Domain.Products;
using Yuyi.Jinyinmao.Helper;
using Yuyi.Jinyinmao.Packages.Helper;
using Yuyi.Jinyinmao.Service;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class User.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public partial class User : EntityGrain<IUserState>, IUser, IRemindable
    {
        #region IUser Members

        /// <summary>
        ///     Adds bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        public async Task<BankCardInfo> AddBankCardAsync(AddBankCard command)
        {
            BankCard card;
            if (this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                if (card.Dispaly)
                {
                    return card.ToInfo();
                }

                if (!card.Verified && !card.VerifiedByYilian)
                {
                    card.Args = command.Args;
                    card.BankCardNo = command.BankCardNo;
                    card.BankName = command.BankName;
                    card.Cellphone = this.State.Cellphone;
                    card.CityName = command.CityName;
                    card.UserId = this.State.UserId;
                    card.Verified = false;
                    card.VerifiedByYilian = false;
                    card.VerifiedTime = null;
                }

                card.Dispaly = true;
            }
            else
            {
                if (this.State.BankCards.Count(c => c.Value.Dispaly) >= 10)
                {
                    return null;
                }

                this.BeginProcessCommandAsync(command);

                card = new BankCard
                {
                    AddingTime = DateTime.Now.AddHours(8),
                    Args = command.Args,
                    BankCardNo = command.BankCardNo,
                    BankName = command.BankName,
                    Cellphone = this.State.Cellphone,
                    CityName = command.CityName,
                    Dispaly = true,
                    UserId = this.State.UserId,
                    Verified = false,
                    VerifiedByYilian = false,
                    VerifiedTime = null,
                    WithdrawAmount = 0
                };

                this.State.BankCards.Add(card.BankCardNo, card);
            }

            await this.SaveStateAsync();

            await this.RaiseBankCardAddedEvent(command, card.ToInfo());

            return card.ToInfo();
        }

        /// <summary>
        ///     Adds the extra interest to order.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;OrderInfo&gt;.</returns>
        public async Task<OrderInfo> AddExtraInterestToOrderAsync(AddExtraInterest command)
        {
            Order order;
            if (!this.State.Orders.TryGetValue(command.OrderId, out order))
            {
                return null;
            }

            if (order.ExtraInterestRecords.Any(r => r.OperationId == command.OperationId))
            {
                return order.ToInfo();
            }

            this.BeginProcessCommandAsync(command);

            int amount = command.ExtraInterest + BuildInterest(order.ValueDate, order.SettleDate, command.ExtraPrincipal, order.Yield);

            order.ExtraInterestRecords.Add(new ExtraInterestRecord
            {
                Amount = amount,
                Description = command.Description,
                OperationId = command.OperationId
            });

            await this.SaveStateAsync();
            this.ReloadOrderInfosData();

            OrderInfo info = order.ToInfo();

            await this.RaiseExtraInterestAddedEvent(command, info, amount);

            return info;
        }

        /// <summary>
        ///     authenticate as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> AuthenticateAsync(Authenticate command)
        {
            if (!this.State.Verified)
            {
                this.BeginProcessCommandAsync(command);

                await this.SaveStateAsync();
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

                await this.SaveStateAsync();
                await this.RaiseAuthenticationResultedEvent(command, card.ToInfo(), result, message);
            }

            return await this.GetUserInfoAsync();
        }

        /// <summary>
        ///     Changes the cellphone asynchronous.
        /// </summary>
        /// <param name="cellphoneNo">The cellphone no.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> ChangeCellphoneAsync(string cellphoneNo)
        {
            ICellphone cellphone = CellphoneFactory.GetGrain(GrainTypeHelper.GetCellphoneGrainTypeLongKey(cellphoneNo));
            CellphoneInfo cellphoneInfo = await cellphone.GetCellphoneInfoAsync();
            if (cellphoneInfo.Registered)
            {
                return null;
            }

            ICellphone originalCellphone = CellphoneFactory.GetGrain(GrainTypeHelper.GetCellphoneGrainTypeLongKey(this.State.Cellphone));

            this.State.LoginNames.Remove(this.State.Cellphone);
            this.State.Cellphone = cellphoneNo;
            this.State.LoginNames.Add(cellphoneNo);

            this.State.BankCards.ForEach(c => c.Value.Cellphone = cellphoneNo);

            await this.State.WriteStateAsync();

            await cellphone.Register(this.State.UserId);

            await originalCellphone.Unregister();

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
            if (this.State.Cellphone.IsNullOrEmpty() || this.PasswordErrorCount > 10)
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
                        RemainCount = 10 - this.PasswordErrorCount,
                        Success = true,
                        UserExist = true,
                        UserId = this.State.UserId
                    });
                }
            }

            this.PasswordErrorCount += 1;

            return Task.FromResult(new CheckPasswordResult
            {
                Cellphone = this.State.Cellphone,
                RemainCount = 10 - this.PasswordErrorCount,
                Success = false,
                UserExist = true,
                UserId = Guid.Empty
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
            if (this.State.EncryptedPaymentPassword.IsNullOrEmpty() || this.PaymentPasswordErrorCount >= 5)
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

            this.PaymentPasswordErrorCount += 1;

            return Task.FromResult(new CheckPaymentPasswordResult
            {
                Success = false,
                RemainCount = 5 - this.PaymentPasswordErrorCount
            });
        }

        /// <summary>
        ///     Clears the unauthenticated information asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task ClearUnauthenticatedInfoAsync()
        {
            if (!this.State.Verified)
            {
                this.State.RealName = string.Empty;
                this.State.Credential = Credential.None;
                this.State.CredentialNo = string.Empty;
                this.State.VerifiedTime = null;
            }

            await this.SaveStateAsync();
        }

        /// <summary>
        ///     deposit as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;Tuple&lt;UserInfo, SettleAccountTransactionInfo, BankCardInfo&gt;&gt;.</returns>
        /// <exception cref="System.ApplicationException">
        ///     Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}.FormatWith(this.State.UserId, command.CommandId)
        ///     or
        ///     Missing BankCard data. UserId-{0}, CommandId-{1}.FormatWith(this.State.UserId, command.CommandId)
        /// </exception>
        public async Task<Tuple<UserInfo, SettleAccountTransactionInfo, BankCardInfo>> DepositAsync(PayCommand command)
        {
            if (!this.State.Verified)
            {
                throw new ApplicationException("Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}".FormatWith(this.State.UserId, command.CommandId));
            }

            BankCard card;
            if (!this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                throw new ApplicationException("Missing BankCard data. UserId-{0}, CommandId-{1}".FormatWith(this.State.UserId, command.CommandId));
            }

            UserInfo userInfo = await this.GetUserInfoAsync();

            SettleAccountTransaction transaction;
            SettleAccountTransactionInfo transactionInfo = null;
            if (!this.State.SettleAccount.TryGetValue(command.CommandId, out transaction))
            {
                this.BeginProcessCommandAsync(command);

                transaction = new SettleAccountTransaction
                {
                    Amount = command.Amount,
                    Args = command.Args,
                    BankCardNo = command.BankCardNo,
                    ChannelCode = ChannelCodeHelper.Jinyinmao,
                    OrderId = Guid.Empty,
                    ResultCode = 0,
                    ResultTime = null,
                    SequenceNo = await SequenceNoHelper.GetSequenceNoAsync(),
                    Trade = Trade.Debit,
                    TradeCode = TradeCodeHelper.TC1005051001,
                    TransactionId = command.CommandId,
                    TransactionTime = DateTime.UtcNow.AddHours(8),
                    TransDesc = "充值申请",
                    UserId = this.State.UserId,
                    UserInfo = await this.GetUserInfoAsync()
                };

                this.State.SettleAccount.Add(transaction.TransactionId, transaction);

                await this.SaveStateAsync();
                this.ReloadSettleAccountData();

                transactionInfo = transaction.ToInfo();

                await this.RaisePayingByYilianEvent(command, transactionInfo);
            }

            return new Tuple<UserInfo, SettleAccountTransactionInfo, BankCardInfo>(userInfo, transactionInfo, card.ToInfo());
        }

        /// <summary>
        ///     deposit resulted as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ApplicationException">
        ///     Missing SettleAccountTransaction {0}..FormatWith(command.CommandId)
        ///     or
        ///     Missing BankCard {0}..FormatWith(transaction.BankCardNo)
        /// </exception>
        public async Task DepositResultedAsync(PayCommand command, bool result, string message)
        {
            if (!this.State.Verified)
            {
                throw new ApplicationException("Invalid PayByYilian command. UserId-{0}, CommandId-{1}.".FormatWith(this.State.UserId, command.CommandId));
            }

            BankCard card;
            if (!this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                throw new ApplicationException("Missing BankCard data. UserId-{0}, CommandId-{1}".FormatWith(this.State.UserId, command.CommandId));
            }

            SettleAccountTransaction transaction;
            if (!this.State.SettleAccount.TryGetValue(command.CommandId, out transaction))
            {
                throw new ApplicationException("Missing SettleAccountTransaction. UserId-{0}, CommandId-{1}, SettleAccountTransactionId-{2} ."
                    .FormatWith(this.State.UserId, command.CommandId, command.CommandId));
            }

            if (transaction.ResultCode != 0 && transaction.ResultTime.HasValue)
            {
                return;
            }

            transaction.ResultCode = result ? 1 : -1;
            transaction.ResultTime = DateTime.UtcNow.AddHours(8);
            transaction.TransDesc = result ? "充值成功" : message;

            await this.SaveStateAsync();
            this.ReloadSettleAccountData();
            await this.RaiseDepositResultedEvent(command, transaction.ToInfo(), result, message);
        }

        /// <summary>
        ///     Gets the bank card information asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        public Task<BankCardInfo> GetBankCardInfoAsync(string bankCardNo)
        {
            BankCard card;
            return this.State.BankCards.TryGetValue(bankCardNo, out card) ? Task.FromResult(card.ToInfo()) : Task.FromResult<BankCardInfo>(null);
        }

        /// <summary>
        ///     Gets the bank card infos asynchronous.
        /// </summary>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        public Task<List<BankCardInfo>> GetBankCardInfosAsync()
        {
            return Task.FromResult(this.State.BankCards.Values.Where(c => c.Dispaly).Select(c => c.ToInfo()).ToList());
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

        /// <summary>
        ///     Gets the jby account reinvesting transaction infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;JBYAccountTransactionInfo&gt;&gt;.</returns>
        public Task<PaginatedList<JBYAccountTransactionInfo>> GetJBYAccountReinvestingTransactionInfosAsync(int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;

            int totalCount = this.State.JBYAccount.Count;
            IList<JBYAccountTransactionInfo> items = this.State.JBYAccount.Values.Where(t => t.TradeCode == TradeCodeHelper.TC2001011106).OrderByDescending(t => t.TransactionTime)
                .Skip(pageIndex * pageSize).Take(pageSize).Select(t => t.ToInfo()).ToList();

            return Task.FromResult(new PaginatedList<JBYAccountTransactionInfo>(pageIndex, pageSize, totalCount, items));
        }

        /// <summary>
        ///     Gets the jby account transaction information asynchronous.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        public Task<JBYAccountTransactionInfo> GetJBYAccountTransactionInfoAsync(Guid transactionId)
        {
            JBYAccountTransaction transaction;
            return this.State.JBYAccount.TryGetValue(transactionId, out transaction) ? Task.FromResult(transaction.ToInfo()) : Task.FromResult<JBYAccountTransactionInfo>(null);
        }

        /// <summary>
        ///     Gets the jby account transaction infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;TransactionInfo&gt;&gt;.</returns>
        public Task<PaginatedList<JBYAccountTransactionInfo>> GetJBYAccountTransactionInfosAsync(int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;

            int totalCount = this.State.JBYAccount.Count;
            IList<JBYAccountTransactionInfo> items = this.State.JBYAccount.Values.OrderByDescending(t => t.TransactionTime)
                .Skip(pageIndex * pageSize).Take(pageSize).Select(t => t.ToInfo()).ToList();

            return Task.FromResult(new PaginatedList<JBYAccountTransactionInfo>(pageIndex, pageSize, totalCount, items));
        }

        /// <summary>
        ///     Gets the order information asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>Task&lt;OrderInfo&gt;.</returns>
        public Task<OrderInfo> GetOrderInfoAsync(Guid orderId)
        {
            Order order;
            return this.State.Orders.TryGetValue(orderId, out order) ? Task.FromResult(order.ToInfo()) : Task.FromResult<OrderInfo>(null);
        }

        /// <summary>
        ///     Gets the order infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="ordersSortMode">The orders sort mode.</param>
        /// <returns>Task&lt;Tuple&lt;System.Int32, List&lt;OrderInfo&gt;&gt;&gt;.</returns>
        public Task<Tuple<int, List<OrderInfo>>> GetOrderInfosAsync(int pageIndex, int pageSize, OrdersSortMode ordersSortMode)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;

            IList<Order> orders = this.State.Orders.Values.ToList();

            int totalCount = orders.Count;
            orders = ordersSortMode == OrdersSortMode.ByOrderTimeDesc ?
                orders.OrderByDescending(o => o.OrderTime).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                : orders.OrderBy(o => o.SettleDate).ThenByDescending(o => o.OrderTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return Task.FromResult(new Tuple<int, List<OrderInfo>>(totalCount, orders.Select(o => o.ToInfo()).ToList()));
        }

        /// <summary>
        ///     Gets the order infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="ordersSortMode">The orders sort mode.</param>
        /// <param name="categories">The categories.</param>
        /// <returns>Task&lt;Tuple&lt;System.Int32, List&lt;OrderInfo&gt;&gt;&gt;.</returns>
        public Task<Tuple<int, List<OrderInfo>>> GetOrderInfosAsync(int pageIndex, int pageSize, OrdersSortMode ordersSortMode, IEnumerable<long> categories)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;

            IList<Order> orders = this.State.Orders.Values.Where(o => categories.Contains(o.ProductCategory)).ToList();

            int totalCount = orders.Count;
            orders = ordersSortMode == OrdersSortMode.ByOrderTimeDesc ?
                orders.OrderByDescending(o => o.OrderTime).Skip(pageIndex * pageSize).Take(pageSize).ToList()
                : orders.OrderBy(o => o.SettleDate).ThenByDescending(o => o.OrderTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return Task.FromResult(new Tuple<int, List<OrderInfo>>(totalCount, orders.Select(o => o.ToInfo()).ToList()));
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
        ///     Gets the transaction information asynchronous.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;TransactionInfo&gt;.</returns>
        public Task<SettleAccountTransactionInfo> GetSettleAccountTransactionInfoAsync(Guid transactionId)
        {
            SettleAccountTransaction transaction;
            return this.State.SettleAccount.TryGetValue(transactionId, out transaction) ? Task.FromResult(transaction.ToInfo()) : Task.FromResult<SettleAccountTransactionInfo>(null);
        }

        /// <summary>
        ///     Gets the settle account transaction infos asynchronous.
        /// </summary>
        /// <returns>Task&lt;PaginatedList&lt;TransactionInfo&gt;&gt;.</returns>
        public Task<PaginatedList<SettleAccountTransactionInfo>> GetSettleAccountTransactionInfosAsync(int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;

            int totalCount = this.State.SettleAccount.Count;
            IList<SettleAccountTransactionInfo> items = this.State.SettleAccount.Values.OrderByDescending(t => t.TransactionTime)
                .Skip(pageIndex * pageSize).Take(pageSize).Select(t => t.ToInfo()).ToList();

            return Task.FromResult(new PaginatedList<SettleAccountTransactionInfo>(pageIndex, pageSize, totalCount, items));
        }

        /// <summary>
        ///     Gets the order infos asynchronous.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>Task&lt;Tuple&lt;System.Int32, List&lt;OrderInfo&gt;&gt;&gt;.</returns>
        public Task<List<OrderInfo>> GetSettlingOrderInfosAsync(int count)
        {
            count = count < 1 ? 0 : count;
            DateTime now = DateTime.UtcNow.AddHours(8);

            IList<Order> orders = this.State.Orders.Values.ToList();

            orders = orders.Where(o => o.SettleDate >= now).OrderBy(o => o.SettleDate).ThenByDescending(o => o.OrderTime).Take(count).ToList();

            return Task.FromResult(orders.Select(o => o.ToInfo()).ToList());
        }

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetUserInfoAsync()
        {
            long withdrawalableAmount = this.SettleAccountBalance - this.GetWithrawalCharge();

            UserInfo userInfo = new UserInfo
            {
                Args = this.State.Args,
                Balance = this.SettleAccountBalance,
                BankCardsCount = this.State.BankCards.Count(c => c.Value.Dispaly),
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
                JBYLastInterest = this.JBYLastInterest,
                JBYTotalAmount = this.JBYTotalAmount,
                JBYTotalInterest = this.JBYTotalInterest,
                JBYTotalPricipal = this.JBYTotalPricipal,
                JBYWithdrawalableAmount = this.JBYWithdrawalableAmount,
                LoginNames = this.State.LoginNames,
                MonthWithdrawalCount = this.MonthWithdrawalCount,
                OutletCode = this.State.OutletCode,
                PasswordErrorCount = this.PasswordErrorCount,
                PaymentPasswordErrorCount = this.PaymentPasswordErrorCount,
                RealName = this.State.RealName,
                RegisterTime = this.State.RegisterTime,
                TodayJBYWithdrawalAmount = this.TodayJBYWithdrawalAmount,
                TodayWithdrawalCount = this.TodayWithdrawalCount,
                TotalInterest = this.TotalInterest,
                TotalPrincipal = this.TotalPrincipal,
                UserId = this.State.UserId,
                Verified = this.State.Verified,
                VerifiedTime = this.State.VerifiedTime,
                WithdrawalableAmount = withdrawalableAmount > 0 ? withdrawalableAmount : 0
            };

            return Task.FromResult(userInfo);
        }

        /// <summary>
        ///     Gets the withdrawalable bank card infos asynchronous.
        /// </summary>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        public Task<List<BankCardInfo>> GetWithdrawalableBankCardInfosAsync()
        {
            return Task.FromResult(this.State.BankCards.Values.Where(c => c.Dispaly && c.Verified).Select(c => c.ToInfo()).ToList());
        }

        /// <summary>
        ///     Hides the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task HideBankCardAsync(HideBankCard command)
        {
            BankCard card;
            if (this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                if (card.Dispaly)
                {
                    card.Dispaly = false;
                }

                await this.SaveStateAsync();

                await this.RaiseBankCardHidenEvent(command, card.ToInfo());
            }
        }

        /// <summary>
        ///     insert settle account transcation as an asynchronous operation.
        /// </summary>
        /// <param name="transactionDto">The transaction dto.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        public async Task<SettleAccountTransactionInfo> InsertSettleAccountTranscationAsync(InsertSettleAccountTransactionDto transactionDto)
        {
            DateTime now = DateTime.UtcNow.AddHours(8);
            SettleAccountTransaction transaction = new SettleAccountTransaction
            {
                Amount = transactionDto.Amount,
                Args = transactionDto.Args,
                BankCardNo = transactionDto.BankCardNo,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                OrderId = transactionDto.OrderId,
                ResultCode = 1,
                ResultTime = now,
                SequenceNo = await SequenceNoHelper.GetSequenceNoAsync(),
                Trade = transactionDto.Trade,
                TradeCode = TradeCodeHelper.TC1005051001,
                TransactionId = Guid.NewGuid(),
                TransactionTime = now,
                TransDesc = transactionDto.TransDesc,
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync()
            };

            this.State.SettleAccount.Add(transaction.TransactionId, transaction);

            await this.State.WriteStateAsync();
            this.ReloadSettleAccountData();

            SettleAccountTransactionInfo transactionInfo = transaction.ToInfo();

            await this.RaiseTransactionInsertdEvent(transactionDto, transactionInfo);

            return transactionInfo;
        }

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task<OrderInfo> InvestingAsync(RegularInvesting command)
        {
            Order order;
            if (this.State.Orders.TryGetValue(command.CommandId, out order))
            {
                return order.ToInfo();
            }

            if (this.SettleAccountBalance < command.Amount)
            {
                return null;
            }

            this.BeginProcessCommandAsync(command);

            int tradeCode = ProductCategoryCodeHelper.IsJinyinmaoProduct(command.ProductCategory) ? TradeCodeHelper.TC1005012004 : TradeCodeHelper.TC1005022004;
            DateTime now = DateTime.UtcNow.AddHours(8);

            SettleAccountTransaction transaction = new SettleAccountTransaction
            {
                Amount = command.Amount,
                Args = command.Args,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                OrderId = command.CommandId,
                ResultCode = 1,
                ResultTime = now,
                SequenceNo = await SequenceNoHelper.GetSequenceNoAsync(),
                Trade = Trade.Credit,
                TradeCode = tradeCode,
                TransDesc = "支付成功",
                TransactionId = Guid.NewGuid(),
                TransactionTime = now,
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync()
            };

            SettleAccountTransactionInfo transactionInfo = transaction.ToInfo();

            order = new Order
            {
                AccountTransactionId = transaction.TransactionId,
                Args = command.Args,
                Cellphone = this.State.Cellphone,
                ExtraInterest = 0,
                ExtraInterestRecords = new List<ExtraInterestRecord>(),
                ExtraYield = 0,
                Interest = 0, // to be updated
                IsRepaid = false,
                OrderId = command.CommandId,
                OrderNo = await SequenceNoHelper.GetSequenceNoAsync(),
                OrderTime = now,
                Principal = command.Amount,
                ProductCategory = command.ProductCategory,
                ProductId = command.ProductId,
                ProductSnapshot = null, // to be updated
                RepaidTime = null,
                ResultCode = 1,
                ResultTime = now,
                SettleDate = DateTime.UtcNow, // to be updated
                TransDesc = "购买成功",
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync(),
                ValueDate = DateTime.UtcNow, // to be updated
                Yield = 0 // to be updated
            };

            IRegularProduct product = RegularProductFactory.GetGrain(command.ProductId);
            OrderInfo orderInfo = await product.BuildOrderAsync(order.ToInfo());

            if (orderInfo == null)
            {
                return null;
            }

            order.Interest = orderInfo.Interest;
            order.ProductSnapshot = orderInfo.ProductSnapshot;
            order.SettleDate = orderInfo.SettleDate;
            order.ValueDate = orderInfo.ValueDate;
            order.Yield = orderInfo.Yield;

            if (command.CouponId != null)
            {
                ICouponService couponService = new CouponService();
                CouponInfo coupon = await couponService.UseCouponAsync(command.CouponId.GetValueOrDefault(), this.State.UserId);

                if (coupon != null)
                {
                    order.ExtraInterestRecords.Add(new ExtraInterestRecord
                    {
                        Amount = coupon.Amount,
                        Description = "本金券" + coupon.Id,
                        OperationId = command.CommandId
                    });

                    order.ExtraInterest += order.Interest * coupon.Amount / order.Principal;

                    orderInfo.ExtraInterest = order.ExtraInterest;
                    orderInfo.ExtraInterestRecords = order.ExtraInterestRecords;
                }
            }

            this.State.Orders.Add(order.OrderId, order);

            this.State.SettleAccount.Add(transaction.TransactionId, transaction);

            await this.SaveStateAsync();
            this.ReloadSettleAccountData();
            this.ReloadOrderInfosData();

            await this.RaiseOrderPaidEvent(orderInfo, transactionInfo);

            return orderInfo;
        }

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;TransactionInfo&gt;.</returns>
        public async Task<JBYAccountTransactionInfo> InvestingAsync(JBYInvesting command)
        {
            JBYAccountTransaction transaction;
            if (this.State.JBYAccount.TryGetValue(command.CommandId, out transaction))
            {
                return transaction.ToInfo();
            }

            if (this.SettleAccountBalance < command.Amount)
            {
                return null;
            }

            this.BeginProcessCommandAsync(command);

            DateTime now = DateTime.UtcNow.AddHours(8);

            SettleAccountTransaction settleTransaction = new SettleAccountTransaction
            {
                Amount = command.Amount,
                Args = command.Args,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                OrderId = Guid.Empty,
                ResultCode = 1,
                ResultTime = now,
                SequenceNo = await SequenceNoHelper.GetSequenceNoAsync(),
                Trade = Trade.Credit,
                TradeCode = TradeCodeHelper.TC1005012003,
                TransDesc = "支付成功",
                TransactionId = Guid.NewGuid(),
                TransactionTime = now,
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync()
            };

            JBYAccountTransaction jbyTransaction = new JBYAccountTransaction
            {
                Amount = command.Amount,
                Args = command.Args,
                PredeterminedResultDate = now,
                ResultCode = 1,
                ResultTime = now,
                SettleAccountTransactionId = settleTransaction.TransactionId,
                Trade = Trade.Debit,
                TradeCode = TradeCodeHelper.TC2001051102,
                TransDesc = "申购成功",
                TransactionId = command.CommandId,
                TransactionTime = now,
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync()
            };

            IJBYProduct product = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
            Guid? result = await product.BuildJBYTransactionAsync(jbyTransaction.ToInfo());
            if (result.HasValue)
            {
                jbyTransaction.ProductId = result.Value;
                this.State.SettleAccount.Add(settleTransaction.TransactionId, settleTransaction);
                this.State.JBYAccount.Add(jbyTransaction.TransactionId, jbyTransaction);

                await this.SaveStateAsync();
                this.ReloadSettleAccountData();
                this.ReloadJBYAccountData();

                await this.RaiseJBYPurchasedEvent(command, jbyTransaction.ToInfo(), settleTransaction.ToInfo());

                return jbyTransaction.ToInfo();
            }

            return null;
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
        ///     migrate as an asynchronous operation.
        /// </summary>
        /// <param name="migrationDto">The migration dto.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> MigrateAsync(UserMigrationDto migrationDto)
        {
            this.State.Args = migrationDto.Args;
            this.State.BankCards = migrationDto.BankCards;
            this.State.Cellphone = migrationDto.Cellphone;
            this.State.ClientType = migrationDto.ClientType;
            this.State.Closed = migrationDto.Closed;
            this.State.ContractId = migrationDto.ContractId;
            this.State.Credential = migrationDto.Credential;
            this.State.CredentialNo = migrationDto.CredentialNo;
            this.State.EncryptedPassword = migrationDto.EncryptedPassword;
            this.State.EncryptedPaymentPassword = migrationDto.EncryptedPaymentPassword;
            this.State.InviteBy = migrationDto.InviteBy;
            this.State.JBYAccount = migrationDto.JBYAccount;
            this.State.LoginNames = migrationDto.LoginNames;
            this.State.Orders = migrationDto.Orders;
            this.State.OutletCode = migrationDto.OutletCode;
            this.State.PaymentSalt = migrationDto.PaymentSalt;
            this.State.RealName = migrationDto.RealName;
            this.State.RegisterTime = migrationDto.RegisterTime;
            this.State.Salt = migrationDto.Salt;
            this.State.SettleAccount = migrationDto.SettleAccount;
            this.State.UserId = migrationDto.UserId;
            this.State.Verified = migrationDto.Verified;
            this.State.VerifiedTime = migrationDto.VerifiedTime;

            this.State.Args.Add("MigratingTime", DateTime.UtcNow);

            ICellphone cellphone = CellphoneFactory.GetGrain(GrainTypeHelper.GetCellphoneGrainTypeLongKey(this.State.Cellphone));
            await cellphone.Register(this.State.UserId);

            this.ReloadJBYAccountData();
            this.ReloadOrderInfosData();
            this.ReloadSettleAccountData();

            await this.State.WriteStateAsync();

            await this.SyncAsync();

            await this.RegisterOrUpdateReminder("DailyWork", TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(20));

            SiloClusterTraceLogger.Log("User {0} migrated".FormatWith(this.State.UserId));

            return await this.GetUserInfoAsync();
        }

        /// <summary>
        ///     Registers the specified user register.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task<UserInfo> RegisterAsync(UserRegister command)
        {
            if (this.State.UserId == command.UserId)
            {
                return null;
            }

            if (this.State.UserId != Guid.Empty)
            {
                this.GetLogger().Warn(1, "Conflict user id: UserId {0}, UserRegisterCommand.UserId {1}", this.State.UserId, command.UserId);
                return null;
            }

            this.BeginProcessCommandAsync(command);

            DateTime registerTime = DateTime.UtcNow.AddHours(8);
            this.State.UserId = command.UserId;
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
            this.State.JBYAccount = new Dictionary<Guid, JBYAccountTransaction>();
            this.State.SettleAccount = new Dictionary<Guid, SettleAccountTransaction>();

            await this.SaveStateAsync();

            await this.RegisterOrUpdateReminder("DailyWork", TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(20));

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
            Order order;
            if (!this.State.Orders.TryGetValue(orderId, out order))
            {
                throw new ApplicationException("Missing Order data. UserId-{0}, OrderId-{1}.".FormatWith(this.State.UserId, orderId));
            }

            if (order.IsRepaid && order.RepaidTime.HasValue)
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

            SettleAccountTransaction principalTransaction = new SettleAccountTransaction
            {
                Amount = order.Principal,
                Args = new Dictionary<string, object>(),
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                OrderId = order.OrderId,
                ResultCode = 1,
                ResultTime = now,
                SequenceNo = await SequenceNoHelper.GetSequenceNoAsync(),
                Trade = Trade.Debit,
                TradeCode = principalTradeCode,
                TransactionId = Guid.NewGuid(),
                TransDesc = "本金还款",
                TransactionTime = now,
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync()
            };

            SettleAccountTransaction interestTransaction = new SettleAccountTransaction
            {
                Amount = order.Interest + order.ExtraInterest,
                Args = new Dictionary<string, object>(),
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                OrderId = order.OrderId,
                ResultCode = 1,
                ResultTime = now,
                SequenceNo = await SequenceNoHelper.GetSequenceNoAsync(),
                Trade = Trade.Debit,
                TradeCode = interestTradeCode,
                TransactionId = Guid.NewGuid(),
                TransDesc = "产品结息",
                TransactionTime = now,
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync()
            };

            this.State.SettleAccount.Add(principalTransaction.TransactionId, principalTransaction);
            this.State.SettleAccount.Add(interestTransaction.TransactionId, interestTransaction);

            await this.SaveStateAsync();
            this.ReloadSettleAccountData();
            this.ReloadOrderInfosData();

            this.RaiseOrderRepaidEvent(order.ToInfo(), principalTransaction.ToInfo(), interestTransaction.ToInfo());
        }

        /// <summary>
        ///     Resets the login password.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task ResetLoginPasswordAsync(ResetLoginPassword command)
        {
            this.BeginProcessCommandAsync(command);

            this.State.Salt = command.Salt;
            this.State.EncryptedPassword = CryptographyHelper.Encrypting(command.Password, command.Salt);
            this.PasswordErrorCount = 0;

            await this.SaveStateAsync();

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

            this.BeginProcessCommandAsync(command);

            this.State.PaymentSalt = command.Salt;
            this.State.EncryptedPaymentPassword = CryptographyHelper.Encrypting(command.PaymentPassword, command.Salt);
            this.PaymentPasswordErrorCount = 0;

            await this.SaveStateAsync();

            await this.RaisePaymentPasswordSetEvent(command);
        }

        /// <summary>
        ///     verify bank card as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;Tuple&lt;UserInfo, BankCardInfo&gt;&gt;.</returns>
        /// <exception cref="System.ApplicationException">
        ///     Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}.FormatWith(this.State.UserId, command.CommandId)
        ///     or
        ///     Missing BankCard data. UserId-{0}, CommandId-{1}.FormatWith(this.State.UserId, command.CommandId)
        /// </exception>
        public async Task<Tuple<UserInfo, BankCardInfo>> VerifyBankCardAsync(VerifyBankCard command)
        {
            if (!this.State.Verified)
            {
                throw new ApplicationException("Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}".FormatWith(this.State.UserId, command.CommandId));
            }

            BankCard card;
            if (!this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                throw new ApplicationException("Missing BankCard data. UserId-{0}, CommandId-{1}".FormatWith(this.State.UserId, command.CommandId));
            }

            this.BeginProcessCommandAsync(command);

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
        ///     Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}..FormatWith(this.State.UserId, command.CommandId)
        ///     or
        ///     Missing BankCard data. UserId-{0}, CommandId-{1}.FormatWith(this.State.UserId, command.CommandId)
        /// </exception>
        public async Task VerifyBankCardAsync(VerifyBankCard command, bool result, string message)
        {
            if (!this.State.Verified)
            {
                throw new ApplicationException("Invalid VerifyBankCard command. UserId-{0}, CommandId-{1}.".FormatWith(this.State.UserId, command.CommandId));
            }

            BankCard card;
            if (!this.State.BankCards.TryGetValue(command.BankCardNo, out card))
            {
                throw new ApplicationException("Missing BankCard data. UserId-{0}, CommandId-{1}".FormatWith(this.State.UserId, command.CommandId));
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

            await this.SaveStateAsync();
            await this.RaiseVerifyBankCardResultedEvent(command, card.ToInfo(), result, message);
        }

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task<SettleAccountTransactionInfo> WithdrawalAsync(Withdrawal command)
        {
            SettleAccountTransaction transaction;
            if (this.State.SettleAccount.TryGetValue(command.CommandId, out transaction))
            {
                return transaction.ToInfo();
            }

            if (this.TodayWithdrawalCount >= VariableHelper.DailyWithdrawalLimitCount)
            {
                return null;
            }

            if (this.SettleAccountBalance < command.Amount)
            {
                return null;
            }

            BankCardInfo bankCardInfo = await this.GetBankCardInfoAsync(command.BankCardNo);
            if (bankCardInfo == null || !bankCardInfo.Dispaly || !bankCardInfo.Verified || !bankCardInfo.VerifiedByYilian)
            {
                return null;
            }

            if (bankCardInfo.WithdrawAmount < command.Amount)
            {
                return null;
            }

            this.BeginProcessCommandAsync(command);

            transaction = new SettleAccountTransaction
            {
                Amount = command.Amount,
                Args = command.Args,
                BankCardNo = command.BankCardNo,
                ChannelCode = ChannelCodeHelper.Yilian,
                OrderId = Guid.Empty,
                ResultCode = 0,
                ResultTime = null,
                SequenceNo = await SequenceNoHelper.GetSequenceNoAsync(),
                Trade = Trade.Credit,
                TradeCode = TradeCodeHelper.TC1005052001,
                TransactionId = command.CommandId,
                TransactionTime = DateTime.UtcNow.AddHours(8),
                TransDesc = "取现申请",
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync()
            };

            SettleAccountTransaction chargeTransaction = await this.BuildChargeTransactionAsync(command, transaction);

            this.State.SettleAccount.Add(transaction.TransactionId, transaction);
            this.State.SettleAccount.Add(chargeTransaction.TransactionId, chargeTransaction);

            await this.SaveStateAsync();
            this.ReloadSettleAccountData();

            await this.RaiseWithdrawalAcceptedEvent(command, transaction.ToInfo(), chargeTransaction.ToInfo());

            return transaction.ToInfo();
        }

        /// <summary>
        ///     withdrawal as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        public async Task<JBYAccountTransactionInfo> WithdrawalAsync(JBYWithdrawal command)
        {
            JBYAccountTransaction transaction;
            if (this.State.JBYAccount.TryGetValue(command.CommandId, out transaction))
            {
                return transaction.ToInfo();
            }

            if (this.TodayJBYWithdrawalAmount + command.Amount > VariableHelper.DailyJBYWithdrawalAmountLimit || this.JBYWithdrawalableAmount < command.Amount)
            {
                return null;
            }

            this.BeginProcessCommandAsync(command);

            transaction = new JBYAccountTransaction
            {
                Amount = command.Amount,
                Args = command.Args,
                PredeterminedResultDate = null,
                ProductId = SpecialIdHelper.WithdrawalJBYTransactionProductId,
                ResultCode = 0,
                ResultTime = null,
                SettleAccountTransactionId = Guid.NewGuid(),
                Trade = Trade.Credit,
                TradeCode = TradeCodeHelper.TC2001012002,
                TransactionId = command.CommandId,
                TransactionTime = DateTime.UtcNow.AddHours(8),
                TransDesc = "赎回申请",
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync()
            };

            IJBYProductWithdrawalManager manager = JBYProductWithdrawalManagerFactory.GetGrain(GrainTypeHelper.GetJBYProductWithdrawalManagerGrainTypeLongKey());
            DateTime? predeterminedResultDate = await manager.BuildWithdrawalTransactionAsync(transaction.ToInfo());

            if (predeterminedResultDate == null)
            {
                return null;
            }

            transaction.PredeterminedResultDate = predeterminedResultDate;

            this.State.JBYAccount.Add(transaction.TransactionId, transaction);

            await this.SaveStateAsync();
            this.ReloadJBYAccountData();

            await this.RaiseJBYWithdrawalAcceptedEvent(command, transaction.ToInfo());

            return transaction.ToInfo();
        }

        /// <summary>
        ///     withdrawal resulted as an asynchronous operation.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        public async Task<SettleAccountTransactionInfo> WithdrawalResultedAsync(Guid transactionId)
        {
            SettleAccountTransaction transaction;
            if (!this.State.SettleAccount.TryGetValue(transactionId, out transaction))
            {
                return null;
            }

            if (transaction.ResultCode != 0 || transaction.TradeCode != TradeCodeHelper.TC1005052001)
            {
                return null;
            }

            transaction.ResultCode = 1;
            transaction.ResultTime = DateTime.UtcNow.AddHours(8);
            transaction.TransDesc = "取现成功";

            await this.SaveStateAsync();
            this.ReloadSettleAccountData();

            SettleAccountTransactionInfo info = transaction.ToInfo();
            await this.RaiseWithdrawalResultedEvent(info);

            return info;
        }

        #endregion IUser Members

        /// <summary>
        ///     jby compute interest as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public async Task<JBYAccountTransactionInfo> JBYReinvestingAsync()
        {
            this.ReloadJBYAccountData();

            DateTime now = DateTime.UtcNow.AddHours(8);
            if (this.State.JBYAccount.Values.Any(t => t.TradeCode == TradeCodeHelper.TC2001011106 && t.ResultCode > 0
                                                      && t.TransactionTime.Date == now.Date))
            {
                return null;
            }

            long yield = Convert.ToInt64(DailyConfigHelper.GetDailyConfig(now.AddDays(-1)).JBYYield);

            if (this.JBYAccrualAmount <= 0)
            {
                return null;
            }

            long interest = this.JBYAccrualAmount * yield / 3600000L;

            if (interest <= 0)
            {
                return null;
            }

            JBYAccountTransaction jbyTransaction = new JBYAccountTransaction
            {
                Amount = interest,
                Args = new Dictionary<string, object> { { "Yield", yield } },
                PredeterminedResultDate = now,
                ProductId = SpecialIdHelper.ReinvestingJBYTransactionProductId,
                ResultCode = 1,
                ResultTime = now,
                SettleAccountTransactionId = Guid.Empty,
                Trade = Trade.Debit,
                TradeCode = TradeCodeHelper.TC2001011106,
                TransDesc = "利息复投成功",
                TransactionId = Guid.NewGuid(),
                TransactionTime = now,
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync()
            };

            this.State.JBYAccount.Add(jbyTransaction.TransactionId, jbyTransaction);

            await this.SaveStateAsync();
            this.ReloadJBYAccountData();

            JBYAccountTransactionInfo info = jbyTransaction.ToInfo();
            await this.RaiseJBYReinvestedEvent(info);
            return info;
        }

        /// <summary>
        ///     jby withdrawal resulted as an asynchronous operation.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task.</returns>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public async Task JBYWithdrawalResultedAsync(Guid transactionId)
        {
            JBYAccountTransaction transaction;
            if (!this.State.JBYAccount.TryGetValue(transactionId, out transaction))
            {
                return;
            }

            if (transaction.ResultCode != 0 || transaction.TradeCode != TradeCodeHelper.TC2001012002)
            {
                return;
            }

            DateTime now = DateTime.UtcNow.AddHours(8);

            SettleAccountTransaction settleAccountTransaction = new SettleAccountTransaction
            {
                Amount = transaction.Amount,
                Args = transaction.Args,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                OrderId = Guid.Empty,
                ResultCode = 1,
                ResultTime = now,
                SequenceNo = await SequenceNoHelper.GetSequenceNoAsync(),
                Trade = Trade.Debit,
                TradeCode = TradeCodeHelper.TC1005011103,
                TransactionId = transaction.SettleAccountTransactionId,
                TransactionTime = now,
                TransDesc = "金包银赎回资金到账",
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync()
            };

            transaction.SettleAccountTransactionId = settleAccountTransaction.TransactionId;
            transaction.ResultCode = 1;
            transaction.ResultTime = now;
            transaction.TransDesc = "赎回成功";

            this.State.SettleAccount.Add(settleAccountTransaction.TransactionId, settleAccountTransaction);

            await this.SaveStateAsync();
            this.ReloadJBYAccountData();
            this.ReloadSettleAccountData();

            await this.RaiseJBYWithdrawalResultedEvent(transaction.ToInfo(), settleAccountTransaction.ToInfo());
        }

        /// <summary>
        ///     Builds the interest.
        /// </summary>
        /// <param name="valueDate">The value date.</param>
        /// <param name="settleDate">The settle date.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="yield">The yield.</param>
        /// <returns>System.Int32.</returns>
        private static int BuildInterest(DateTime valueDate, DateTime settleDate, int principal, int yield)
        {
            int dayCount = (settleDate.Date.AddHours(1) - valueDate.Date).Days;
            return principal * yield * dayCount / 3600000;
        }

        /// <summary>
        ///     Gets the last investing confirm time.
        /// </summary>
        /// <param name="date">The date.UTC+8</param>
        /// <returns>System.DateTime.</returns>
        private static DateTime GetLastInvestingConfirmTime(DateTime date)
        {
            DailyConfig confirmConfig = DailyConfigHelper.GetLastWorkDayConfig(date, 1);
            return confirmConfig.Date.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        ///     Builds the charge transaction.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>Transaction.</returns>
        private async Task<SettleAccountTransaction> BuildChargeTransactionAsync(Withdrawal command, SettleAccountTransaction transaction)
        {
            DateTime now = DateTime.UtcNow.AddHours(8).AddSeconds(1);
            long chargeAmount;

            if (this.MonthWithdrawalCount < VariableHelper.MonthFreeWithrawalLimitCount)
            {
                chargeAmount = 0;
            }
            else
            {
                chargeAmount = this.SettleAccountBalance - transaction.Amount;
            }

            chargeAmount = chargeAmount > VariableHelper.WithdrawalChargeFee ? VariableHelper.WithdrawalChargeFee : chargeAmount;

            return new SettleAccountTransaction
            {
                Amount = chargeAmount,
                Args = command.Args,
                BankCardNo = string.Empty,
                ChannelCode = ChannelCodeHelper.Jinyinmao,
                ResultCode = 1,
                ResultTime = now,
                SequenceNo = await SequenceNoHelper.GetSequenceNoAsync(),
                Trade = Trade.Credit,
                TradeCode = TradeCodeHelper.TC1005012102,
                TransDesc = "账户取现手续费",
                TransactionId = Guid.NewGuid(),
                TransactionTime = now,
                UserId = this.State.UserId,
                UserInfo = await this.GetUserInfoAsync()
            };
        }

        /// <summary>
        ///     Gets the jby accrual amount.
        /// </summary>
        /// <param name="date">The date.UTC+8</param>
        /// <param name="reload">The reload.</param>
        /// <returns>System.Int32.</returns>
        private long GetJBYAccrualAmount(DateTime date, bool reload = false)
        {
            DateTime confirmTime = GetLastInvestingConfirmTime(date);
            if (reload)
            {
                this.ReloadJBYAccountData();
            }

            List<JBYAccountTransaction> transactions = this.State.JBYAccount.Values.ToList();
            long investedAmount = transactions.Where(t => t.Trade == Trade.Debit && t.ResultCode > 0 && t.ResultTime.GetValueOrDefault(DateTime.MaxValue) <= confirmTime).Sum(t => t.Amount);
            long creditedTransAmount = transactions.Where(t => t.Trade == Trade.Credit && t.ResultCode > 0 && t.ResultTime.GetValueOrDefault(DateTime.MaxValue) < date.AddDays(1).Date).Sum(t => t.Amount);
            long creditingTransAmount = transactions.Where(t => t.Trade == Trade.Credit && t.ResultCode == 0 && t.PredeterminedResultDate.GetValueOrDefault(DateTime.MaxValue) < date.AddDays(1).Date).Sum(t => t.Amount);

            return investedAmount - creditedTransAmount - creditingTransAmount;
        }

        /// <summary>
        ///     Gets the withrawal charge.
        /// </summary>
        /// <returns>System.Int64.</returns>
        private long GetWithrawalCharge()
        {
            int charge = 0;
            if (this.MonthWithdrawalCount >= VariableHelper.MonthFreeWithrawalLimitCount)
            {
                charge = VariableHelper.WithdrawalChargeFee;
            }

            return Convert.ToInt64(charge);
        }
    }
}