// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IProductService.cs
// Created          : 2015-04-28  10:57 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-11  6:59 PM
// ***********************************************************************
// <copyright file="IProductService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;

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
        Task<JBYProductInfo> HitShelvesAsync(IssueJBYProduct command);

        /// <summary>
        ///     Migrates the asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="migrationDto">The migration dto.</param>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        Task<RegularProductInfo> MigrateAsync(Guid productId, RegularProductMigrationDto migrationDto);

        /// <summary>
        ///     Refreshes the jyb product asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task RefreshJybProductAsync();

        /// <summary>
        ///     Reloads the jby product asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task ReloadJBYProductAsync();

        /// <summary>
        ///     Reloads the regular product asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task.</returns>
        Task ReloadRegularProductAsync(Guid productId);

        /// <summary>
        ///     Repays the asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>Task.</returns>
        Task RepayRegularProductAsync(Guid productId, Dictionary<string, object> args);

        /// <summary>
        ///     Sets the current jby product to sold out asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task SetCurrentJBYProductToSoldOutAsync();

        /// <summary>
        ///     Sets the regular product to sold out asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task.</returns>
        Task SetRegularProductToSoldOutAsync(Guid productId);
    }
}