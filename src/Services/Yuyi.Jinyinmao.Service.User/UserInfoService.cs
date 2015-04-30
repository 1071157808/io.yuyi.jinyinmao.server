// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  12:59 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-29  11:37 AM
// ***********************************************************************
// <copyright file="UserInfoService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Dtos;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     UserInfoService.
    /// </summary>
    public class UserInfoService : IUserInfoService
    {
        private readonly IUserService innerService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserInfoService" /> class.
        /// </summary>
        /// <param name="innerService">The inter service.</param>
        public UserInfoService(IUserService innerService)
        {
            this.innerService = innerService;
        }

        #region IUserInfoService Members

        /// <summary>
        ///     Checks the cellphone asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;CheckCellphoneResult&gt;.</returns>
        public Task<CheckCellphoneResult> CheckCellphoneAsync(string cellphone)
        {
            return this.innerService.CheckCellphoneAsync(cellphone);
        }

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> CheckPasswordAsync(Guid userId, string password)
        {
            return this.innerService.CheckPasswordAsync(userId, password);
        }

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;SignInResult&gt;.</returns>
        public Task<SignInResult> CheckPasswordViaCellphoneAsync(string cellphone, string password)
        {
            return this.innerService.CheckPasswordViaCellphoneAsync(cellphone, password);
        }

        /// <summary>
        ///     Gets the bank card information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        public Task<BankCardInfo> GetBankCardInfoAsync(Guid userId, string bankCardNo)
        {
            return this.innerService.GetBankCardInfoAsync(userId, bankCardNo);
        }

        /// <summary>
        ///     Gets the bank card infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        public Task<List<BankCardInfo>> GetBankCardInfosAsync(Guid userId)
        {
            return this.innerService.GetBankCardInfosAsync(userId);
        }

        /// <summary>
        ///     Gets the sign up user identifier information asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;SignUpUserIdInfo&gt;.</returns>
        public Task<SignUpUserIdInfo> GetSignUpUserIdInfoAsync(string cellphone)
        {
            return this.innerService.GetSignUpUserIdInfoAsync(cellphone);
        }

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetUserInfoAsync(Guid userId)
        {
            return this.innerService.GetUserInfoAsync(userId);
        }

        #endregion IUserInfoService Members
    }
}
