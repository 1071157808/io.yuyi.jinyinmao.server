// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  12:56 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  10:45 PM
// ***********************************************************************
// <copyright file="IJBYProductWithdrawalManagerState.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     Interface IJBYProductWithdrawalManagerState
    /// </summary>
    public interface IJBYProductWithdrawalManagerState : IEntityState
    {
        /// <summary>
        ///     Gets or sets the withdrawal transactions.
        /// </summary>
        /// <value>The withdrawal transactions.</value>
        Dictionary<Guid, JBYAccountTransactionInfo> WithdrawalTransactions { get; set; }
    }
}