// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-12  3:23 AM
// ***********************************************************************
// <copyright file="UserService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Dtos;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     Class UserService.
    /// </summary>
    public class UserService : IUserService
    {
        #region IUserService Members

        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task AddBankCardAsync(AddBankCard command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.AddBankCardAsync(command);
        }

        /// <summary>
        ///     Authenticatings the asynchronous.
        /// </summary>
        /// <param name="command">The apply for authentication.</param>
        /// <returns>Task.</returns>
        public Task AuthenticateAsync(Authenticate command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.AuthenticateAsync(command);
        }

        /// <summary>
        ///     Checks the cellphone asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;CheckCellphoneResult&gt;.</returns>
        public async Task<CheckCellphoneResult> CheckCellphoneAsync(string cellphone)
        {
            ICellphone cellphoneGrain = CellphoneFactory.GetGrain(GrainTypeHelper.GetGrainTypeLongKey(GrainType.Cellphone, cellphone));
            CellphoneInfo info = await cellphoneGrain.GetCellphoneInfoAsync();

            return new CheckCellphoneResult
            {
                Result = info.Registered
            };
        }

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;SignInResult&gt;.</returns>
        public Task<bool> CheckPasswordAsync(Guid userId, string password)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.CheckPasswordAsync(password);
        }

        /// <summary>
        ///     Sign in asynchronous.
        /// </summary>
        /// <param name="cellphone"></param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;SignInResult&gt;.</returns>
        public async Task<SignInResult> CheckPasswordViaCellphoneAsync(string cellphone, string password)
        {
            ICellphone cellphoneGrain = CellphoneFactory.GetGrain(GrainTypeHelper.GetGrainTypeLongKey(GrainType.Cellphone, cellphone));
            CellphoneInfo info = await cellphoneGrain.GetCellphoneInfoAsync();
            IUser user = UserFactory.GetGrain(info.UserId);
            CheckPasswordResult result = await user.CheckPasswordAsync(cellphone, password);
            return new SignInResult
            {
                Cellphone = result.Cellphone,
                RemainCount = 5 - result.ErrorCount,
                Success = result.Success,
                UserExist = result.UserExist,
                UserId = result.UserId
            };
        }

        /// <summary>
        ///     Checks the payment password asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="paymentPassword">The payment password.</param>
        /// <returns>Task&lt;CheckPaymentPasswordResult&gt;.</returns>
        public Task<CheckPaymentPasswordResult> CheckPaymentPasswordAsync(Guid userId, string paymentPassword)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.CheckPaymentPasswordAsync(paymentPassword);
        }

        /// <summary>
        ///     Clears the unauthenticated information.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public Task ClearUnauthenticatedInfo(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.ClearUnauthenticatedInfo();
        }

        /// <summary>
        ///     Deposits from the settle account.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task DepositAsync(DepositFromYilian command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.DepositAsync(command);
        }

        /// <summary>
        ///     Gets the bank card information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        public Task<BankCardInfo> GetBankCardInfoAsync(Guid userId, string bankCardNo)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetBankCardInfoAsync(bankCardNo);
        }

        /// <summary>
        ///     Gets the bank card infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        public Task<List<BankCardInfo>> GetBankCardInfosAsync(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetBankCardInfosAsync();
        }

        /// <summary>
        ///     Gets the jby account information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;JBYAccountInfo&gt;.</returns>
        public Task<JBYAccountInfo> GetJBYAccountInfoAsync(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetJBYAccountInfoAsync();
        }

        /// <summary>
        ///     Gets the jby account transcation information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task&lt;TranscationInfo&gt;.</returns>
        public Task<TranscationInfo> GetJBYAccountTranscationInfoAsync(Guid userId, Guid transcationId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetJBYAccountTranscationInfoAsync(transcationId);
        }

        /// <summary>
        ///     Gets the jby account transcation infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;TranscationInfo&gt;&gt;.</returns>
        public Task<PaginatedList<TranscationInfo>> GetJBYAccountTranscationInfosAsync(Guid userId, int pageIndex, int pageSize)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetJBYAccountTranscationInfosAsync(pageIndex, pageSize);
        }

        /// <summary>
        ///     Gets the order infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="ordersSortMode">The orders sort mode.</param>
        /// <param name="categories">The categories.</param>
        /// <returns>PaginatedList&lt;OrderInfo&gt;.</returns>
        public Task<PaginatedList<OrderInfo>> GetOrderInfosAsync(Guid userId, int pageIndex, int pageSize, OrdersSortMode ordersSortMode, long[] categories)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetOrderInfosAsync(pageIndex, pageSize, ordersSortMode, categories);
        }

        /// <summary>
        ///     Gets the settle account information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;SettleAccountInfo&gt;.</returns>
        public Task<SettleAccountInfo> GetSettleAccountInfoAsync(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetSettleAccountInfoAsync();
        }

        /// <summary>
        ///     Gets the settle account transcation information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task&lt;TranscationInfo&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<TranscationInfo> GetSettleAccountTranscationInfoAsync(Guid userId, Guid transcationId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetSettleAccountTranscationInfoAsync(transcationId);
        }

        /// <summary>
        ///     Gets the settle account transcation information asynchronous.
        /// </summary>
        /// <param name="userId">The useri identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;IPaginatedList&lt;TranscationInfo&gt;&gt;.</returns>
        public Task<PaginatedList<TranscationInfo>> GetSettleAccountTranscationInfosAsync(Guid userId, int pageIndex, int pageSize)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetSettleAccountTranscationInfosAsync(pageIndex, pageSize);
        }

        /// <summary>
        ///     Gets the sign up user identifier information asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;SignUpUserIdInfo&gt;.</returns>
        public async Task<SignUpUserIdInfo> GetSignUpUserIdInfoAsync(string cellphone)
        {
            ICellphone cellphoneGrain = CellphoneFactory.GetGrain(GrainTypeHelper.GetGrainTypeLongKey(GrainType.Cellphone, cellphone));
            CellphoneInfo info = await cellphoneGrain.GetCellphoneInfoAsync();
            return new SignUpUserIdInfo
            {
                Cellphone = info.Cellphone,
                Registered = info.Registered,
                UserId = info.UserId
            };
        }

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> GetUserInfoAsync(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            UserInfo info = await user.GetUserInfoAsync();
            return info.Cellphone.IsNullOrEmpty() ? null : info;
        }

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The regular investing.</param>
        /// <returns>Task.</returns>
        public Task<OrderInfo> InvestingAsync(RegularInvesting command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.InvestingAsync(command);
        }

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The regular investing.</param>
        /// <returns>Task.</returns>
        public Task<TranscationInfo> InvestingAsync(JBYInvesting command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.InvestingAsync(command);
        }

        /// <summary>
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;ICommandHanderResult&lt;TResult&gt;&gt;.</returns>
        public async Task<UserInfo> RegisterUserAsync(UserRegister command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            await user.RegisterAsync(command);
            return await user.GetUserInfoAsync();
        }

        /// <summary>
        ///     Reloads the data asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task.</returns>
        public Task ReloadDataAsync(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.ReloadAsync();
        }

        /// <summary>
        ///     Resets the login password.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task ResetLoginPasswordAsync(ResetLoginPassword command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.ResetLoginPasswordAsync(command);
        }

        /// <summary>
        ///     Sets the default bank card asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task.</returns>
        public Task SetDefaultBankCardAsync(Guid userId, string bankCardNo)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.SetDefaultBankCardAsync(bankCardNo);
        }

        /// <summary>
        ///     Sets the payment password asynchronous.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Task.</returns>
        public Task SetPaymentPasswordAsync(SetPaymentPassword command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.SetPaymentPasswordAsync(command);
        }

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The withdrawal.</param>
        /// <returns>Task.</returns>
        public Task<TranscationInfo> WithdrawalAsync(Withdrawal command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.WithdrawalAsync(command);
        }

        /// <summary>
        /// Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;TranscationInfo&gt;.</returns>
        public Task<TranscationInfo> WithdrawalAsync(JBYWithdrawal command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.WithdrawalAsync(command);
        }

        /// <summary>
        ///     Withdrawals the resulted asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transcationId">The transcation identifier.</param>
        /// <returns>Task.</returns>
        public Task WithdrawalResultedAsync(Guid userId, Guid transcationId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.WithdrawalResultedAsync(transcationId);
        }

        #endregion IUserService Members
    }
}