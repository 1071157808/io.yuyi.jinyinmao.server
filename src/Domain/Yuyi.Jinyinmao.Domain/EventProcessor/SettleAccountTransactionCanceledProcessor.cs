// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SettleAccountTransactionCanceledProcessor.cs
// Created          : 2015-08-05  10:21 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-05  10:31 PM
// ***********************************************************************
// <copyright file="SettleAccountTransactionCanceledProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     SettleAccountTransactionCanceledProcessor.
    /// </summary>
    public class SettleAccountTransactionCanceledProcessor : EventProcessor<SettleAccountTransactionCanceled>, ISettleAccountTransactionCanceledProcessor
    {
        #region ISettleAccountTransactionCanceledProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(SettleAccountTransactionCanceled @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.RemoveSettleAccountTransactionAsync(e.TransactionInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion ISettleAccountTransactionCanceledProcessor Members
    }
}