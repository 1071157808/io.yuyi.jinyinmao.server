// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  2:51 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-23  1:31 PM
// ***********************************************************************
// <copyright file="JBYProductIssuedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYProductIssuedProcessor.
    /// </summary>
    public class JBYProductIssuedProcessor : EventProcessor<JBYProductIssued>, IJBYProductIssuedProcessor
    {
        #region IJBYProductIssuedProcessor Members

        /// <summary>
        ///     process event as an asynchronous operation.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(JBYProductIssued @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncJBYProductAsync(e.ProductInfo, e.Agreement1, e.Agreement2));

            await base.ProcessEventAsync(@event);
        }

        #endregion IJBYProductIssuedProcessor Members
    }
}