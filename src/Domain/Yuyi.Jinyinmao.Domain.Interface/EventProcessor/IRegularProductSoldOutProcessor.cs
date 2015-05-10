// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-09  1:25 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-09  1:27 AM
// ***********************************************************************
// <copyright file="IRegularProductSoldOutProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     Interface IRegularProductSoldOutProcessor
    /// </summary>
    public interface IRegularProductSoldOutProcessor : IGrain
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(RegularProductSoldOut @event);
    }
}