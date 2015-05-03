// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  6:02 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-03  6:02 PM
// ***********************************************************************
// <copyright file="JBYAccount.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     JBYAccount.
    /// </summary>
    public class JBYAccount
    {
        /// <summary>
        ///     Gets or sets the transactions.
        /// </summary>
        /// <value>The transactions.</value>
        public List<Transcation> Transactions { get; set; }
    }
}