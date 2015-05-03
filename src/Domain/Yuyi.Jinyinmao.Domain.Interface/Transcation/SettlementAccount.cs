// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  5:47 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  1:32 AM
// ***********************************************************************
// <copyright file="SettlementAccount.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     SettlementAccount.
    /// </summary>
    public class SettlementAccount
    {
        /// <summary>
        ///     Gets or sets the transactions.
        /// </summary>
        /// <value>The transactions.</value>
        public List<Transcation> Transactions { get; set; }
    }
}