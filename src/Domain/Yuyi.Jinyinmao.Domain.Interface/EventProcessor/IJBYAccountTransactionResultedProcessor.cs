// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IJBYAccountTransactionResultedProcessor.cs
// Created          : 2015-08-05  5:58 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-05  7:13 PM
// ***********************************************************************
// <copyright file="IJBYAccountTransactionResultedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Interface IJBYAccountTransactionResultedProcessor
    /// </summary>
    public interface IJBYAccountTransactionResultedProcessor : IGrain
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(JBYAccountTransactionResulted @event);
    }
}