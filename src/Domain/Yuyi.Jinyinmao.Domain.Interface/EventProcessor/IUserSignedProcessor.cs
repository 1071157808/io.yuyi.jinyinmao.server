// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IUserSignedProcessor.cs
// Created          : 2015-08-17  20:08
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  20:11
// ***********************************************************************
// <copyright file="IUserSignedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Interface IUserSignedProcessor
    /// </summary>
    public interface IUserSignedProcessor : IGrainWithGuidKey
    {
        /// <summary>
        ///     Processes the event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        Task ProcessEventAsync(UserSigned @event);
    }
}