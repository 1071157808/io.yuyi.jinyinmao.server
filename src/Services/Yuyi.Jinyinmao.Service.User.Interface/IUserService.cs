// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  4:15 PM
// ***********************************************************************
// <copyright file="IUserService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Interface IUserService
    /// </summary>
    public interface IUserService : IUserInfoService
    {
        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task AddBankCardAsync(AddBankCard command);

        /// <summary>
        ///     Authenticatings the asynchronous.
        /// </summary>
        /// <param name="command">The apply for authentication.</param>
        /// <returns>Task.</returns>
        Task AuthenticateAsync(Authenticate command);

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
        ///     Sets the default bank card asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task.</returns>
        Task SetDefaultBankCardAsync(Guid userId, string bankCardNo);

        /// <summary>
        ///     Sets the payment password asynchronous.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Task.</returns>
        Task SetPaymentPasswordAsync(SetPaymentPassword command);
    }
}
