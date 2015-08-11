// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IJBYAccountTransactionCanceledProcessor.cs
// Created          : 2015-08-05  10:20 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-12  3:26 AM
// ***********************************************************************
// <copyright file="IJBYAccountTransactionCanceledProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Interface IJBYAccountTransactionCanceledProcessor
    /// </summary>
    public interface IJBYAccountTransactionCanceledProcessor : IGrainWithGuidKey
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(JBYAccountTransactionCanceled @event);
    }
}