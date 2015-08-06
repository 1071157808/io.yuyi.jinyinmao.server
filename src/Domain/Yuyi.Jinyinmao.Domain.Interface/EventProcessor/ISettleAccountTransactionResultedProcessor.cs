// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ISettleAccountTransactionResultedProcessor.cs
// Created          : 2015-08-05  5:48 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-05  7:03 PM
// ***********************************************************************
// <copyright file="ISettleAccountTransactionResultedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Interface ISettleAccountTransactionResultedProcessor
    /// </summary>
    public interface ISettleAccountTransactionResultedProcessor : IGrain
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(SettleAccountTransactionResulted @event);
    }
}