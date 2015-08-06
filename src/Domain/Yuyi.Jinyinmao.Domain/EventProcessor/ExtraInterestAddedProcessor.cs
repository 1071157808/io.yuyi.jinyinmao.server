// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-06-05  1:34 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-05  1:35 AM
// ***********************************************************************
// <copyright file="ExtraInterestAddedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     ExtraInterestAddedProcessor.
    /// </summary>
    public class ExtraInterestAddedProcessor : EventProcessor<ExtraInterestAdded>, IExtraInterestAddedProcessor
    {
        #region IExtraInterestAddedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(ExtraInterestAdded @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncOrderAsync(e.OrderInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion IExtraInterestAddedProcessor Members
    }
}