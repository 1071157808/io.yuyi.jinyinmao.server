// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : UserSignedProcessor.cs
// Created          : 2015-08-17  20:11
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  20:12
// ***********************************************************************
// <copyright file="UserSignedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     UserSignedProcessor.
    /// </summary>
    public class UserSignedProcessor : EventProcessor<UserSigned>, IUserSignedProcessor
    {
        #region IUserSignedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(UserSigned @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncSettleAccountTransactionAsync(e.TransactionInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion IUserSignedProcessor Members
    }
}