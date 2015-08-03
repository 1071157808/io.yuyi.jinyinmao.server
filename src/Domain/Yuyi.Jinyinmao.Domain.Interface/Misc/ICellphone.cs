// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ICellphone.cs
// Created          : 2015-04-28  11:26 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-03  5:39 PM
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
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        Task Register(Guid userId);

        /// <summary>
        ///     Unregisters this instance.
        /// </summary>
        /// <returns>Task.</returns>
        Task Unregister();
    }
}