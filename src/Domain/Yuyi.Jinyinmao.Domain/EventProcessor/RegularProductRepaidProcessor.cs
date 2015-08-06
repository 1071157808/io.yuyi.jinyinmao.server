// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-18  12:23 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  12:26 AM
// ***********************************************************************
// <copyright file="RegularProductRepaidProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     RegularProductRepaidProcessor.
    /// </summary>
    public class RegularProductRepaidProcessor : EventProcessor<RegularProductRepaid>, IRegularProductRepaidProcessor
    {
        #region IRegularProductRepaidProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(RegularProductRepaid @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncRegularProductAsync(e.ProductInfo, e.Agreement1, e.Agreement2));

            await base.ProcessEventAsync(@event);
        }

        #endregion IRegularProductRepaidProcessor Members
    }
}