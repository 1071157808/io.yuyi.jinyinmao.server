// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  2:32 AM
// ***********************************************************************
// <copyright file="UserService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dto;
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
        /// Checks the password asynchronous.
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
        ///     Sets the payment password asynchronous.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Task.</returns>
        public Task SetPaymentPasswordAsync(SetPaymentPassword command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.SetPaymentPasswordAsync(command);
        }

        #endregion IUserService Members
    }
}
