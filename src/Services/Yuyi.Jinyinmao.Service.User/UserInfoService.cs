// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  12:59 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  1:02 AM
// ***********************************************************************
// <copyright file="UserInfoService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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
        private readonly IUserInfoService interService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserInfoService" /> class.
        /// </summary>
        /// <param name="interService">The inter service.</param>
        public UserInfoService(IUserInfoService interService)
        {
            this.interService = interService;
        }

        #region IUserInfoService Members

        /// <summary>
        ///     Checks the cellphone asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;CheckCellphoneResult&gt;.</returns>
        public Task<CheckCellphoneResult> CheckCellphoneAsync(string cellphone)
        {
            return this.interService.CheckCellphoneAsync(cellphone);
        }

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> CheckPasswordAsync(Guid userId, string password)
        {
            return this.interService.CheckPasswordAsync(userId, password);
        }

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;SignInResult&gt;.</returns>
        public Task<SignInResult> CheckPasswordViaCellphoneAsync(string cellphone, string password)
        {
            return this.interService.CheckPasswordViaCellphoneAsync(cellphone, password);
        }

        /// <summary>
        ///     Gets the sign up user identifier information asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;SignUpUserIdInfo&gt;.</returns>
        public Task<SignUpUserIdInfo> GetSignUpUserIdInfoAsync(string cellphone)
        {
            return this.interService.GetSignUpUserIdInfoAsync(cellphone);
        }

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetUserInfoAsync(Guid userId)
        {
            return this.interService.GetUserInfoAsync(userId);
        }

        #endregion IUserInfoService Members
    }
}
