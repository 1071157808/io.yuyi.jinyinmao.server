// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:36 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  11:38 PM
// ***********************************************************************
// <copyright file="IEventProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Interface IEventProcessor
    /// </summary>
    public interface IEventProcessor
    {
    }

    /// <summary>
    ///     Interface IEventProcessor
    /// </summary>
    /// <typeparam name="TEvent">The type of the t event.</typeparam>
    public interface IEventProcessor<in TEvent> where TEvent : IEvent
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(TEvent @event);
    }
}
