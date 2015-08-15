// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYProduct.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-14  17:00
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
        private long PaidAmount { get; set; }

        #region IJBYProduct Members

        /// <summary>
        ///     build jby transaction as an asynchronous operation.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        public async Task<Guid?> BuildJBYTransactionAsync(JBYAccountTransactionInfo info)
        {
            if (info.Amount > this.State.FinancingSumAmount - this.PaidAmount || this.State.SoldOut || this.State.StartSellTime > DateTime.UtcNow.AddHours(8))
            {
                return null;
            }

            JBYAccountTransactionInfo transactionInfo;
            if (this.State.Transactions.TryGetValue(info.TransactionId, out transactionInfo))
            {
                return transactionInfo.ProductId;
            }

            transactionInfo = new JBYAccountTransactionInfo
            {
                Amount = info.Amount,
                Args = info.Args,
                PredeterminedResultDate = info.PredeterminedResultDate,
                ProductId = this.State.ProductId,
                ResultCode = info.ResultCode,
                ResultTime = info.ResultTime,
                SettleAccountTransactionId = info.SettleAccountTransactionId,
                Trade = info.Trade,
                TradeCode = info.TradeCode,
                TransDesc = info.TransDesc,
                TransactionId = info.TransactionId,
                TransactionTime = info.TransactionTime,
                UserId = info.UserId,
                UserInfo = info.UserInfo
            };

            this.State.Transactions.Add(transactionInfo.TransactionId, transactionInfo);

            await this.SaveStateAsync();

            this.ReloadTransactionData();
            await this.CheckSaleStatusAsync();

            return transactionInfo.ProductId;
        }

        /// <summary>
        ///     Cancels the jby transaction asynchronous.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> CancelJBYTransactionAsync(Guid transactionId)
        {
            if (this.State.SoldOut || this.State.SoldOutTime.HasValue)
            {
                return false;
            }

            JBYAccountTransactionInfo transactionInfo;
            if (this.State.Transactions.TryGetValue(transactionId, out transactionInfo))
            {
                this.State.Transactions.Remove(transactionInfo.TransactionId);

                await this.SaveStateAsync();

                this.ReloadTransactionData();
                await this.CheckSaleStatusAsync();

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Checks the sale status asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public Task CheckSaleStatusAsync()
        {
            if (!this.State.SoldOut && (this.PaidAmount >= this.State.FinancingSumAmount || this.State.EndSellTime.AddMinutes(3) <= DateTime.UtcNow.AddHours(8)))
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
        public async Task<JBYProductInfo> GetProductInfoAsync()
        {
            if (this.State.ProductId == Guid.Empty)
            {
                await this.RefreshAsync(true);
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
                ProductId = this.State.ProductId,
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

            return info;
        }

        /// <summary>
        ///     Hits the shelves asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task<JBYProductInfo> HitShelvesAsync(IssueJBYProduct command)
        {
            this.BeginProcessCommandAsync(command);

            if (this.State.ProductId == Guid.Empty)
            {
                this.State.ProductId = command.ProductId;

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
                this.State.Transactions = new Dictionary<Guid, JBYAccountTransactionInfo>();
                this.State.UnitPrice = command.UnitPrice;
                this.State.UpdateTime = DateTime.UtcNow.AddHours(8);
                this.State.ValueDateMode = command.ValueDateMode;
                this.State.Yield = command.Yield;

                await this.SaveStateAsync();
            }

            await this.RaiseJBYProductIssuedEvent(command);

            return await this.GetProductInfoAsync();
        }

        /// <summary>
        ///     refresh as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<Task<JBYProductInfo>> RefreshAsync(bool force = false)
        {
            if (!force && (!this.State.SoldOut || !this.State.SoldOutTime.HasValue))
            {
                return this.GetProductInfoAsync();
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

            if (nextProduct != null && this.State.ProductId.ToGuidString() != nextProduct.ProductIdentifier)
            {
                Dictionary<string, object> i = JsonConvert.DeserializeObject<Dictionary<string, object>>(nextProduct.Info);

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
                this.State.Transactions = new Dictionary<Guid, JBYAccountTransactionInfo>();
                this.State.UnitPrice = nextProduct.UnitPrice;
                this.State.UpdateTime = DateTime.UtcNow.AddHours(8);
                this.State.ValueDateMode = nextProduct.ValueDateMode;
                this.State.Yield = nextProduct.Yield;

                await this.SaveStateAsync();

                this.ReloadTransactionData();

                await this.RaiseJBYPorductUpdatedEvent();
            }

            return this.GetProductInfoAsync();
        }

        /// <summary>
        ///     Reload state data as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task ReloadAsync()
        {
            await this.ReadStateAsync();
            this.ReloadTransactionData();
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
            await DBSyncHelper.SyncJBYProductAsync(await this.GetProductInfoAsync(), this.State.Agreement1, this.State.Agreement2);
        }

        #endregion IJBYProduct Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.ReloadTransactionData();

            this.RegisterTimer(o => this.CheckSaleStatusAsync(), new object(), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(13));
            this.RegisterTimer(o => this.RefreshAsync(), new object(), TimeSpan.FromMinutes(3), TimeSpan.FromMinutes(7));

            return base.OnActivateAsync();
        }

        private Func<IEvent, Task> GetEventProcessing(Type evenType)
        {
            Dictionary<Type, Func<IEvent, Task>> eventProcessing = new Dictionary<Type, Func<IEvent, Task>>
            {
                { typeof(JBYProductIssued), e => this.GrainFactory.GetGrain<IJBYProductIssuedProcessor>(e.EventId).ProcessEventAsync((JBYProductIssued)e) },
                { typeof(JBYProductSoldOut), e => this.GrainFactory.GetGrain<IJBYProductSoldOutProcessor>(e.EventId).ProcessEventAsync((JBYProductSoldOut)e) },
                { typeof(JBYProductUpdated), e => this.GrainFactory.GetGrain<IJBYProductUpdatedProcessor>(e.EventId).ProcessEventAsync((JBYProductUpdated)e) }
            };

            return eventProcessing[evenType];
        }

        /// <summary>
        ///     process event as an asynchronous operation.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        private async Task ProcessEventAsync(Event @event)
        {
            @event.SourceId = this.State.ProductId.ToGuidString();
            @event.SourceType = this.GetType().Name;
            @event.TimeStamp = DateTime.UtcNow;

            this.StoreEventAsync(@event);

            await this.GetEventProcessing(@event.GetType()).Invoke(@event);
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
                Transactions = this.State.Transactions.Values.ToList()
            };

            await this.ProcessEventAsync(@event);
        }

        private void ReloadTransactionData()
        {
            this.PaidAmount = this.State.Transactions.Values.Sum(o => Convert.ToInt64(o.Amount));
        }
    }
}