// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:50 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  12:19 AM
// ***********************************************************************
// <copyright file="IPaymentPasswordSetProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Interface IPaymentPasswordSet
    /// </summary>
    public interface IPaymentPasswordSetProcessor : IGrainWithGuidKey
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(PaymentPasswordSet @event);
    }
}