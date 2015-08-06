// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  6:16 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  12:25 AM
// ***********************************************************************
// <copyright file="RegularProductIssuedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Service;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     RegularProductIssuedProcessor.
    /// </summary>
    public class RegularProductIssuedProcessor : EventProcessor<RegularProductIssued>, IRegularProductIssuedProcessor
    {
        private IProductInfoService productInfoService;

        #region IRegularProductIssuedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(RegularProductIssued @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncRegularProductAsync(e.ProductInfo, e.Agreement1, e.Agreement2));

            await this.ProcessingEventAsync(@event, async e =>
            {
                await this.productInfoService.GetProductInfoAsync(e.ProductInfo.ProductId);
                for (int i = 1; i < 20; i++)
                {
                    string agreement = await this.productInfoService.GetAgreementAsync(e.ProductInfo.ProductId, i);
                    if (agreement.IsNullOrEmpty())
                    {
                        break;
                    }
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IRegularProductIssuedProcessor Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.productInfoService = new ProductInfoService(new ProductService());

            return base.OnActivateAsync();
        }
    }
}