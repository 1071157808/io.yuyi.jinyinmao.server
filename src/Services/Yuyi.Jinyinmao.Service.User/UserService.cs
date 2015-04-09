// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-07  3:02 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-07  10:51 AM
// ***********************************************************************
// <copyright file="UserService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Actor.Commands;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
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
        ///     Excutes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;ICommandHanderResult&lt;TResult&gt;&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<ICommandHanderResult<UserInfo>> ExcuteCommand(UserRegister command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets the sign up user identifier information asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;SignUpUserIdInfo&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<SignUpUserIdInfo> GetSignUpUserIdInfoAsync(string cellphone)
        {
        }

        /// <summary>
        ///     Sign in asynchronous.
        /// </summary>
        /// <param name="loginName">The login name.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;SignInResult&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<SignInResult> SignInAsync(string loginName, string password)
        {
            throw new NotImplementedException();
        }

        #endregion IUserService Members
    }
}
