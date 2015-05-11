// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  12:26 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-12  12:46 AM
// ***********************************************************************
// <copyright file="JBYProduct.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Newtonsoft.Json;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.EventProcessor;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     JBYProduct.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public class JBYProduct : EntityGrain<IJBYProductState>, IJBYProduct
    {
        private int PaidAmount { get; set; }

        #region IJBYProduct Members

        /// <summary>
        ///     Builds the jby transcation asynchronous.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Task&lt;Transcation&gt;.</returns>
        public async Task<Tuple<bool, Guid>> BuildJBYTranscationAsync(TranscationInfo info)
        {
            if (this.State.SoldOut)
            {
                return new Tuple<bool, Guid>(false, Guid.Empty);
            }

            if (this.State.StartSellTime > DateTime.UtcNow.AddHours(8))
            {
                return new Tuple<bool, Guid>(false, Guid.Empty);
            }

            if (info.Amount > this.State.FinancingSumAmount - this.PaidAmount)
            {
                return new Tuple<bool, Guid>(false, Guid.Empty);
            }

            this.State.Transcations.Add(info);

            await this.State.WriteStateAsync();

            this.ReloadTranscationData();

            if (this.PaidAmount == this.State.FinancingSumAmount && !this.State.SoldOut)
            {
#pragma warning disable 4014
                this.SetToSoldOutAsync();
#pragma warning restore 4014
            }

            return new Tuple<bool, Guid>(true, this.State.Id);
        }

        /// <summary>
        ///     Gets the agreement asynchronous.
        /// </summary>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public Task<string> GetAgreementAsync(int agreementIndex)
        {
            if (agreementIndex == 1)
            {
                return Task.FromResult(this.State.Agreement1);
            }
            if (agreementIndex == 2)
            {
                return Task.FromResult(this.State.Agreement2);
            }
            return Task.FromResult(string.Empty);
        }

        /// <summary>
        ///     Gets the product information asynchronous.
        /// </summary>
        /// <returns>Task&lt;JBYProductInfo&gt;.</returns>
        public Task<JBYProductInfo> GetProductInfoAsync()
        {
            JBYProductInfo info = new JBYProductInfo
            {
                EndSellTime = this.State.EndSellTime,
                FinancingSumAmount = this.State.FinancingSumAmount,
                Info = new Dictionary<string, object>(),
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
                ValueDateMode = this.State.ValueDateMode,
                Yield = this.State.Yield
            };

            if ((info.PaidAmount >= info.FinancingSumAmount || info.EndSellTime < DateTime.UtcNow.AddHours(8)) && !info.SoldOut)
            {
#pragma warning disable 4014
                this.SetToSoldOutAsync();
#pragma warning restore 4014
            }

            return Task.FromResult(info);
        }

        /// <summary>
        ///     Hits the shelves asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task HitShelvesAsync(IssueJBYProduct command)
        {
            await this.BeginProcessCommandAsync(command);

            if (this.State.Id == Guid.Empty)
            {
                this.State.Agreement1 = command.Agreement1;
                this.State.Agreement2 = command.Agreement2;
                this.State.Args = command.Args;
                this.State.EndSellTime = command.EndSellTime;
                this.State.FinancingSumAmount = command.FinancingSumAmount;
                this.State.Info = new Dictionary<string, object>();
                this.State.IssueNo = command.IssueNo;
                this.State.IssueTime = command.IssueTime;
                this.State.Transcations = new List<TranscationInfo>();
                this.State.ProductCategory = command.ProductCategory;
                this.State.ProductName = command.ProductName;
                this.State.ProductNo = command.ProductNo;
                this.State.SoldOut = false;
                this.State.SoldOutTime = null;
                this.State.StartSellTime = command.StartSellTime;
                this.State.UnitPrice = command.UnitPrice;
                this.State.ValueDateMode = command.ValueDateMode;
                this.State.Yield = command.Yield;

                await this.State.WriteStateAsync();
            }

            await this.RaiseJBYProductIssuedEvent(command);
        }

        /// <summary>
        ///     set to sold out as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SetToSoldOutAsync()
        {
            if (this.State.SoldOut)
            {
                return;
            }

            this.State.SoldOut = true;
            this.State.SoldOutTime = DateTime.UtcNow.AddHours(8);

            await this.State.WriteStateAsync();

            await this.RaiseJBYProductSoldOutEvent();
        }

        /// <summary>
        ///     Updates the sale information asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task UpdateSaleInfoAsync(Models.JBYProduct product)
        {
            if (this.State.Id.ToGuidString() != product.ProductIdentifier)
            {
                Dictionary<string, object> infos = JsonConvert.DeserializeObject<Dictionary<string, object>>(product.Info);
                infos.Add("UpdateTime", DateTime.UtcNow.AddHours(8));

                this.State.Agreement1 = infos.First(kv => kv.Key == "Agreement1").ToString();
                this.State.Agreement2 = infos.First(kv => kv.Key == "Agreement2").ToString();
                this.State.Args = new Dictionary<string, object>();
                this.State.EndSellTime = product.EndSellTime;
                this.State.FinancingSumAmount = product.FinancingSumAmount;
                this.State.Info = infos;
                this.State.IssueNo = product.IssueNo;
                this.State.IssueTime = product.IssueTime;
                this.State.ProductCategory = product.ProductCategory;
                this.State.ProductName = product.ProductName;
                this.State.ProductNo = product.ProductNo;
                this.State.SoldOut = product.SoldOut;
                this.State.SoldOutTime = product.SoldOutTime;
                this.State.StartSellTime = product.StartSellTime;
                this.State.Transcations = new List<TranscationInfo>();
                this.State.UnitPrice = product.UnitPrice;
                this.State.ValueDateMode = product.ValueDateMode;
            }

            await this.State.WriteStateAsync();

            await this.RaiseJBYPorductUpdatedEvent();
        }

        #endregion IJBYProduct Members

        /// <summary>
        ///     Reload state data as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public override async Task ReloadAsync()
        {
            await this.State.ReadStateAsync();
            this.ReloadTranscationData();
        }

        private async Task RaiseJBYPorductUpdatedEvent()
        {
            JBYProductUpdated @event = new JBYProductUpdated
            {
                Agreement1 = this.State.Agreement1,
                Agreement2 = this.State.Agreement2,
                Args = this.State.Args,
                EndSellTime = this.State.EndSellTime,
                FinancingSumAmount = this.State.FinancingSumAmount,
                IssueNo = this.State.IssueNo,
                IssueTime = this.State.IssueTime,
                ProductCategory = this.State.ProductCategory,
                ProductName = this.State.ProductName,
                ProductNo = this.State.ProductNo,
                ProductId = this.State.Id,
                SourceId = this.State.Id.ToString(),
                SourceType = this.GetType().Name,
                StartSellTime = this.State.StartSellTime,
                UnitPrice = this.State.UnitPrice,
                ValueDateMode = this.State.ValueDateMode,
                Yield = this.State.Yield
            };

            await this.StoreEventAsync(@event);

            await JBYProductUpdatedProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        private async Task RaiseJBYProductIssuedEvent(IssueJBYProduct command)
        {
            JBYProductIssued @event = new JBYProductIssued
            {
                Agreement1 = command.Agreement1,
                Agreement2 = command.Agreement2,
                Args = command.Args,
                EndSellTime = command.EndSellTime,
                FinancingSumAmount = command.FinancingSumAmount,
                IssueNo = command.IssueNo,
                IssueTime = command.IssueTime,
                ProductCategory = command.ProductCategory,
                ProductId = command.ProductId,
                ProductName = command.ProductName,
                ProductNo = command.ProductNo,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                StartSellTime = command.StartSellTime,
                UnitPrice = command.UnitPrice,
                ValueDateMode = command.ValueDateMode,
                Yield = command.Yield
            };

            await this.StoreEventAsync(@event);

            await JBYProductIssuedProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        private async Task RaiseJBYProductSoldOutEvent()
        {
            JBYProductSoldOut @event = new JBYProductSoldOut
            {
                Args = this.State.Args,
                EndSellTime = this.State.EndSellTime,
                FinancingSumAmount = this.State.FinancingSumAmount,
                Info = this.State.Info,
                IssueNo = this.State.IssueNo,
                IssueTime = this.State.IssueTime,
                PaidAmount = this.PaidAmount,
                ProductCategory = this.State.ProductCategory,
                ProductId = this.State.Id,
                ProductName = this.State.ProductName,
                ProductNo = this.State.ProductNo,
                SoldOut = this.State.SoldOut,
                SoldOutTime = this.State.SoldOutTime.GetValueOrDefault(),
                StartSellTime = this.State.StartSellTime,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                Transcations = this.State.Transcations,
                UnitPrice = this.State.UnitPrice,
                ValueDateMode = this.State.ValueDateMode,
                Yield = this.State.Yield
            };

            await this.StoreEventAsync(@event);
        }

        private void ReloadTranscationData()
        {
            this.PaidAmount = this.State.Transcations.Sum(o => o.Amount);
        }
    }
}