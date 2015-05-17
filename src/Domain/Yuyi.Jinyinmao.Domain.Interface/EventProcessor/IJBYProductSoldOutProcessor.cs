// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  4:44 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  12:19 AM
// ***********************************************************************
// <copyright file="IJBYProductSoldOutProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Interface IJBYProductSoldOutProcessor
    /// </summary>
    public interface IJBYProductSoldOutProcessor : IGrain
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(JBYProductSoldOut @event);
    }
}