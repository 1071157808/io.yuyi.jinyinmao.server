// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  12:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-30  5:02 AM
// ***********************************************************************
// <copyright file="RegularProduct.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
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
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     RegularProduct.
    /// </summary>
    public class RegularProduct : EntityGrain<IRegularProductState>, IRegularProduct
    {
        private int PaidAmount { get; set; }

        private List<OrderInfo> PaidOrders { get; set; }

        #region IRegularProduct Members

        /// <summary>
        ///     Gets the agreement asynchronous.
        /// </summary>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
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
            return null;
        }

        /// <summary>
        ///     Gets the paid amount.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public Task<int> GetPaidAmountAsync()
        {
            return Task.FromResult(0);
        }

        /// <summary>
        ///     Gets the product paid amount asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public Task<int> GetProductPaidAmountAsync()
        {
            return Task.FromResult(this.PaidAmount);
        }

        /// <summary>
        ///     Gets the regular product information asynchronous.
        /// </summary>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        public Task<RegularProductInfo> GetRegularProductInfoAsync()
        {
            if (this.State.ProductNo.IsNotNullOrEmpty())
            {
                return null;
            }

            string info = new
            {
                this.State.BankName,
                this.State.Drawee,
                this.State.DraweeInfo,
                this.State.EndorseImageLink,
                this.State.EnterpriseInfo,
                this.State.EnterpriseLicense,
                this.State.EnterpriseName,
                this.State.Period,
                this.State.RiskManagement,
                this.State.RiskManagementInfo,
                this.State.RiskManagementMode,
                this.State.Usage
            }.ToJson();

            return Task.FromResult(new RegularProductInfo
            {
                EndSellTime = this.State.EndSellTime,
                FinancingSumAmount = this.State.FinancingSumAmount,
                Info = info,
                IssueNo = this.State.IssueNo,
                IssueTime = this.State.IssueTime,
                PaidAmount = this.PaidAmount,
                PledgeNo = this.State.PledgeNo,
                ProductCategory = this.State.ProductCategory,
                ProductIdentifier = this.State.Id.ToGuidString(),
                ProductName = this.State.ProductName,
                ProductNo = this.State.ProductNo,
                Repaid = this.State.Repaid,
                RepaidTime = this.State.RepaidTime,
                RepaymentDeadline = this.State.RepaymentDeadline,
                SettleDate = this.State.SettleDate,
                SoldOut = this.State.SoldOut,
                SoldOutTime = this.State.SoldOutTime,
                StartSellTime = this.State.StartSellTime,
                UnitPrice = this.State.UnitPrice,
                ValueDate = this.State.ValueDate,
                ValueDateMode = this.State.ValueDateMode,
                Yield = this.State.Yield
            });
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
            this.State.Period = command.Period;
            this.State.PledgeNo = command.PledgeNo;
            this.State.ProductCategory = command.ProductCategory;
            this.State.ProductName = command.ProductName;
            this.State.ProductNo = command.ProductNo;
            this.State.RepaymentDeadline = command.RepaymentDeadline;
            this.State.RiskManagement = command.RiskManagement;
            this.State.RiskManagementInfo = command.RiskManagementInfo;
            this.State.RiskManagementMode = command.RiskManagementMode;
            this.State.SettleDate = command.SettleDate;
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

            await this.StoreCommandAsync(command);
            await this.RaiseRegularProductIssued(command);

            await this.State.WriteStateAsync();
        }

        #endregion IRegularProduct Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            if (this.State.Orders == null)
            {
                this.State.Orders = new List<OrderInfo>();
            }

            this.ReloadOrderInfos();

            return base.OnActivateAsync();
        }

        private async Task RaiseRegularProductIssued(IssueRegularProduct command)
        {
            RegularProductIssued @event = new RegularProductIssued
            {
                Agreement1 = this.State.Agreement1,
                Agreement2 = this.State.Agreement2,
                Args = this.State.Args,
                BankName = this.State.BankName,
                Drawee = this.State.Drawee,
                DraweeInfo = this.State.DraweeInfo,
                EndorseImageLink = this.State.EndorseImageLink,
                EndSellTime = this.State.EndSellTime,
                EnterpriseInfo = this.State.EnterpriseInfo,
                EnterpriseLicense = this.State.EnterpriseLicense,
                EnterpriseName = this.State.EnterpriseName,
                FinancingSumCount = this.State.FinancingSumAmount,
                IssueNo = this.State.IssueNo,
                IssueTime = this.State.IssueTime,
                Period = this.State.Period,
                PledgeNo = this.State.PledgeNo,
                ProductCategory = this.State.ProductCategory,
                ProductId = this.State.Id,
                ProductName = this.State.ProductName,
                ProductNo = this.State.ProductNo,
                RepaymentDeadline = this.State.RepaymentDeadline,
                RiskManagement = this.State.RiskManagement,
                RiskManagementInfo = this.State.RiskManagementInfo,
                RiskManagementMode = this.State.RiskManagementMode,
                SettleDate = this.State.SettleDate,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                StartSellTime = this.State.StartSellTime,
                UnitPrice = this.State.UnitPrice,
                Usage = this.State.Usage,
                ValueDate = this.State.ValueDate,
                ValueDateMode = this.State.ValueDateMode,
                Yield = this.State.Yield
            };

            await this.StoreEventAsync(@event);
        }

        private void ReloadOrderInfos()
        {
            this.PaidOrders = this.State.Orders.Where(o => o.ResultCode == 1).ToList();
            this.PaidAmount = this.PaidOrders.Sum(o => o.Principal);
        }
    }
}
