// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  1:35 AM
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
        /// Adds the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        Task<BankCardInfo> AddBankCardAsync(AddBankCard command);

        /// <summary>
        /// Adds the bank card asynchronous.
        /// </summary>
        /// <param name="addBankCardCommand">The add bank card command.</param>
        /// <param name="verifyBankCardCommand">The verify bank card command.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        Task AddBankCardAsync(AddBankCard addBankCardCommand, VerifyBankCard verifyBankCardCommand);

        /// <summary>
        ///     Authenticatings the asynchronous.
        /// </summary>
        /// <param name="command">The apply for authentication.</param>
        /// <returns>Task.</returns>
        Task AuthenticateAsync(Authenticate command);

        /// <summary>
        /// Authenticates the asynchronous.
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
        ///     Clears the unauthenticated information.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task.</returns>
        Task ClearUnauthenticatedInfo(Guid userId);

        /// <summary>
        /// Deposits the asynchronous.
        /// </summary>
        /// <param name="payByYilian">The pay by yilian.</param>
        /// <returns>Task.</returns>
        Task DepositAsync(PayByYilian payByYilian);

        /// <summary>
        /// Hides the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task HideBankCardAsync(HideBankCard command);

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
        /// <returns>Task&lt;JBYAccountTranscationInfo&gt;.</returns>
        Task<JBYAccountTranscationInfo> InvestingAsync(JBYInvesting command);

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
        /// <returns>Task.</returns>
        Task ResetLoginPasswordAsync(ResetLoginPassword command);

        /// <summary>
        ///     Sets the payment password asynchronous.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Task.</returns>
        Task SetPaymentPasswordAsync(SetPaymentPassword command);

        /// <summary>
        /// Verifies the bank card asynchronous.
        /// </summary>
        /// <param name="verifyBankCardCommand">The verify bank card command.</param>
        /// <returns>Task.</returns>
        Task VerifyBankCardAsync(VerifyBankCard verifyBankCardCommand);

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The withdrawal.</param>
        /// <returns>Task&lt;SettleAccountTranscationInfo&gt;.</returns>
        Task<SettleAccountTranscationInfo> WithdrawalAsync(Withdrawal command);

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;JBYAccountTranscationInfo&gt;.</returns>
        Task<JBYAccountTranscationInfo> WithdrawalAsync(JBYWithdrawal command);

        /// <summary>
        /// Withdrawals the resulted asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task&lt;SettleAccountTranscationInfo&gt;.</returns>
        Task<SettleAccountTranscationInfo> WithdrawalResultedAsync(Guid userId, Guid transcationId);
    }
}