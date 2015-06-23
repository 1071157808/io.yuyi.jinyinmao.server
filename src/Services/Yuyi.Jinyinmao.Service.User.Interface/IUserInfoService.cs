// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  12:57 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-09  10:50 PM
// ***********************************************************************
// <copyright file="IUserInfoService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Dtos;

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Interface IUserInfoService
    /// </summary>
    public interface IUserInfoService
    {
        /// <summary>
        ///     Checks the bank card used asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> CheckBankCardUsedAsync(string bankCardNo);

        /// <summary>
        ///     Checks the cellphone asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;CheckCellphoneResult&gt;.</returns>
        Task<CheckCellphoneResult> CheckCellphoneAsync(string cellphone);

        /// <summary>
        ///     Checks the credential no used asynchronous.
        /// </summary>
        /// <param name="credentialNo">The credential no.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> CheckCredentialNoUsedAsync(string credentialNo);

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> CheckPasswordAsync(Guid userId, string password);

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;SignInResult&gt;.</returns>
        Task<SignInResult> CheckPasswordViaCellphoneAsync(string cellphone, string password);

        /// <summary>
        ///     Gets the bank card information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        Task<BankCardInfo> GetBankCardInfoAsync(Guid userId, string bankCardNo);

        /// <summary>
        ///     Gets the bank card infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        Task<List<BankCardInfo>> GetBankCardInfosAsync(Guid userId);

        /// <summary>
        ///     Gets the jby account information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;JBYAccountInfo&gt;.</returns>
        Task<JBYAccountInfo> GetJBYAccountInfoAsync(Guid userId);

        /// <summary>
        /// Gets the jby account reinvesting transcation infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;JBYAccountTranscationInfo&gt;&gt;.</returns>
        Task<PaginatedList<JBYAccountTranscationInfo>> GetJBYAccountReinvestingTranscationInfosAsync(Guid userId, int pageIndex, int pageSize);

        /// <summary>
        ///     Gets the jby account transcation information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task&lt;TranscationInfo&gt;.</returns>
        Task<JBYAccountTranscationInfo> GetJBYAccountTranscationInfoAsync(Guid userId, Guid transcationId);

        /// <summary>
        ///     Gets the jby account transcation infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;JBYAccountTranscationInfo&gt;&gt;.</returns>
        Task<PaginatedList<JBYAccountTranscationInfo>> GetJBYAccountTranscationInfosAsync(Guid userId, int pageIndex, int pageSize);

        /// <summary>
        ///     Gets the order information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>Task&lt;OrderInfo&gt;.</returns>
        Task<OrderInfo> GetOrderInfoAsync(Guid userId, Guid orderId);

        /// <summary>
        ///     Gets the order infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="ordersSortMode">The orders sort mode.</param>
        /// <param name="categories">The categories.</param>
        /// <returns>Task&lt;PaginatedList&lt;OrderInfo&gt;&gt;.</returns>
        Task<PaginatedList<OrderInfo>> GetOrderInfosAsync(Guid userId, int pageIndex, int pageSize, OrdersSortMode ordersSortMode, long[] categories);

        /// <summary>
        ///     Gets the settle account information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;SettleAccountInfo&gt;.</returns>
        Task<SettleAccountInfo> GetSettleAccountInfoAsync(Guid userId);

        /// <summary>
        ///     Gets the settle account transcation information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task&lt;TranscationInfo&gt;.</returns>
        Task<SettleAccountTranscationInfo> GetSettleAccountTranscationInfoAsync(Guid userId, Guid transcationId);

        /// <summary>
        ///     Gets the settle account transcation information asynchronous.
        /// </summary>
        /// <param name="userId">The useri identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;IPaginatedList&lt;TranscationInfo&gt;&gt;.</returns>
        Task<PaginatedList<SettleAccountTranscationInfo>> GetSettleAccountTranscationInfosAsync(Guid userId, int pageIndex, int pageSize);

        /// <summary>
        ///     Gets the sign up user identifier information asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;SignUpUserIdInfo&gt;.</returns>
        Task<SignUpUserIdInfo> GetSignUpUserIdInfoAsync(string cellphone);

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> GetUserInfoAsync(Guid userId);

        /// <summary>
        ///     Gets the withdrawalable bank card infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        Task<List<BankCardInfo>> GetWithdrawalableBankCardInfosAsync(Guid userId);
    }
}