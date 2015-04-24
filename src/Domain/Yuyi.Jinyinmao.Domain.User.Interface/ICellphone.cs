// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  10:22 PM
// ***********************************************************************
// <copyright file="ICellphone.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

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
    }
}
