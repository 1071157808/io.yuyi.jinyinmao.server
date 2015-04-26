// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  1:36 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  12:32 AM
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
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(UserRegistered @event);
    }
}
