// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IProductService.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  2:21
// ***********************************************************************
// <copyright file="IProductService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        Task<RegularProductInfo> HitShelvesAsync(IssueRegularProduct command);

        /// <summary>
        ///     Hits the shelves.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task<JBYProductInfo> HitShelvesAsync(IssueJBYProduct command);

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