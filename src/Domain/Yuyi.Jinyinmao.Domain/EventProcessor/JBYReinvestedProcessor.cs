// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-19  1:05 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  1:07 AM
// ***********************************************************************
// <copyright file="JBYReinvestedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYReinvestedProcessor.
    /// </summary>
    public class JBYReinvestedProcessor : EventProcessor<JBYReinvested>, IJBYReinvestedProcessor
    {
        #region IJBYReinvestedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(JBYReinvested @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncJBYAccountTranscation(e.TranscationInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion IJBYReinvestedProcessor Members
    }
}