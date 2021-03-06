// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IJBYProduct.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-18  19:45
// ***********************************************************************
// <copyright file="IJBYProduct.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     Interface IJBYProduct
    /// </summary>
    public interface IJBYProduct : IGrainWithIntegerKey
    {
        /// <summary>
        ///     Builds the jby transaction asynchronous.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Task&lt;System.Nullable&lt;Guid&gt;&gt;.</returns>
        Task<Guid?> BuildJBYTransactionAsync(JBYAccountTransactionInfo info);

        /// <summary>
        ///     Checks the sale status asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task CheckSaleStatusAsync();

        /// <summary>
        ///     Dumps the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task DumpAsync();

        /// <summary>
        ///     Gets the agreement asynchronous.
        /// </summary>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GetAgreementAsync(int agreementIndex);

        /// <summary>
        ///     Gets the jby product paid amount asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int64&gt;.</returns>
        Task<long> GetJBYProductPaidAmountAsync();

        /// <summary>
        ///     Gets the product information asynchronous.
        /// </summary>
        /// <returns>Task&lt;JBYProductInfo&gt;.</returns>
        Task<JBYProductInfo> GetProductInfoAsync();

        /// <summary>
        ///     Hits the shelves asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task<JBYProductInfo> HitShelvesAsync(IssueJBYProduct command);

        /// <summary>
        ///     Refreshes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task<JBYProductInfo> RefreshAsync(bool force = false);

        /// <summary>
        ///     Reloads the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task ReloadAsync();

        /// <summary>
        ///     Sets to sold out asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task SetToSoldOutAsync();

        /// <summary>
        ///     Synchronizes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task SyncAsync();
    }
}