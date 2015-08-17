// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IUserService.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  10:10
// ***********************************************************************
// <copyright file="IUserService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Interface IUserService
    /// </summary>
    public interface IUserService : IUserInfoService
    {
        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        Task<BankCardInfo> AddBankCardAsync(AddBankCard command);

        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="addBankCardCommand">The add bank card command.</param>
        /// <param name="verifyBankCardCommand">The verify bank card command.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        Task AddBankCardAsync(AddBankCard addBankCardCommand, VerifyBankCard verifyBankCardCommand);

        /// <summary>
        ///     Authenticates the asynchronous.
        /// </summary>
        /// <param name="addBankCardCommand">The add bank card command.</param>
        /// <param name="authenticateCommand">The authenticate command.</param>
        /// <returns>Task.</returns>
        Task AuthenticateAsync(AddBankCard addBankCardCommand, Authenticate authenticateCommand);

        /// <summary>
        ///     Checks the payment password asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="paymentPassword">The payment password.</param>
        /// <returns>Task&lt;CheckPaymentPasswordResult&gt;.</returns>
        Task<CheckPaymentPasswordResult> CheckPaymentPasswordAsync(Guid userId, string paymentPassword);

        /// <summary>
        ///     Deposits the asynchronous.
        /// </summary>
        /// <param name="payByYilian">The pay by yilian.</param>
        /// <returns>Task.</returns>
        Task DepositAsync(PayByYilian payByYilian);

        /// <summary>
        ///     Hides the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        Task<BankCardInfo> HideBankCardAsync(HideBankCard command);

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The regular investing.</param>
        /// <returns>Task.</returns>
        Task<OrderInfo> InvestingAsync(RegularInvesting command);

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The regular investing.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        Task<JBYAccountTransactionInfo> InvestingAsync(JBYInvesting command);

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        Task<JBYAccountTransactionInfo> JBYWithdrawalAsync(JBYWithdrawal command);

        /// <summary>
        ///     Registers the user asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> RegisterUserAsync(UserRegister command);

        /// <summary>
        ///     Reloads the data asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task.</returns>
        Task ReloadDataAsync(Guid userId);

        /// <summary>
        ///     Resets the login password asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> ResetLoginPasswordAsync(ResetLoginPassword command);

        /// <summary>
        ///     Sets the payment password asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> SetPaymentPasswordAsync(SetPaymentPassword command);

        /// <summary>
        ///     Signs the asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        Task<SettleAccountTransactionInfo> SignAsync(Guid userId);

        /// <summary>
        ///     Verifies the bank card asynchronous.
        /// </summary>
        /// <param name="verifyBankCardCommand">The verify bank card command.</param>
        /// <returns>Task.</returns>
        Task VerifyBankCardAsync(VerifyBankCard verifyBankCardCommand);

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The withdrawal.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        Task<SettleAccountTransactionInfo> WithdrawalAsync(Withdrawal command);

        /// <summary>
        ///     Withdrawals the resulted asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        Task<SettleAccountTransactionInfo> WithdrawalResultedAsync(Guid userId, Guid transactionId);
    }
}