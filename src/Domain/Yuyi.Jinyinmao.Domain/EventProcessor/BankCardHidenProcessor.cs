// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-21  4:26 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-21  5:19 PM
// ***********************************************************************
// <copyright file="BankCardHidenProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     BankCardHidenProcessor.
    /// </summary>
    public class BankCardHidenProcessor : EventProcessor<BankCardHiden>, IBankCardHidenProcessor
    {
        #region IBankCardHidenProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(BankCardHiden @event)
        {
            string userIdentifier = @event.UserInfo.UserId.ToGuidString();

            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncBankCard(e.BankCardInfo, userIdentifier));

            await base.ProcessEventAsync(@event);
        }

        #endregion IBankCardHidenProcessor Members
    }
}