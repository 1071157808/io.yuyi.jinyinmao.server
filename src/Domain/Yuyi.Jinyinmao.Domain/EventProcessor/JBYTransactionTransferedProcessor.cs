// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYTransactionTransferedProcessor.cs
// Created          : 2015-08-06  12:45 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-06  12:50 AM
// ***********************************************************************
// <copyright file="JBYTransactionTransferedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     IjbyTransactionTransferedProcessor.
    /// </summary>
    public class JBYTransactionTransferedProcessor : EventProcessor<JBYTransactionTransfered>, IJBYTransactionTransferedProcessor
    {
        #region IJBYTransactionTransferedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(JBYTransactionTransfered @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                await DBSyncHelper.RemoveSettleAccountTransactionAsync(e.TransactionInfo);
                await DBSyncHelper.RemoveJBYAccountTransactionAsync(e.JBYInfo);
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IJBYTransactionTransferedProcessor Members
    }
}