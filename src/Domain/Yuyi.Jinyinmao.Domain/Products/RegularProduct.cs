// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-15  12:42 PM
// ***********************************************************************
// <copyright file="RegularProduct.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Moe.Lib;
using Orleans;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     RegularProduct.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public class RegularProduct : EntityGrain<IRegularProductState>, IRegularProduct
    {
        /// <summary>
        ///     The event processing
        /// </summary>
        private static readonly Dictionary<Type, Func<IEvent, Task>> EventProcessing = new Dictionary<Type, Func<IEvent, Task>>
        {
            { typeof(RegularProductSoldOut), e => RegularProductSoldOutProcessorFactory.GetGrain(e.EventId).ProcessEventAsync((RegularProductSoldOut)e) },
            { typeof(RegularProductIssued), e => RegularProductIssuedProcessorFactory.GetGrain(e.EventId).ProcessEventAsync((RegularProductIssued)e) },
            { typeof(RegularProductRepaid), e => RegularProductRepaidProcessorFactory.GetGrain(e.EventId).ProcessEventAsync((RegularProductRepaid)e) }
        };

        private long PaidAmount { get; set; }

        private IEnumerable<OrderInfo> PaidOrders
        {
            get { return this.State.Orders.Values.Where(o => o.ResultCode > 0).ToList(); }
        }

        #region IRegularProduct Members

        /// <summary>
        ///     build order as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;OrderInfo&gt;.</returns>
        public async Task<OrderInfo> BuildOrderAsync(OrderInfo orderInfo)
        {
            if (this.State.SoldOut)
            {
                return null;
            }

            if (this.State.StartSellTime > DateTime.UtcNow.AddHours(8))
            {
                return null;
            }

            if (orderInfo.Principal > this.State.FinancingSumAmount - this.PaidAmount)
            {
                return null;
            }

            OrderInfo order;
            if (this.State.Orders.TryGetValue(orderInfo.OrderId, out order))
            {
                return order;
            }

            DateTime valueDate = this.BuildValueDate();
            long interest = this.BuildInterest(valueDate, orderInfo.Principal);

            order = new OrderInfo
            {
                AccountTransactionId = orderInfo.AccountTransactionId,
                Args = orderInfo.Args,
                Cellphone = orderInfo.Cellphone,
                ExtraInterest = 0,
                ExtraYield = 0,
                Interest = interest,
                IsRepaid = orderInfo.IsRepaid,
                OrderId = orderInfo.OrderId,
                OrderNo = orderInfo.OrderNo,
                OrderTime = orderInfo.OrderTime,
                Principal = orderInfo.Principal,
                ProductCategory = orderInfo.ProductCategory,
                ProductId = orderInfo.ProductId,
                ProductSnapshot = await this.GetRegularProductInfoAsync(),
                RepaidTime = orderInfo.RepaidTime,
                ResultCode = orderInfo.ResultCode,
                ResultTime = orderInfo.ResultTime,
                SettleDate = this.State.SettleDate.Date,
                TransDesc = orderInfo.TransDesc,
                UserId = orderInfo.UserId,
                UserInfo = orderInfo.UserInfo,
                ValueDate = valueDate,
                Yield = this.State.Yield
            };

            this.State.Orders.Add(order.OrderId, order);

            await this.SaveStateAsync();

            this.ReloadOrderData();

            await this.CheckSaleStatusAsync();

            return order;
        }

        /// <summary>
        ///     Cancels the order asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>Task&lt;OrderInfo&gt;.</returns>
        public Task<OrderInfo> CancelOrderAsync(Guid orderId)
        {
            OrderInfo order;
            if (this.State.Orders.TryGetValue(orderId, out order))
            {
                this.State.Orders.Remove(orderId);
                this.ReloadOrderData();
                return Task.FromResult(order);
            }

            return Task.FromResult<OrderInfo>(null);
        }

        /// <summary>
        ///     Checks the sale status asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public Task CheckSaleStatusAsync()
        {
            if (this.PaidAmount >= this.State.FinancingSumAmount && !this.State.SoldOut)
            {
                Task.Factory.StartNew(() => this.SetToSoldOutAsync());
            }

            return TaskDone.Done;
        }

        /// <summary>
        ///     Gets the agreement asynchronous.
        /// </summary>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public Task<string> GetAgreementAsync(int agreementIndex)
        {
            switch (agreementIndex)
            {
                case 1:
                    return Task.FromResult(this.State.Agreement1);

                case 2:
                    return Task.FromResult(this.State.Agreement2);

                default:
                    return Task.FromResult(string.Empty);
            }
        }

        /// <summary>
        ///     Gets the product paid amount asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public Task<long> GetProductPaidAmountAsync()
        {
            return Task.FromResult(this.PaidAmount);
        }

        /// <summary>
        ///     Gets the regular product information asynchronous.
        /// </summary>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        public Task<RegularProductInfo> GetRegularProductInfoAsync()
        {
            if (this.State.ProductNo.IsNullOrEmpty())
            {
                return Task.FromResult<RegularProductInfo>(null);
            }

            RegularProductInfo info = new RegularProductInfo
            {
                Args = this.State.Args,
                BankName = this.State.BankName,
                Drawee = this.State.Drawee,
                DraweeInfo = this.State.DraweeInfo,
                EndorseImageLink = this.State.EndorseImageLink,
                EndSellTime = this.State.EndSellTime,
                EnterpriseInfo = this.State.EnterpriseInfo,
                EnterpriseLicense = this.State.EnterpriseLicense,
                EnterpriseName = this.State.EnterpriseName,
                FinancingSumAmount = this.State.FinancingSumAmount,
                IssueNo = this.State.IssueNo,
                IssueTime = this.State.IssueTime,
                PaidAmount = this.PaidAmount,
                Period = this.State.Period,
                PledgeNo = this.State.PledgeNo,
                ProductCategory = this.State.ProductCategory,
                ProductId = this.State.Id,
                ProductName = this.State.ProductName,
                ProductNo = this.State.ProductNo,
                Repaid = this.State.Repaid,
                RepaidTime = this.State.RepaidTime,
                RepaymentDeadline = this.State.RepaymentDeadline,
                RiskManagement = this.State.RiskManagement,
                RiskManagementInfo = this.State.RiskManagementInfo,
                RiskManagementMode = this.State.RiskManagementMode,
                SettleDate = this.State.SettleDate,
                SoldOut = this.State.SoldOut,
                SoldOutTime = this.State.SoldOutTime,
                StartSellTime = this.State.StartSellTime,
                UnitPrice = this.State.UnitPrice,
                Usage = this.State.Usage,
                ValueDate = this.State.ValueDate,
                ValueDateMode = this.State.ValueDateMode,
                Yield = this.State.Yield
            };

            return Task.FromResult(info);
        }

        /// <summary>
        ///     Hits the shelves.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task HitShelvesAsync(IssueRegularProduct command)
        {
            if (this.State.Id == command.ProductId)
            {
                return;
            }

            if (this.State.Id != Guid.Empty)
            {
                this.GetLogger().Warn(1, "Conflict product id: UserId {0}, RegularProductHitShelvesCommand.ProductId {1}", this.State.Id, command.ProductId);
                return;
            }

            this.BeginProcessCommandAsync(command);

            DateTime now = DateTime.UtcNow.AddHours(8);
            this.State.Id = command.ProductId;
            this.State.Agreement1 = command.Agreement1;
            this.State.Agreement2 = command.Agreement2;
            this.State.Args = command.Args;
            this.State.BankName = command.BankName;
            this.State.Drawee = command.Drawee;
            this.State.DraweeInfo = command.DraweeInfo;
            this.State.EndSellTime = command.EndSellTime;
            this.State.EnterpriseInfo = command.EnterpriseInfo;
            this.State.EnterpriseLicense = command.EnterpriseLicense;
            this.State.EnterpriseName = command.EnterpriseName;
            this.State.FinancingSumAmount = command.FinancingSumCount;
            this.State.IssueNo = command.IssueNo;
            this.State.IssueTime = now;
            this.State.Orders = new Dictionary<Guid, OrderInfo>();
            this.State.Period = command.Period;
            this.State.PledgeNo = command.PledgeNo;
            this.State.ProductCategory = command.ProductCategory;
            this.State.ProductName = command.ProductName;
            this.State.ProductNo = command.ProductNo;
            this.State.Repaid = false;
            this.State.RepaidTime = null;
            this.State.RepaymentDeadline = command.RepaymentDeadline;
            this.State.RiskManagement = command.RiskManagement;
            this.State.RiskManagementInfo = command.RiskManagementInfo;
            this.State.RiskManagementMode = command.RiskManagementMode;
            this.State.SettleDate = command.SettleDate;
            this.State.SoldOut = false;
            this.State.SoldOutTime = null;
            this.State.StartSellTime = command.StartSellTime;
            this.State.UnitPrice = command.UnitPrice;
            this.State.Usage = command.Usage;
            this.State.ValueDate = command.ValueDate;
            this.State.ValueDateMode = command.ValueDateMode;
            this.State.Yield = command.Yield;

            Stream stream = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    stream = await client.GetStreamAsync(command.EndorseImageLink);
                }

                CloudBlockBlob blob = SiloClusterConfig.PublicFileContainer.GetBlockBlobReference("EndorseImages/" + command.ProductId.ToGuidString());
                blob.Properties.ContentType = "image/jpeg";
                await blob.UploadFromStreamAsync(stream);
                this.State.EndorseImageLink = blob.Uri.AbsoluteUri;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

            await this.SaveStateAsync();

            await this.RaiseRegularProductIssuedEvent();
        }

        /// <summary>
        ///     Reload state data as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public override async Task ReloadAsync()
        {
            await this.State.ReadStateAsync();
            this.ReloadOrderData();
            await this.SyncAsync();
        }

        /// <summary>
        ///     Repays the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task RepayAsync()
        {
            if (!this.State.SoldOut)
            {
                return;
            }

            if (this.State.Repaid && this.State.RepaidTime.HasValue)
            {
                return;
            }

            DateTime now = DateTime.UtcNow.AddHours(8);

            foreach (OrderInfo order in this.PaidOrders)
            {
                await UserFactory.GetGrain(order.UserId).RepayOrderAsync(order.OrderId, now);
                order.IsRepaid = true;
                order.RepaidTime = order.RepaidTime.HasValue ? order.RepaidTime : now;
            }

            this.State.Repaid = true;
            this.State.RepaidTime = this.State.RepaidTime.HasValue ? this.State.RepaidTime : now;

            await this.SaveStateAsync();

            await this.RaiseRegularProductRepaidEvent();
        }

        /// <summary>
        ///     Sets to on sale asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SetToOnSaleAsync()
        {
            if (this.State.SoldOut)
            {
                this.State.SoldOut = false;
                this.State.SoldOutTime = null;

                await this.SaveStateAsync();
            }
        }

        /// <summary>
        ///     set to sold out as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SetToSoldOutAsync()
        {
            if (this.State.SoldOut && this.State.SoldOutTime.HasValue)
            {
                return;
            }

            this.State.SoldOut = true;
            this.State.SoldOutTime = DateTime.UtcNow.AddHours(8);

            await this.SaveStateAsync();

            await this.RaiseRegularProductSoldOutEvent();
        }

        /// <summary>
        ///     Synchronizes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SyncAsync()
        {
            await DBSyncHelper.SyncRegularProduct(await this.GetRegularProductInfoAsync(), this.State.Agreement1, this.State.Agreement2);
        }

        #endregion IRegularProduct Members

        //        /// <summary>
        //        /// migrate as an asynchronous operation.
        //        /// </summary>
        //        /// <param name="migrationDto">The migration dto.</param>
        //        /// <returns>Task.</returns>
        //        public async Task MigrateAsync(RegularProductMigrationDto migrationDto)
        //        {
        //            this.State.Agreement1 = migrationDto.Agreement1;
        //            this.State.Agreement2 = migrationDto.Agreement2;
        //            this.State.Args = migrationDto.Args;
        //            this.State.BankName = migrationDto.BankName;
        //            this.State.Drawee = migrationDto.Drawee;
        //            this.State.DraweeInfo = migrationDto.DraweeInfo;
        //            this.State.EndSellTime = migrationDto.EndSellTime;
        //            this.State.EndorseImageLink = migrationDto.EndorseImageLink;
        //            this.State.EnterpriseInfo = migrationDto.EnterpriseInfo;
        //            this.State.EnterpriseLicense = migrationDto.EnterpriseLicense;
        //            this.State.EnterpriseName = migrationDto.EnterpriseName;
        //            this.State.FinancingSumAmount = migrationDto
        //        }

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.ReloadOrderData();

            this.RegisterTimer(o => this.CheckSaleStatusAsync(), new object(), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(13));

            return base.OnActivateAsync();
        }

        private long BuildInterest(DateTime valueDate, long principal)
        {
            int dayCount = (this.State.SettleDate.Date.AddHours(1) - valueDate.Date).Days;
            return principal * this.State.Yield * dayCount / 3600000;
        }

        private DateTime BuildValueDate()
        {
            return this.State.ValueDateMode == null ?
                this.State.ValueDate.GetValueOrDefault(DateTime.UtcNow.AddHours(8).Date)
                : DateTime.UtcNow.AddHours(8).AddDays(this.State.ValueDateMode.GetValueOrDefault(0)).Date;
        }

        /// <summary>
        ///     process event as an asynchronous operation.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        private async Task ProcessEventAsync(Event @event)
        {
            @event.SourceId = this.State.Id.ToGuidString();
            @event.SourceType = this.GetType().Name;
            @event.TimeStamp = DateTime.UtcNow;

            this.StoreEventAsync(@event);

            await EventProcessing[@event.GetType()].Invoke(@event);
        }

        private async Task RaiseRegularProductIssuedEvent()
        {
            RegularProductIssued @event = new RegularProductIssued
            {
                Agreement1 = this.State.Agreement1,
                Agreement2 = this.State.Agreement2,
                Args = this.State.Args,
                ProductInfo = await this.GetRegularProductInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseRegularProductRepaidEvent()
        {
            RegularProductRepaid @event = new RegularProductRepaid
            {
                Agreement1 = this.State.Agreement1,
                Agreement2 = this.State.Agreement2,
                Args = this.State.Args,
                PaidAmount = this.PaidAmount,
                PaidOrders = this.PaidOrders.Select(o => o.OrderId.ToGuidString()).ToList(),
                ProductInfo = await this.GetRegularProductInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseRegularProductSoldOutEvent()
        {
            RegularProductSoldOut @event = new RegularProductSoldOut
            {
                Agreement1 = this.State.Agreement1,
                Agreement2 = this.State.Agreement2,
                Args = this.State.Args,
                PaidAmount = this.PaidAmount,
                PaidOrders = this.PaidOrders.Select(o => o.OrderId.ToGuidString()).ToList(),
                ProductInfo = await this.GetRegularProductInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private void ReloadOrderData()
        {
            this.PaidAmount = this.PaidOrders.Sum(o => Convert.ToInt64(o.Principal));
        }
    }
}