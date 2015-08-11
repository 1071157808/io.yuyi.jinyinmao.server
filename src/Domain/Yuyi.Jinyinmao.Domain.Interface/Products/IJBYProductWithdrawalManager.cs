// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IJBYProductWithdrawalManager.cs
// Created          : 2015-05-27  7:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-12  2:44 AM
// ***********************************************************************
// <copyright file="IJBYProductWithdrawalManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     Interface IJBYProductWithdrawalManager
    /// </summary>
    public interface IJBYProductWithdrawalManager : IGrainWithIntegerKey
    {
        /// <summary>
        ///     Builds the withdrawal transaction asynchronous.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<DateTime?> BuildWithdrawalTransactionAsync(JBYAccountTransactionInfo info);
    }
}