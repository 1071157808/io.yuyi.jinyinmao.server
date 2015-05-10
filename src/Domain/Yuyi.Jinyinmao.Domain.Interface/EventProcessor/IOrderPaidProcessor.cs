// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  5:21 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-09  3:55 PM
// ***********************************************************************
// <copyright file="IOrderBuiltProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     Interface IOrderBuiltProcessor
    /// </summary>
    public interface IOrderPaidProcessor : IGrain
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(OrderPaid @event);
    }
}