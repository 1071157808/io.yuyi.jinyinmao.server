// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-18  11:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  11:05 PM
// ***********************************************************************
// <copyright file="IJBYWithdrawalAcceptedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Interface IJBYWithdrawalAcceptedProcessor
    /// </summary>
    public interface IJBYWithdrawalAcceptedProcessor : IGrain
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(JBYWithdrawalAccepted @event);
    }
}