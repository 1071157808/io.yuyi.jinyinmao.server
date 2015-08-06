// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYAccountTransactionCanceledProcessor.cs
// Created          : 2015-08-05  10:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-05  10:32 PM
// ***********************************************************************
// <copyright file="JBYAccountTransactionCanceledProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYAccountTransactionCanceledProcessor.
    /// </summary>
    public class JBYAccountTransactionCanceledProcessor : EventProcessor<JBYAccountTransactionCanceled>, IJBYAccountTransactionCanceledProcessor
    {
        #region IJBYAccountTransactionCanceledProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(JBYAccountTransactionCanceled @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.RemoveJBYAccountTransactionAsync(e.TransactionInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion IJBYAccountTransactionCanceledProcessor Members
    }
}