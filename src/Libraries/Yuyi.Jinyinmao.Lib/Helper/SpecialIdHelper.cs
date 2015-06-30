// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-23  10:41 PM
// 
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  6:23 PM
// ***********************************************************************
// <copyright file="SpecialIdHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Packages.Helper
{
    /// <summary>
    ///     SpecialIdHelper.
    /// </summary>
    public static class SpecialIdHelper
    {
        /// <summary>
        ///     Gets the j reinvesting by transaction product identifier.
        /// </summary>
        /// <value>The j reinvesting by transaction product identifier.</value>
        public static Guid ReinvestingJBYTransactionProductId { get; } = Guid.Parse("92CFADC4-91A5-4A09-8D0E-AC122C837F5B");

        /// <summary>
        ///     Gets the withdrawal jby transaction product identifier.
        /// </summary>
        /// <value>The withdrawal jby transaction product identifier.</value>
        public static Guid WithdrawalJBYTransactionProductId { get; } = Guid.Parse("92CFADC4-91A5-4A09-8D0E-AC122C837F5B");
    }
}