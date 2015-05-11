// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-10  11:58 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-12  12:10 AM
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
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     Interface IJBYProduct
    /// </summary>
    public interface IJBYProduct : IGrain
    {
        /// <summary>
        ///     Builds the jby transcation asynchronous.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Task&lt;Transcation&gt;.</returns>
        Task<Tuple<bool, Guid>> BuildJBYTranscationAsync(TranscationInfo info);

        /// <summary>
        /// Gets the agreement asynchronous.
        /// </summary>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GetAgreementAsync(int agreementIndex);

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
        Task HitShelvesAsync(IssueJBYProduct command);

        /// <summary>
        ///     Sets to sold out asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task SetToSoldOutAsync();

        /// <summary>
        ///     Updates the sale information asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task UpdateSaleInfoAsync(JBYProduct product);
    }
}