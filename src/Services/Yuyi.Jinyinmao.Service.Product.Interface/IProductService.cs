// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  10:57 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  1:48 AM
// ***********************************************************************
// <copyright file="IProductService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Commands;

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Interface IProductService
    /// </summary>
    public interface IProductService : IProductInfoService
    {
        /// <summary>
        ///     Hits the shelves.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task HitShelvesAsync(IssueRegularProduct command);

        /// <summary>
        ///     Hits the shelves.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task HitShelvesAsync(IssueJBYProduct command);

        /// <summary>
        /// Refreshes the jyb product asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task RefreshJybProductAsync();

        /// <summary>
        /// Reloads the jby product asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task ReloadJBYProductAsync();

        /// <summary>
        /// Reloads the regular product asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task.</returns>
        Task ReloadRegularProductAsync(Guid productId);

        /// <summary>
        ///     Repays the asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task.</returns>
        Task RepayRegularProductAsync(Guid productId);

        /// <summary>
        /// Sets the current jby product to sold out asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task SetCurrentJBYProductToSoldOutAsync();

        /// <summary>
        /// Sets the regular product to sold out asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task.</returns>
        Task SetRegularProductToSoldOutAsync(Guid productId);
    }
}