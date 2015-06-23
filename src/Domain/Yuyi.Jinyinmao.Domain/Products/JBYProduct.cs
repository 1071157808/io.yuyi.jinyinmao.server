// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-22  11:48 PM
// ***********************************************************************
// <copyright file="JBYProduct.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     JBYProduct.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public class JBYProduct : EntityGrain<IJBYProductState>, IJBYProduct
    {
        /// <summary>
        ///     The event processing
        /// </summary>
        private static readonly Dictionary<Type, Func<IEvent, Task>> EventProcessing = new Dictionary<Type, Func<IEvent, Task>>
        {
            { typeof(JBYProductIssued), e => JBYProductIssuedProcessorFactory.GetGrain(e.EventId).ProcessEventAsync((JBYProductIssued)e) },
            { typeof(JBYProductSoldOut), e => JBYProductSoldOutProcessorFactory.GetGrain(e.EventId).ProcessEventAsync((JBYProductSoldOut)e) },
            { typeof(JBYProductUpdated), e => JBYProductUpdatedProcessorFactory.GetGrain(e.EventId).ProcessEventAsync((JBYProductUpdated)e) }
        };

        private long PaidAmount { get; set; }

        #region IJBYProduct Members

        /// <summary>
        ///     build jby transcation as an asynchronous operation.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Task&lt;JBYAccountTranscationInfo&gt;.</returns>
        public async Task<Guid?> BuildJBYTranscationAsync(JBYAccountTranscationInfo info)
        {
            if (info.Amount > this.State.FinancingSumAmount - this.PaidAmount || this.State.SoldOut || this.State.StartSellTime > DateTime.UtcNow.AddHours(8))
            {
                return null;
            }

            JBYAccountTranscationInfo transcationInfo;
            if (this.State.Transcations.TryGetValue(info.TransactionId, out transcationInfo))
            {
                return transcationInfo.ProductId;
            }

            transcationInfo = new JBYAccountTranscationInfo
            {
                Amount = info.Amount,
                Args = info.Args,
                PredeterminedResultDate = info.PredeterminedResultDate,
                ProductId = this.State.ProductId,
                ResultCode = info.ResultCode,
                ResultTime = info.ResultTime,
                SettleAccountTranscationId = info.SettleAccountTranscationId,
                Trade = info.Trade,
                TradeCode = info.TradeCode,
                TransDesc = info.TransDesc,
                TransactionId = info.TransactionId,
                TransactionTime = info.TransactionTime,
                UserId = info.UserId,
                UserInfo = info.UserInfo
            };

            this.State.Transcations.Add(transcationInfo.TransactionId, transcationInfo);

            await this.SaveStateAsync();

            this.ReloadTranscationData();
            await this.CheckSaleStatusAsync();

            return transcationInfo.ProductId;
        }

        /// <summary>
        ///     Checks the sale status asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public Task CheckSaleStatusAsync()
        {
            if ((this.PaidAmount >= this.State.FinancingSumAmount || this.State.EndSellTime.AddMinutes(3) <= DateTime.UtcNow.AddHours(8)) && !this.State.SoldOut)
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
        ///     Gets the jby product paid amount asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int64&gt;.</returns>
        public Task<long> GetJBYProductPaidAmountAsync()
        {
            return Task.FromResult(this.PaidAmount);
        }

        /// <summary>
        ///     Gets the product information asynchronous.
        /// </summary>
        /// <returns>Task&lt;JBYProductInfo&gt;.</returns>
        public Task<JBYProductInfo> GetProductInfoAsync()
        {
            if (this.State.ProductNo.IsNullOrEmpty())
            {
                return Task.FromResult<JBYProductInfo>(null);
            }

            JBYProductInfo info = new JBYProductInfo
            {
                Args = this.State.Args,
                EndSellTime = this.State.EndSellTime,
                FinancingSumAmount = this.State.FinancingSumAmount,
                IssueNo = this.State.IssueNo,
                IssueTime = this.State.IssueTime,
                PaidAmount = this.PaidAmount,
                ProductCategory = this.State.ProductCategory,
                ProductId = this.State.Id,
                ProductName = this.State.ProductName,
                ProductNo = this.State.ProductNo,
                SoldOut = this.State.SoldOut,
                SoldOutTime = this.State.SoldOutTime,
                StartSellTime = this.State.StartSellTime,
                UnitPrice = this.State.UnitPrice,
                UpdateTime = this.State.UpdateTime,
                ValueDateMode = this.State.ValueDateMode,
                Yield = this.State.Yield
            };

            return Task.FromResult(info);
        }

        /// <summary>
        ///     Hits the shelves asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task HitShelvesAsync(IssueJBYProduct command)
        {
            this.BeginProcessCommandAsync(command);

            if (this.State.Id == Guid.Empty)
            {
                this.State.Id = command.ProductId;
                this.State.Agreement1 = command.Agreement1;
                this.State.Agreement2 = command.Agreement2;
                this.State.Args = command.Args;
                this.State.EndSellTime = command.EndSellTime;
                this.State.FinancingSumAmount = command.FinancingSumAmount;
                this.State.IssueNo = command.IssueNo;
                this.State.IssueTime = command.IssueTime;
                this.State.ProductCategory = command.ProductCategory;
                this.State.ProductId = command.ProductId;
                this.State.ProductName = command.ProductName;
                this.State.ProductNo = command.ProductNo;
                this.State.SoldOut = false;
                this.State.SoldOutTime = null;
                this.State.StartSellTime = command.StartSellTime;
                this.State.Transcations = new Dictionary<Guid, JBYAccountTranscationInfo>();
                this.State.UnitPrice = command.UnitPrice;
                this.State.UpdateTime = DateTime.UtcNow.AddHours(8);
                this.State.ValueDateMode = command.ValueDateMode;
                this.State.Yield = command.Yield;

                await this.SaveStateAsync();
            }

            await this.RaiseJBYProductIssuedEvent(command);
        }

        /// <summary>
        ///     refresh as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task RefreshAsync()
        {
            if (!this.State.SoldOut || !this.State.SoldOutTime.HasValue)
            {
                return;
            }

            string productIdentifier = this.State.ProductId.ToGuidString();
            DateTime now = DateTime.UtcNow.AddHours(8);
            Models.JBYProduct nextProduct;
            using (JYMDBContext db = new JYMDBContext())
            {
                nextProduct = await db.ReadonlyQuery<Models.JBYProduct>()
                    .Where(p => p.ProductIdentifier != productIdentifier && p.IssueNo > this.State.IssueNo && !p.SoldOut && p.EndSellTime > now)
                    .OrderBy(p => p.IssueNo).ThenBy(p => p.IssueTime).FirstOrDefaultAsync();
            }

            if (nextProduct != null && this.State.Id.ToGuidString() != nextProduct.ProductIdentifier)
            {
                Dictionary<string, object> i = JsonConvert.DeserializeObject<Dictionary<string, object>>(nextProduct.Info);

                this.State.Id = Guid.ParseExact(nextProduct.ProductIdentifier, "N");
                this.State.Agreement1 = i["Agreement1"].ToString();
                this.State.Agreement2 = i["Agreement2"].ToString();
                this.State.Args = JsonConvert.DeserializeObject<Dictionary<string, object>>(i["Args"].ToString());
                this.State.EndSellTime = nextProduct.EndSellTime;
                this.State.FinancingSumAmount = nextProduct.FinancingSumAmount;
                this.State.IssueNo = nextProduct.IssueNo;
                this.State.IssueTime = nextProduct.IssueTime;
                this.State.ProductCategory = nextProduct.ProductCategory;
                this.State.ProductId = Guid.ParseExact(nextProduct.ProductIdentifier, "N");
                this.State.ProductName = nextProduct.ProductName;
                this.State.ProductNo = nextProduct.ProductNo;
                this.State.SoldOut = false;
                this.State.SoldOutTime = null;
                this.State.StartSellTime = nextProduct.StartSellTime;
                this.State.Transcations = new Dictionary<Guid, JBYAccountTranscationInfo>();
                this.State.UnitPrice = nextProduct.UnitPrice;
                this.State.UpdateTime = DateTime.UtcNow.AddHours(8);
                this.State.ValueDateMode = nextProduct.ValueDateMode;
                this.State.Yield = nextProduct.Yield;

                await this.SaveStateAsync();

                this.ReloadTranscationData();

                await this.RaiseJBYPorductUpdatedEvent();
            }
        }

        /// <summary>
        ///     Reload state data as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public override async Task ReloadAsync()
        {
            await this.State.ReadStateAsync();
            this.ReloadTranscationData();
            await this.SyncAsync();
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

            await this.RaiseJBYProductSoldOutEvent();
        }

        /// <summary>
        ///     Synchronizes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SyncAsync()
        {
            await DBSyncHelper.SyncJBYProduct(await this.GetProductInfoAsync(), this.State.Agreement1, this.State.Agreement2);
        }

        #endregion IJBYProduct Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.ReloadTranscationData();

            this.RegisterTimer(o => this.CheckSaleStatusAsync(), new object(), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(13));
            this.RegisterTimer(o => this.RefreshAsync(), new object(), TimeSpan.FromMinutes(3), TimeSpan.FromMinutes(7));

            return base.OnActivateAsync();
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

        private async Task RaiseJBYPorductUpdatedEvent()
        {
            JBYProductUpdated @event = new JBYProductUpdated
            {
                Agreement1 = this.State.Agreement1,
                Agreement2 = this.State.Agreement2,
                Args = this.State.Args,
                ProductInfo = await this.GetProductInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseJBYProductIssuedEvent(IssueJBYProduct command)
        {
            JBYProductInfo info = new JBYProductInfo
            {
                Args = command.Args,
                EndSellTime = command.EndSellTime,
                FinancingSumAmount = command.FinancingSumAmount,
                IssueNo = command.IssueNo,
                IssueTime = command.IssueTime,
                PaidAmount = 0,
                ProductCategory = command.ProductCategory,
                ProductId = command.ProductId,
                ProductName = command.ProductName,
                ProductNo = command.ProductNo,
                SoldOut = false,
                SoldOutTime = null,
                StartSellTime = command.StartSellTime,
                UnitPrice = command.UnitPrice,
                UpdateTime = DateTime.MinValue,
                ValueDateMode = command.ValueDateMode,
                Yield = command.Yield
            };

            JBYProductIssued @event = new JBYProductIssued
            {
                Agreement1 = command.Agreement1,
                Agreement2 = command.Agreement2,
                Args = command.Args,
                ProductInfo = info
            };

            await this.ProcessEventAsync(@event);
        }

        private async Task RaiseJBYProductSoldOutEvent()
        {
            JBYProductSoldOut @event = new JBYProductSoldOut
            {
                Args = this.State.Args,
                ProductInfo = await this.GetProductInfoAsync(),
                Transcations = this.State.Transcations.Values.ToList()
            };

            await this.ProcessEventAsync(@event);
        }

        private void ReloadTranscationData()
        {
            this.PaidAmount = this.State.Transcations.Values.Sum(o => Convert.ToInt64(o.Amount));
        }
    }
}