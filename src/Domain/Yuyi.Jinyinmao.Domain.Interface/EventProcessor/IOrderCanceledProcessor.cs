// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IOrderCanceledProcessor.cs
// Created          : 2015-08-06  4:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-06  4:40 PM
// ***********************************************************************
// <copyright file="IOrderCanceledProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Interface IOrderTransferedProcessor
    /// </summary>
    public interface IOrderCanceledProcessor : IGrain
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(OrderCanceled @event);
    }
}