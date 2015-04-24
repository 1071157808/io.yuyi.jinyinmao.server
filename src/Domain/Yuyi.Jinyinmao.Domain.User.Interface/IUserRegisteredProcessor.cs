// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  10:30 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  10:35 PM
// ***********************************************************************
// <copyright file="IUserRegisteredProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IUserRegisteredProcessor
    /// </summary>
    public interface IUserRegisteredProcessor : IGrain
    {
        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(UserRegistered @event);
    }
}
