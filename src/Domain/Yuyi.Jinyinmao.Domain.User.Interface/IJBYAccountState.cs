// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-22  12:58 AM
// ***********************************************************************
// <copyright file="IJBYAccountState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Orleans;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IJBYAccountState
    /// </summary>
    public interface IJBYAccountState : IGrainState
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        Guid Id { get; set; }

        /// <summary>
        ///     利率，以万分之一为单位，从2015年1月1日算起
        /// </summary>
        /// <value>The interest rate.</value>
        IList<int> InterestRate { get; set; }

        /// <summary>
        ///     Gets or sets the transactions.
        /// </summary>
        /// <value>The transactions.</value>
        IList<TransactionInfo> Transactions { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        Guid UserId { get; set; }
    }
}
