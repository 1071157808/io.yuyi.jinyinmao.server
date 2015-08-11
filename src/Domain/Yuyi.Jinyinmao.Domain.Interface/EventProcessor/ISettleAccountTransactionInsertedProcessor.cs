// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ISettleAccountTransactionInsertedProcessor.cs
// Created          : 2015-07-31  4:27 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-12  3:25 AM
// ***********************************************************************
// <copyright file="ISettleAccountTransactionInsertedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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
    public interface ISettleAccountTransactionInsertedProcessor : IGrainWithGuidKey
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(SettleAccountTransactionInserted @event);
    }
}