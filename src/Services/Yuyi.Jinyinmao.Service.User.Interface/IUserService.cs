// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  3:11 AM
// ***********************************************************************
// <copyright file="IUserService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Dtos;

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Interface IUserService
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        ///     Checks the cellphone asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;CheckCellphoneResult&gt;.</returns>
        Task<CheckCellphoneResult> CheckCellphoneAsync(string cellphone);

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
        ///     Gets the sign up user identifier information asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;SignUpUserIdInfo&gt;.</returns>
        Task<SignUpUserIdInfo> GetSignUpUserIdInfoAsync(string cellphone);

        /// <summary>
        ///     Registers the user asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> RegisterUserAsync(UserRegister command);

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
    }
}
