// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:26 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-29  12:11 PM
// ***********************************************************************
// <copyright file="ICellphone.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface ICellphone
    /// </summary>
    public interface ICellphone : IGrain
    {
        /// <summary>
        ///     Gets the cellphone information.
        /// </summary>
        /// <returns>Task&lt;CellphoneInfo&gt;.</returns>
        Task<CellphoneInfo> GetCellphoneInfoAsync();

        /// <summary>
        ///     Registers this instance.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        Task Register();

        /// <summary>
        ///     Registers this instance.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        Task Register(Guid userId);

        /// <summary>
        ///     Resets the user identifier.
        /// </summary>
        /// <returns>Task.</returns>
        Task ResetUserIdentifier();

        /// <summary>
        ///     Unregisters this instance.
        /// </summary>
        /// <returns>Task.</returns>
        Task Unregister();
    }
}