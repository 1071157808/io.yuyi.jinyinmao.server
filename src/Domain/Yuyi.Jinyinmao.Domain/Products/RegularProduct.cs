// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  12:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  12:39 AM
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
        private List<OrderInfo> PaidOrders { get; set; }

        #region IRegularProduct Members

        /// <summary>
        ///     build order as an asynchronous operation.
        /// </summary>
        /// <param name="commnad">The commnad.</param>
        /// <param name="userInfo">The user information.</param>
        /// <param name="transcationInfo">The transcation information.</param>
        /// <returns>Task&lt;OrderInfo&gt;.</returns>
        public async Task<OrderInfo> BuildOrderAsync(RegularInvesting commnad, UserInfo userInfo, SettleAccountTranscationInfo transcationInfo)
        {
            if (this.State.SoldOut)
            {
                return null;
            }

            if (this.State.StartSellTime > DateTime.UtcNow.AddHours(8))
            {
                return null;
            }

            if (transcationInfo.Amount > this.State.FinancingSumAmount - this.PaidAmount)
            {
                return null;
            }

            Order order = this.State.Orders.Values.FirstOrDefault(o => o.AccountTranscationId == transcationInfo.TransactionId);
            if (order == null)
            {
                ISequenceGenerator generator = SequenceGeneratorFactory.GetGrain(Guid.Empty);
                string orderNo = await generator.GenerateNoAsync('O');
                DateTime valueDate = this.BuildValueDate();
                int interest = this.BuildInterest(valueDate, transcationInfo.Amount);

                DateTime now = DateTime.UtcNow.AddHours(8);
                order = new Order
                {
                    AccountTranscationId = transcationInfo.TransactionId,
                    Args = commnad.Args,
                    Cellphone = userInfo.Cellphone,
                    ExtraInterest = 0,
                    ExtraYield = 0,
                    Interest = interest,
                    IsRepaid = false,
                    OrderId = Guid.NewGuid(),
                    OrderNo = orderNo,
                    OrderTime = now,
                    Principal = commnad.Amount,
                    ProductCategory = this.State.ProductCategory,
                    ProductId = this.State.Id,
                    ProductSnapshot = await this.GetRegularProductInfoAsync(),
                    RepaidTime = null,
                    ResultCode = 1,
                    ResultTime = now,
                    SettleDate = this.State.SettleDate.Date,
                    TransDesc = "购买成功",
                    UserId = userInfo.UserId,
                    UserInfo = userInfo,
                    ValueDate = valueDate,
                    Yield = this.State.Yield
                };

                this.State.Orders.Add(order.OrderId, order);

                await this.State.WriteStateAsync();

                this.ReloadOrderData();
            }

            if (this.PaidAmount == this.State.FinancingSumAmount && !this.State.SoldOut)
            {
                this.SetToSoldOutAsync().Forget();
            }

            return order.ToInfo();
        }

        /// <summary>
        ///     Gets the agreement asynchronous.
        /// </summary>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
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

            if (info.PaidAmount >= info.FinancingSumAmount && !info.SoldOut)
            {
                this.SetToSoldOutAsync().Forget();
            }

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

            this.BeginProcessCommandAsync(command).Forget();

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
            this.State.Orders = new Dictionary<Guid, Order>();
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

                CloudBlockBlob blob = SiloClusterConfig.PublicFileContainer.GetBlockBlobReference("EndorseImage-" + command.ProductId.ToGuidString());
                blob.Properties.ContentType = "image/jpeg";
                await blob.UploadFromStreamAsync(stream);
                this.State.EndorseImageLink = blob.Uri.AbsolutePath;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

            await this.State.WriteStateAsync();

            await this.RaiseRegularProductIssuedEvent(command);
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

            DateTime now = DateTime.UtcNow.AddHours(8);

            this.State.Repaid = true;
            this.State.RepaidTime = now;

            await this.State.WriteStateAsync();

            this.PaidOrders.ForEach(async o =>
            {
                o.IsRepaid = true;
                o.RepaidTime = now;

                IUser user = UserFactory.GetGrain(o.UserId);
                await user.RepayOrderAsync(o.OrderId, now);
            });

            await this.RaiseRegularProductRepaidEvent();
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

            await this.State.WriteStateAsync();

            await this.RaiseRegularProductSoldOutEvent();
        }

        /// <summary>
        ///     Reload state data as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public override async Task ReloadAsync()
        {
            await this.State.ReadStateAsync();
            this.ReloadOrderData();
        }

        #endregion IRegularProduct Members

        private async Task RaiseRegularProductRepaidEvent()
        {
            RegularProductRepaid @event = new RegularProductRepaid
            {
                Agreement1 = this.State.Agreement1,
                Agreement2 = this.State.Agreement2,
                Args = this.State.Args,
                ProductInfo = await this.GetRegularProductInfoAsync(),
                PaidAmount = this.PaidAmount,
                PaidOrders = this.PaidOrders
            };

            await this.ProcessEventAsync(@event);
        }

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.ReloadOrderData();

            return base.OnActivateAsync();
        }

        private int BuildInterest(DateTime valueDate, int principal)
        {
            int dayCount = (this.State.SettleDate.Date.AddHours(1) - valueDate.Date).Days;
            return (int)((long)principal * this.State.Yield * dayCount / 3600000);
        }

        private DateTime BuildValueDate()
        {
            return this.State.ValueDateMode == null ?
                this.State.ValueDate.GetValueOrDefault(DateTime.UtcNow.AddHours(8).Date)
                : DateTime.UtcNow.AddHours(8).AddDays(this.State.ValueDateMode.GetValueOrDefault(0)).Date;
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

        private async Task RaiseRegularProductSoldOutEvent()
        {
            RegularProductSoldOut @event = new RegularProductSoldOut
            {
                Agreement1 = this.State.Agreement1,
                Agreement2 = this.State.Agreement2,
                Args = this.State.Args,
                PaidAmount = this.PaidAmount,
                PaidOrders = this.PaidOrders,
                ProductInfo = await this.GetRegularProductInfoAsync()
            };

            await this.ProcessEventAsync(@event);
        }

        private void ReloadOrderData()
        {
            this.PaidOrders = this.State.Orders.Values.Where(o => o.ResultCode == 1).Select(o => o.ToInfo()).ToList();
            this.PaidAmount = this.PaidOrders.Sum(o => Convert.ToInt64(o.Principal));
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

            this.StoreEventAsync(@event).Forget();

            await EventProcessing[@event.GetType()].Invoke(@event);
        }
    }
}