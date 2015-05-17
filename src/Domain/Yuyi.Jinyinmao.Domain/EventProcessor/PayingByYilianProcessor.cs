// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-15  8:15 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-16  1:50 AM
// ***********************************************************************
// <copyright file="PayingByYilianProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     PayingByYilianProcessor.
    /// </summary>
    public class PayingByYilianProcessor : EventProcessor<PayingByYilian>, IPayingByYilianProcessor
    {
        #region IPayingByYilianProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(PayingByYilian @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
                await DBSyncHelper.SyncSettleAccountTranscation(e.TranscationInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion IPayingByYilianProcessor Members
    }
}