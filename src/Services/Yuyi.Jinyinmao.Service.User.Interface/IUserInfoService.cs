// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  12:57 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-24  2:20 PM
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
        ///     Gets the jby account reinvesting transaction infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;JBYAccountTransactionInfo&gt;&gt;.</returns>
        Task<PaginatedList<JBYAccountTransactionInfo>> GetJBYAccountReinvestingTransactionInfosAsync(Guid userId, int pageIndex, int pageSize);

        /// <summary>
        ///     Gets the jby account transaction information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;TransactionInfo&gt;.</returns>
        Task<JBYAccountTransactionInfo> GetJBYAccountTransactionInfoAsync(Guid userId, Guid transactionId);

        /// <summary>
        ///     Gets the jby account transaction infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;JBYAccountTransactionInfo&gt;&gt;.</returns>
        Task<PaginatedList<JBYAccountTransactionInfo>> GetJBYAccountTransactionInfosAsync(Guid userId, int pageIndex, int pageSize);

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
        /// <returns>Task&lt;PaginatedList&lt;OrderInfo&gt;&gt;.</returns>
        Task<PaginatedList<OrderInfo>> GetOrderInfosAsync(Guid userId, int pageIndex, int pageSize, OrdersSortMode ordersSortMode);

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
        ///     Gets the settle account transaction information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;TransactionInfo&gt;.</returns>
        Task<SettleAccountTransactionInfo> GetSettleAccountTransactionInfoAsync(Guid userId, Guid transactionId);

        /// <summary>
        ///     Gets the settle account transaction information asynchronous.
        /// </summary>
        /// <param name="userId">The useri identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;IPaginatedList&lt;TransactionInfo&gt;&gt;.</returns>
        Task<PaginatedList<SettleAccountTransactionInfo>> GetSettleAccountTransactionInfosAsync(Guid userId, int pageIndex, int pageSize);

        /// <summary>
        /// Gets the settling order infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="count">The count.</param>
        /// <returns>Task&lt;List&lt;OrderInfo&gt;&gt;.</returns>
        Task<List<OrderInfo>> GetSettlingOrderInfosAsync(Guid userId, int count);

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