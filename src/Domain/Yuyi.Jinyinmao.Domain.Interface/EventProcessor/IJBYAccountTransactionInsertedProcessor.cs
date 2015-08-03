// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IJBYAccountTransactionInsertedProcessor.cs
// Created          : 2015-08-03  11:26 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-03  11:27 AM
// ***********************************************************************
// <copyright file="IJBYAccountTransactionInsertedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Interface ISettleAccountTransactionInsertedProcessor
    /// </summary>
    public interface IJBYAccountTransactionInsertedProcessor : IGrain
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(JBYAccountTransactionInserted @event);
    }
}