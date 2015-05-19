// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-19  12:03 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  12:05 AM
// ***********************************************************************
// <copyright file="JBYWithdrawalResultedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYWithdrawalResultedProcessor.
    /// </summary>
    public class JBYWithdrawalResultedProcessor : EventProcessor<JBYWithdrawalResulted>, IJBYWithdrawalResultedProcessor
    {
        #region IJBYWithdrawalResultedProcessor Members

        /// <summary>
        ///     process event as an asynchronous operation.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(JBYWithdrawalResulted @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                await DBSyncHelper.SyncJBYAccountTranscation(e.JBYAccountTranscationInfo);

                await DBSyncHelper.SyncSettleAccountTranscation(e.SettleAccountTranscationInfo);
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IJBYWithdrawalResultedProcessor Members
    }
}