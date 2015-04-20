// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-20  11:27 PM
// ***********************************************************************
// <copyright file="IUserService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Moe.Actor.Interface.Commands;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Dtos;

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Interface IUserService
    /// </summary>
    public interface IUserService : ICommandHandler<UserRegister, UserInfo>
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
        /// <param name="loginName">The login name.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;SignInResult&gt;.</returns>
        Task<SignInResult> CheckPasswordAsync(string loginName, string password);

        /// <summary>
        ///     Gets the sign up user identifier information asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;SignUpUserIdInfo&gt;.</returns>
        Task<SignUpUserIdInfo> GetSignUpUserIdInfoAsync(string cellphone);
    }
}
