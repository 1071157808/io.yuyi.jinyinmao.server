// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IProductService.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  0:28
// ***********************************************************************
// <copyright file="IProductService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using PostSharp.Patterns.Contracts;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Log;

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Interface IProductService
    /// </summary>
    public interface IProductService : IProductInfoService
    {
        /// <summary>
        ///     Cancels the order asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        Task<OrderInfo> CancelOrderAsync(CancelOrder command);

        /// <summary>
        ///     Hits the shelves.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        [LogExceptionAspect]
        Task<RegularProductInfo> HitShelvesAsync([Required] IssueRegularProduct command);

        /// <summary>
        ///     Hits the shelves.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        [LogExceptionAspect]
        Task<JBYProductInfo> HitShelvesAsync([Required] IssueJBYProduct command);

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
        Task<JBYProductInfo> ReloadJBYProductAsync();

        /// <summary>
        ///     Reloads the regular product asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task.</returns>
        Task<RegularProductInfo> ReloadRegularProductAsync(Guid productId);

        /// <summary>
        ///     Repays the regular product asynchronous.
        /// </summary>
        /// <param name="productRepayCommand">The product repay command.</param>
        /// <returns>
        ///     Task.
        /// </returns>
        Task<RegularProductInfo> RepayRegularProductAsync(ProductRepay productRepayCommand);

        /// <summary>
        ///     Sets the current jby product to sold out asynchronous.
        /// </summary>
        /// <returns>
        ///     Task.
        /// </returns>
        Task<JBYProductInfo> SetCurrentJBYProductToSoldOutAsync();

        /// <summary>
        ///     Sets the regular product to sold out asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task.</returns>
        Task<RegularProductInfo> SetRegularProductToSoldOutAsync(Guid productId);
    }
}