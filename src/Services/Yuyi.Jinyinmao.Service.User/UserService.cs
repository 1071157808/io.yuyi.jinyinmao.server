// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-07  3:02 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-10  5:56 PM
// ***********************************************************************
// <copyright file="UserService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Actor.Commands;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dto;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     Class UserService.
    /// </summary>
    public class UserService : CommandHandler, IUserService
    {
        #region IUserService Members

        /// <summary>
        ///     Excutes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;ICommandHanderResult&lt;TResult&gt;&gt;.</returns>
        public async Task<ICommandHanderResult<UserInfo>> ExcuteCommand(UserRegister command)
        {
            return await this.TryExcuteCommand(async c =>
            {
                IUser user = UserFactory.GetGrain(c.UserId);
                await user.RegisterAsync(c);
                return await user.GetUserInfoAsync();
            }, command);
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
