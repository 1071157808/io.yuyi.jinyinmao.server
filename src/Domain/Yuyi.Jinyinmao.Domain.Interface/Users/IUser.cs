// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IUser.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  8:53
// ***********************************************************************
// <copyright file="IUser.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IUser
    /// </summary>
    public interface IUser : IGrainWithGuidKey
    {
        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        Task<BankCardInfo> AddBankCardAsync(AddBankCard command);

        /// <summary>
        ///     Authenticates the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> AuthenticateAsync(Authenticate command);

        /// <summary>
        ///     Authenticates the resulted asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> AuthenticateResultedAsync(Authenticate command, bool result, string message);

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;CheckPasswordResult&gt;.</returns>
        Task<CheckPasswordResult> CheckPasswordAsync(string loginName, string password);

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> CheckPasswordAsync(string password);

        /// <summary>
        ///     Checks the payment password asynchronous.
        /// </summary>
        /// <param name="paymentPassword">The payment password.</param>
        /// <returns>Task&lt;CheckPaymentPasswordResult&gt;.</returns>
        Task<CheckPaymentPasswordResult> CheckPaymentPasswordAsync(string paymentPassword);

        /// <summary>
        /// Deposits the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;Tuple&lt;UserInfo, SettleAccountTransactionInfo, BankCardInfo&gt;&gt;.</returns>
        Task<Tuple<UserInfo, SettleAccountTransactionInfo, BankCardInfo>> DepositAsync(PayCommand command);

        /// <summary>
        ///     Deposits the resulted asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task DepositResultedAsync(PayCommand command, bool result, string message);

        /// <summary>
        ///     Does the daily work asynchronous.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns>Task.</returns>
        Task DoDailyWorkAsync(bool force = false);

        /// <summary>
        ///     Dumps the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task DumpAsync();

        /// <summary>
        ///     Gets the bank card information asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        Task<BankCardInfo> GetBankCardInfoAsync(string bankCardNo);

        /// <summary>
        ///     Gets the bank card infos asynchronous.
        /// </summary>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        Task<List<BankCardInfo>> GetBankCardInfosAsync();

        /// <summary>
        ///     Gets the jby account information asynchronous.
        /// </summary>
        /// <returns>Task&lt;JBYAccountInfo&gt;.</returns>
        Task<JBYAccountInfo> GetJBYAccountInfoAsync();

        /// <summary>
        ///     Gets the jby account reinvesting transaction infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;JBYAccountTransactionInfo&gt;&gt;.</returns>
        Task<PaginatedList<JBYAccountTransactionInfo>> GetJBYAccountReinvestingTransactionInfosAsync(int pageIndex, int pageSize);

        /// <summary>
        ///     Gets the jby account transaction information asynchronous.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        Task<JBYAccountTransactionInfo> GetJBYAccountTransactionInfoAsync(Guid transactionId);

        /// <summary>
        ///     Gets the jby account transaction infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;JBYAccountTransactionInfo&gt;&gt;.</returns>
        Task<PaginatedList<JBYAccountTransactionInfo>> GetJBYAccountTransactionInfosAsync(int pageIndex, int pageSize);

        /// <summary>
        ///     Gets the order information asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>Task&lt;OrderInfo&gt;.</returns>
        Task<OrderInfo> GetOrderInfoAsync(Guid orderId);

        /// <summary>
        ///     Gets the order infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="ordersSortMode">The orders sort mode.</param>
        /// <returns>Task&lt;Tuple&lt;System.Int32, List&lt;OrderInfo&gt;&gt;&gt;.</returns>
        Task<Tuple<int, List<OrderInfo>>> GetOrderInfosAsync(int pageIndex, int pageSize, OrdersSortMode ordersSortMode);

        /// <summary>
        ///     Gets the order infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="ordersSortMode">The orders sort mode.</param>
        /// <param name="categories">The categories.</param>
        /// <returns>Task&lt;Tuple&lt;System.Int32, List&lt;OrderInfo&gt;&gt;&gt;.</returns>
        Task<Tuple<int, List<OrderInfo>>> GetOrderInfosAsync(int pageIndex, int pageSize, OrdersSortMode ordersSortMode, IEnumerable<long> categories);

        /// <summary>
        ///     Gets the settle account information asynchronous.
        /// </summary>
        /// <returns>Task&lt;SettleAccountInfo&gt;.</returns>
        Task<SettleAccountInfo> GetSettleAccountInfoAsync();

        /// <summary>
        ///     Gets the settle account transaction information asynchronous.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        Task<SettleAccountTransactionInfo> GetSettleAccountTransactionInfoAsync(Guid transactionId);

        /// <summary>
        ///     Gets the settle account transaction infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;SettleAccountTransactionInfo&gt;&gt;.</returns>
        Task<PaginatedList<SettleAccountTransactionInfo>> GetSettleAccountTransactionInfosAsync(int pageIndex, int pageSize);

        /// <summary>
        ///     Gets the settling order infos asynchronous.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>Task&lt;List&lt;OrderInfo&gt;&gt;.</returns>
        Task<List<OrderInfo>> GetSettlingOrderInfosAsync(int count);

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> GetUserInfoAsync();

        /// <summary>
        ///     Gets the withdrawalable bank card infos asynchronous.
        /// </summary>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        Task<List<BankCardInfo>> GetWithdrawalableBankCardInfosAsync();

        /// <summary>
        /// Hides the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        Task<BankCardInfo> HideBankCardAsync(HideBankCard command);

        /// <summary>
        /// Inserts the jby account transcation asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        Task<JBYAccountTransactionInfo> InsertJBYAccountTranscationAsync(InsertJBYAccountTransaction command);

        /// <summary>
        /// Inserts the settle account transcation asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        Task<SettleAccountTransactionInfo> InsertSettleAccountTranscationAsync(InsertSettleAccountTransaction command);

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task<OrderInfo> InvestingAsync(RegularInvesting command);

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;TransactionInfo&gt;.</returns>
        Task<JBYAccountTransactionInfo> InvestingAsync(JBYInvesting command);

        /// <summary>
        ///     Determines whether [is registered] asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> IsRegisteredAsync();

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        Task<JBYAccountTransactionInfo> JBYWithdrawalAsync(JBYWithdrawal command);

        /// <summary>
        ///     Jby the withdrawal resulted asynchronous.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task.</returns>
        Task JBYWithdrawalResultedAsync(Guid transactionId);

        /// <summary>
        ///     Locks the asynchronous.
        /// </summary>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> LockAsync();

        /// <summary>
        ///     Migrates the asynchronous.
        /// </summary>
        /// <param name="migrationDto">The migration dto.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> MigrateAsync(UserMigrationDto migrationDto);

        /// <summary>
        ///     Registers the specified user register.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Task.</returns>
        Task<UserInfo> RegisterAsync(UserRegister command);

        /// <summary>
        ///     Reloads the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task ReloadAsync();

        /// <summary>
        ///     Repays the order asynchronous.
        /// </summary>
        /// <param name="orderRepayCommand">The order repay command.</param>
        /// <returns></returns>
        Task<OrderInfo> RepayOrderAsync(OrderRepay orderRepayCommand);

        /// <summary>
        /// Resets the login password.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> ResetLoginPasswordAsync(ResetLoginPassword command);

        /// <summary>
        /// Sets the jby account transaction result asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        Task<JBYAccountTransactionInfo> SetJBYAccountTransactionResultAsync(SetJBYAccountTransactionResult command);

        /// <summary>
        /// Sets the payment password asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> SetPaymentPasswordAsync(SetPaymentPassword command);

        /// <summary>
        /// Sets the settle account transaction result asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        Task<SettleAccountTransactionInfo> SetSettleAccountTransactionResultAsync(SetSettleAccountTransactionResult command);

        /// <summary>
        ///     Signs the asynchronous.
        /// </summary>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        Task<SettleAccountTransactionInfo> SignAsync();

        /// <summary>
        ///     Synchronizes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task SyncAsync();

        /// <summary>
        ///     Unlocks the asynchronous.
        /// </summary>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> UnlockAsync();

        /// <summary>
        ///     Verifies the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;Tuple&lt;UserInfo, BankCardInfo&gt;&gt;.</returns>
        Task<Tuple<UserInfo, BankCardInfo>> VerifyBankCardAsync(VerifyBankCard command);

        /// <summary>
        ///     Verifies the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task VerifyBankCardAsync(VerifyBankCard command, bool result, string message);

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        Task<SettleAccountTransactionInfo> WithdrawalAsync(Withdrawal command);

        /// <summary>
        ///     Withdrawals the resulted asynchronous.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        Task<SettleAccountTransactionInfo> WithdrawalResultedAsync(Guid transactionId);
    }
}