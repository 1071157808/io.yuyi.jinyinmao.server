// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYAccountTransactionResultedEventProcessor .cs
// Created          : 2015-08-05  6:00 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-05  7:06 PM
// ***********************************************************************
// <copyright file="JBYAccountTransactionResultedEventProcessor .cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYAccountTransactionResultedProcessor.
    /// </summary>
    public class JBYAccountTransactionResultedProcessor : EventProcessor<JBYAccountTransactionResulted>, IJBYAccountTransactionResultedProcessor
    {
        #region IJBYAccountTransactionResultedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(JBYAccountTransactionResulted @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncJBYAccountTransactionAsync(e.TransactionInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion IJBYAccountTransactionResultedProcessor Members
    }
}