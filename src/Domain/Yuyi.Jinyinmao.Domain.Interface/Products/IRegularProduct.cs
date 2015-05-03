// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  12:26 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-28  12:28 PM
// ***********************************************************************
// <copyright file="IRegularProduct.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IRegularProduct
    /// </summary>
    public interface IRegularProduct : IEntity
    {
        /// <summary>
        /// Builds the order asynchronous.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <param name="transcationInfo">The transcation information.</param>
        /// <returns>Task&lt;OrderInfo&gt;.</returns>
        Task<OrderInfo> BuildOrderAsync(UserInfo userInfo, TranscationInfo transcationInfo);

        /// <summary>
        /// Gets the agreement asynchronous.
        /// </summary>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GetAgreementAsync(int agreementIndex);

        /// <summary>
        /// Gets the paid amount.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> GetPaidAmountAsync();

        /// <summary>
        /// Gets the product paid amount asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> GetProductPaidAmountAsync();

        /// <summary>
        /// Gets the regular product information asynchronous.
        /// </summary>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        Task<RegularProductInfo> GetRegularProductInfoAsync();

        /// <summary>
        ///     Hits the shelves.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task HitShelvesAsync(IssueRegularProduct command);
    }
}