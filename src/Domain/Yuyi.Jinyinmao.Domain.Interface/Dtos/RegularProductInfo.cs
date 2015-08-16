// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-25  5:39 PM
// ***********************************************************************
// <copyright file="RegularProductInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moe.Lib;
using Newtonsoft.Json;
using Orleans.Concurrency;
using PostSharp.Patterns.Model;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    /// </summary>
    public static class RegularProductInfoEx
    {
        /// <summary>
        ///     Maps to database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="productModel">The product model.</param>
        /// <param name="agreements">The agreements.</param>
        public static void MapToDBModel(this RegularProductInfo info, RegularProduct productModel, params string[] agreements)
        {
            Dictionary<string, object> i = BuildRegularProductModelInfo(info, agreements);

            productModel.EndSellTime = info.EndSellTime;
            productModel.FinancingSumAmount = info.FinancingSumAmount;
            productModel.Info = i.ToJson();
            productModel.IssueNo = info.IssueNo;
            productModel.IssueTime = info.IssueTime;
            productModel.PledgeNo = info.PledgeNo;
            productModel.ProductCategory = info.ProductCategory;
            productModel.ProductName = info.ProductName;
            productModel.ProductNo = info.ProductNo;
            productModel.Repaid = info.Repaid;
            productModel.RepaidTime = info.RepaidTime;
            productModel.RepaymentDeadline = info.RepaymentDeadline;
            productModel.SettleDate = info.SettleDate;
            productModel.SoldOut = info.SoldOut;
            productModel.SoldOutTime = info.SoldOutTime;
            productModel.StartSellTime = info.StartSellTime;
            productModel.UnitPrice = info.UnitPrice;
            productModel.ValueDate = info.ValueDate;
            productModel.ValueDateMode = info.ValueDateMode;
            productModel.Yield = info.Yield;
        }

        /// <summary>
        ///     To the database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="agreements">The agreements.</param>
        /// <returns>RegularProduct.</returns>
        public static RegularProduct ToDBModel(this RegularProductInfo info, params string[] agreements)
        {
            Dictionary<string, object> i = BuildRegularProductModelInfo(info, agreements);

            RegularProduct productModel = new RegularProduct
            {
                EndSellTime = info.EndSellTime,
                FinancingSumAmount = info.FinancingSumAmount,
                Info = i.ToJson(),
                IssueNo = info.IssueNo,
                IssueTime = info.IssueTime,
                PledgeNo = info.PledgeNo,
                ProductCategory = info.ProductCategory,
                ProductIdentifier = info.ProductId.ToGuidString(),
                ProductName = info.ProductName,
                ProductNo = info.ProductNo,
                Repaid = info.Repaid,
                RepaidTime = info.RepaidTime,
                RepaymentDeadline = info.RepaymentDeadline,
                SettleDate = info.SettleDate,
                SoldOut = info.SoldOut,
                SoldOutTime = info.SoldOutTime,
                StartSellTime = info.StartSellTime,
                UnitPrice = info.UnitPrice,
                ValueDate = info.ValueDate,
                ValueDateMode = info.ValueDateMode,
                Yield = info.Yield
            };

            return productModel;
        }

        private static Dictionary<string, object> BuildRegularProductModelInfo(RegularProductInfo info, string[] agreements)
        {
            Dictionary<string, object> i = new Dictionary<string, object>
            {
                { "Args", info.Args },
                { "BankName", info.BankName },
                { "Drawee", info.Drawee },
                { "DraweeInfo", info.DraweeInfo },
                { "EndorseImageLink", info.EndorseImageLink },
                { "EnterpriseInfo", info.EnterpriseInfo },
                { "EnterpriseLicense", info.EnterpriseLicense },
                { "EnterpriseName", info.EnterpriseName },
                { "PaidAmount", info.PaidAmount },
                { "Period", info.Period },
                { "RiskManagement", info.RiskManagement },
                { "RiskManagementInfo", info.RiskManagementInfo },
                { "RiskManagementMode", info.RiskManagementMode },
                { "Usage", info.Usage }
            };

            for (int j = 1; j < agreements.Length; j++)
            {
                i.Add("Agreement" + j, agreements[j - 1]);
            }

            return i;
        }
    }

    /// <summary>
    ///     RegularProductModelEx.
    /// </summary>
    public static class RegularProductModelEx
    {
        /// <summary>
        ///     To the information.
        /// </summary>
        /// <param name="productModel">The product model.</param>
        /// <returns>RegularProductInfo.</returns>
        [SuppressMessage("ReSharper", "MergeConditionalExpression")]
        public static RegularProductInfo ToInfo(this RegularProduct productModel)
        {
            Dictionary<string, object> i = JsonConvert.DeserializeObject<Dictionary<string, object>>(productModel.Info);

            string argsJson = i["Args"] == null ? JsonHelper.NewDictionary : i["Args"].ToString();
            Dictionary<string, object> args = JsonConvert.DeserializeObject<Dictionary<string, object>>(argsJson);

            RegularProductInfo info = new RegularProductInfo
            {
                Args = args,
                BankName = i["BankName"].IfNotNull(s => s.ToString()),
                Drawee = i["Drawee"].IfNotNull(s => s.ToString()),
                DraweeInfo = i["DraweeInfo"].IfNotNull(s => s.ToString()),
                EndorseImageLink = i["EndorseImageLink"].IfNotNull(s => s.ToString()),
                EndSellTime = productModel.EndSellTime,
                EnterpriseInfo = i["EnterpriseInfo"].IfNotNull(s => s.ToString()),
                EnterpriseLicense = i["EnterpriseLicense"].IfNotNull(s => s.ToString()),
                EnterpriseName = i["EnterpriseName"].IfNotNull(s => s.ToString()),
                FinancingSumAmount = productModel.FinancingSumAmount,
                IssueNo = productModel.IssueNo,
                IssueTime = productModel.IssueTime,
                PaidAmount = 0,
                Period = Convert.ToInt32(i["Period"].IfNotNull(s => s.ToString())),
                PledgeNo = productModel.PledgeNo,
                ProductCategory = productModel.ProductCategory,
                ProductId = Guid.ParseExact(productModel.ProductIdentifier, "N"),
                ProductName = productModel.ProductName,
                ProductNo = productModel.ProductNo,
                Repaid = productModel.Repaid,
                RepaidTime = productModel.RepaidTime,
                RepaymentDeadline = productModel.RepaymentDeadline,
                RiskManagement = i["RiskManagement"].IfNotNull(s => s.ToString()),
                RiskManagementInfo = i["RiskManagementInfo"].IfNotNull(s => s.ToString()),
                RiskManagementMode = i["RiskManagementMode"].IfNotNull(s => s.ToString()),
                SettleDate = productModel.SettleDate,
                SoldOut = productModel.SoldOut,
                SoldOutTime = productModel.SoldOutTime,
                StartSellTime = productModel.StartSellTime,
                UnitPrice = productModel.UnitPrice,
                Usage = i["Usage"].IfNotNull(s => s.ToString()),
                ValueDate = productModel.ValueDate,
                ValueDateMode = productModel.ValueDateMode,
                Yield = productModel.Yield
            };

            return info;
        }
    }

    /// <summary>
    ///     RegularProductInfo.
    /// </summary>
    [Immutable]
    public class RegularProductInfo
    {
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        [Reference]
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        /// <value>The name of the bank.</value>
        public string BankName { get; set; }

        /// <summary>
        ///     Gets or sets the drawee.
        /// </summary>
        /// <value>The drawee.</value>
        public string Drawee { get; set; }

        /// <summary>
        ///     Gets or sets the drawee information.
        /// </summary>
        /// <value>The drawee information.</value>
        public string DraweeInfo { get; set; }

        /// <summary>
        ///     Gets or sets the endorse image link.
        /// </summary>
        /// <value>The endorse image link.</value>
        public string EndorseImageLink { get; set; }

        /// <summary>
        ///     Gets or sets the end sell time.
        /// </summary>
        /// <value>The end sell time.</value>
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the enterprise information.
        /// </summary>
        /// <value>The enterprise information.</value>
        public string EnterpriseInfo { get; set; }

        /// <summary>
        ///     Gets or sets the enterprise license.
        /// </summary>
        /// <value>The enterprise license.</value>
        public string EnterpriseLicense { get; set; }

        /// <summary>
        ///     Gets or sets the name of the enterprise.
        /// </summary>
        /// <value>The name of the enterprise.</value>
        public string EnterpriseName { get; set; }

        /// <summary>
        ///     Gets or sets the financing sum amount.
        /// </summary>
        /// <value>The financing sum amount.</value>
        public long FinancingSumAmount { get; set; }

        /// <summary>
        ///     Gets or sets the issue no.
        /// </summary>
        /// <value>The issue no.</value>
        public int IssueNo { get; set; }

        /// <summary>
        ///     Gets or sets the issue time.
        /// </summary>
        /// <value>The issue time.</value>
        public DateTime IssueTime { get; set; }

        /// <summary>
        ///     Gets or sets the paid amount.
        /// </summary>
        /// <value>The paid amount.</value>
        public long PaidAmount { get; set; }

        /// <summary>
        ///     Gets or sets the period.
        /// </summary>
        /// <value>The period.</value>
        public int Period { get; set; }

        /// <summary>
        ///     Gets or sets the pledge no.
        /// </summary>
        /// <value>The pledge no.</value>
        public string PledgeNo { get; set; }

        /// <summary>
        ///     Gets or sets the product category.
        /// </summary>
        /// <value>The product category.</value>
        public long ProductCategory { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public Guid ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        public string ProductName { get; set; }

        /// <summary>
        ///     Gets or sets the product no.
        /// </summary>
        /// <value>The product no.</value>
        public string ProductNo { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="RegularProductInfo" /> is repaid.
        /// </summary>
        /// <value><c>true</c> if repaid; otherwise, <c>false</c>.</value>
        public bool Repaid { get; set; }

        /// <summary>
        ///     Gets or sets the repaid time.
        /// </summary>
        /// <value>The repaid time.</value>
        public DateTime? RepaidTime { get; set; }

        /// <summary>
        ///     Gets or sets the repayment deadline.
        /// </summary>
        /// <value>The repayment deadline.</value>
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        ///     Gets or sets the risk management.
        /// </summary>
        /// <value>The risk management.</value>
        public string RiskManagement { get; set; }

        /// <summary>
        ///     Gets or sets the risk management information.
        /// </summary>
        /// <value>The risk management information.</value>
        public string RiskManagementInfo { get; set; }

        /// <summary>
        ///     Gets or sets the risk management mode.
        /// </summary>
        /// <value>The risk management mode.</value>
        public string RiskManagementMode { get; set; }

        /// <summary>
        ///     Gets or sets the settle date.
        /// </summary>
        /// <value>The settle date.</value>
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [sold out].
        /// </summary>
        /// <value><c>true</c> if [sold out]; otherwise, <c>false</c>.</value>
        public bool SoldOut { get; set; }

        /// <summary>
        ///     Gets or sets the sold out time.
        /// </summary>
        /// <value>The sold out time.</value>
        public DateTime? SoldOutTime { get; set; }

        /// <summary>
        ///     Gets or sets the start sell time.
        /// </summary>
        /// <value>The start sell time.</value>
        public DateTime StartSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the unit price.
        /// </summary>
        /// <value>The unit price.</value>
        public long UnitPrice { get; set; }

        /// <summary>
        ///     Gets or sets the usage.
        /// </summary>
        /// <value>The usage.</value>
        public string Usage { get; set; }

        /// <summary>
        ///     Gets or sets the value date.
        /// </summary>
        /// <value>The value date.</value>
        public DateTime? ValueDate { get; set; }

        /// <summary>
        ///     Gets or sets the value date mode.
        /// </summary>
        /// <value>The value date mode.</value>
        public int? ValueDateMode { get; set; }

        /// <summary>
        ///     Gets or sets the yield.
        /// </summary>
        /// <value>The yield.</value>
        public int Yield { get; set; }
    }
}