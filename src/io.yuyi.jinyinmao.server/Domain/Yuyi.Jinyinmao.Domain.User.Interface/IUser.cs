// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-01  2:50 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-02  12:12 AM
// ***********************************************************************
// <copyright file="IUser.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.User.Interface
{
    /// <summary>
    ///     Interface IUser
    /// </summary>
    public interface IUser : IGrain
    {
        /// <summary>
        ///     Registers the specified user register.
        /// </summary>
        /// <param name="userRegister">The user register.</param>
        /// <returns>Task.</returns>
        Task Register(UserRegister userRegister);
    }
}
