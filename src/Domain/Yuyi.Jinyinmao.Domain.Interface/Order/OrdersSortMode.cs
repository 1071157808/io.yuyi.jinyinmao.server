// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-08  9:47 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-08  10:54 PM
// ***********************************************************************
// <copyright file="OrdersSortMode.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Enum OrdersSortMode
    /// </summary>
    public enum OrdersSortMode
    {
        /// <summary>
        ///     The by order time
        /// </summary>
        ByOrderTimeDesc = 1,

        /// <summary>
        ///     The by settle date
        /// </summary>
        BySettleDateAsc = 2
    }
}