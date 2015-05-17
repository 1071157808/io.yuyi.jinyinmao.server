// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-15  2:20 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  12:21 AM
// ***********************************************************************
// <copyright file="BankCardAddedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     BankCardAddedProcessor.
    /// </summary>
    public class BankCardAddedProcessor : EventProcessor<BankCardAdded>, IBankCardAddedProcessor
    {
        #region IBankCardAddedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(BankCardAdded @event)
        {
            string userIdentifier = @event.UserInfo.UserId.ToGuidString();

            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncBankCard(e.BankCardInfo, userIdentifier));

            await base.ProcessEventAsync(@event);
        }

        #endregion IBankCardAddedProcessor Members
    }
}