// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-07  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-07  10:40 AM
// ***********************************************************************
// <copyright file="ICellphone.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Dto;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface ICellphone
    /// </summary>
    public interface ICellphone : IGrain
    {
        /// <summary>
        /// Gets the cellphone information.
        /// </summary>
        /// <returns>Task&lt;CellphoneInfo&gt;.</returns>
        Task<CellphoneInfo> GetCellphoneInfo();
    }
}
