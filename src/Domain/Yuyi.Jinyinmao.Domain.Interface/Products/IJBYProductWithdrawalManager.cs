// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  12:55 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  12:30 AM
// ***********************************************************************
// <copyright file="IJBYProductWithdrawalManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     Interface IJBYProductWithdrawalManager
    /// </summary>
    public interface IJBYProductWithdrawalManager : IGrain
    {
        /// <summary>
        ///     Builds the withdrawal transcation asynchronous.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> BuildWithdrawalTranscationAsync(JBYAccountTranscationInfo info);
    }
}