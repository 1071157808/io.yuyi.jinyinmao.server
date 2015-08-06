// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IJBYTransactionTransferedProcessor.cs
// Created          : 2015-08-06  12:44 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-06  12:46 AM
// ***********************************************************************
// <copyright file="IJBYTransactionTransferedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Interface IJBYTransactionTransferedProcessor
    /// </summary>
    public interface IJBYTransactionTransferedProcessor : IGrain
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(JBYTransactionTransfered @event);
    }
}