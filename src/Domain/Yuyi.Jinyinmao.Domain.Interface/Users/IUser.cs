// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:25 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  1:57 AM
// ***********************************************************************
// <copyright file="IUser.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IUser
    /// </summary>
    public interface IUser : IGrain
    {
        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task AddBankCardAsync(AddBankCard command);

        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="addBankCardSagaInitDto">The add bank card saga initialize dto.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>Task.</returns>
        Task AddBankCardResultedAsync(AddBankCardSagaInitDto addBankCardSagaInitDto, bool result);

        /// <summary>
        ///     Authenticatings the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task AuthenticateAsync(Authenticate command);

        /// <summary>
        ///     Authenticates the resulted asynchronous.
        /// </summary>
        /// <param name="authenticateSagaInitDto">The authenticate saga initialize dto.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>Task.</returns>
        Task AuthenticateResultedAsync(AuthenticateSagaInitDto authenticateSagaInitDto, bool result);

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
        /// Checks the payment password asynchronous.
        /// </summary>
        /// <param name="paymentPassword">The payment password.</param>
        /// <returns>Task&lt;CheckPaymentPasswordResult&gt;.</returns>
        Task<CheckPaymentPasswordResult> CheckPaymentPasswordAsync(string paymentPassword);

        /// <summary>
        /// Clears the unauthenticated information.
        /// </summary>
        /// <returns>Task.</returns>
        Task ClearUnauthenticatedInfo();

        /// <summary>
        ///     Deposits from the settle account asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task DepositAsync(DepositFromYilian command);

        /// <summary>
        ///     Deposits the resulted asynchronous.
        /// </summary>
        /// <param name="depositFromYilianSagaInitDto">The deposit saga initialize dto.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task DepositResultedAsync(DepositFromYilianSagaInitDto depositFromYilianSagaInitDto, bool result, string message);

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
        /// Gets the settle account information asynchronous.
        /// </summary>
        /// <returns>Task&lt;SettleAccountInfo&gt;.</returns>
        Task<SettleAccountInfo> GetSettleAccountInfoAsync();

        /// <summary>
        ///     Gets the transcation information asynchronous.
        /// </summary>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task&lt;TranscationInfo&gt;.</returns>
        Task<TranscationInfo> GetSettleAccountTranscationInfoAsync(Guid transcationId);

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> GetUserInfoAsync();

        /// <summary>
        /// Investings the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task InvestingAsync(RegularInvesting command);

        /// <summary>
        ///     Determines whether [is registered] asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> IsRegisteredAsync();

        /// <summary>
        ///     Registers the specified user register.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Task.</returns>
        Task RegisterAsync(UserRegister command);

        /// <summary>
        /// Repays the order asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="repaidTime">The repaid time.</param>
        /// <returns>Task.</returns>
        Task RepayOrderAsync(Guid orderId, DateTime repaidTime);

        /// <summary>
        ///     Resets the login password.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task ResetLoginPasswordAsync(ResetLoginPassword command);

        /// <summary>
        ///     Sets the default bank card asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task.</returns>
        Task SetDefaultBankCardAsync(string bankCardNo);

        /// <summary>
        ///     Sets the payment password asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task SetPaymentPasswordAsync(SetPaymentPassword command);

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task WithdrawalAsync(Withdrawal command);

        /// <summary>
        ///     Withdrawals the resulted asynchronous.
        /// </summary>
        /// <param name="parseExact">The parse exact.</param>
        /// <returns>Task.</returns>
        Task WithdrawalResultedAsync(Guid parseExact);
    }
}