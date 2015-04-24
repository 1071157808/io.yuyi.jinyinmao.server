// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  3:12 AM
// ***********************************************************************
// <copyright file="IUser.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dto;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IUser
    /// </summary>
    public interface IUser : IGrain
    {
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
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        Task<UserInfo> GetUserInfoAsync();

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
        ///     Resets the login password.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task ResetLoginPasswordAsync(ResetLoginPassword command);

        /// <summary>
        ///     Sets the payment password asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task SetPaymentPasswordAsync(SetPaymentPassword command);
    }
}
