// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  7:48 PM
// ***********************************************************************
// <copyright file="JBYProductInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Moe.Lib;
using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     JBYProductInfoEx.
    /// </summary>
    public static class JBYProductInfoEx
    {
        /// <summary>
        ///     Maps to database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="productModel">The product model.</param>
        /// <param name="agreements">The agreements.</param>
        public static void MapToDBModel(this JBYProductInfo info, JBYProduct productModel, params string[] agreements)
        {
            Dictionary<string, object> i = BuildJBYProductModelInfo(info, agreements);

            productModel.EndSellTime = info.EndSellTime;
            productModel.FinancingSumAmount = info.FinancingSumAmount;
            productModel.Info = i.ToJson();
            productModel.IssueNo = info.IssueNo;
            productModel.IssueTime = info.IssueTime;
            productModel.ProductCategory = info.ProductCategory;
            productModel.ProductName = info.ProductName;
            productModel.ProductNo = info.ProductNo;
            productModel.SoldOut = info.SoldOut;
            productModel.SoldOutTime = info.SoldOutTime;
            productModel.StartSellTime = info.StartSellTime;
            productModel.UnitPrice = info.UnitPrice;
            productModel.ValueDateMode = info.ValueDateMode;
            productModel.Yield = info.Yield;
        }

        /// <summary>
        ///     To the database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="agreements">The agreements.</param>
        /// <returns>JBYProduct.</returns>
        public static JBYProduct ToDBModel(this JBYProductInfo info, params string[] agreements)
        {
            Dictionary<string, object> i = BuildJBYProductModelInfo(info, agreements);

            JBYProduct productModel = new JBYProduct
            {
                EndSellTime = info.EndSellTime,
                FinancingSumAmount = info.FinancingSumAmount,
                Info = i.ToJson(),
                IssueNo = info.IssueNo,
                IssueTime = info.IssueTime,
                ProductCategory = info.ProductCategory,
                ProductIdentifier = info.ProductId.ToGuidString(),
                ProductName = info.ProductName,
                ProductNo = info.ProductNo,
                SoldOut = info.SoldOut,
                SoldOutTime = info.SoldOutTime,
                StartSellTime = info.StartSellTime,
                UnitPrice = info.UnitPrice,
                ValueDateMode = info.ValueDateMode,
                Yield = info.Yield
            };

            return productModel;
        }

        private static Dictionary<string, object> BuildJBYProductModelInfo(JBYProductInfo info, params string[] agreements)
        {
            Dictionary<string, object> i = new Dictionary<string, object>
            {
                { "Args", info.Args },
                { "PaidAmount", info.PaidAmount },
                { "UpdateTime", info.UpdateTime }
            };

            for (int j = 1; j <= agreements.Length; j++)
            {
                i.Add("Agreement" + j, agreements[j - 1]);
            }

            return i;
        }
    }

    /// <summary>
    ///     JBYProductInfo.
    /// </summary>
    [Immutable]
    public class JBYProductInfo
    {
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     Gets or sets the end sell time.
        /// </summary>
        /// <value>The end sell time.</value>
        public DateTime EndSellTime { get; set; }

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
        ///     Gets or sets the update time.
        /// </summary>
        /// <value>The update time.</value>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        ///     Gets or sets the value date mode.
        /// </summary>
        /// <value>The value date mode.</value>
        public int ValueDateMode { get; set; }

        /// <summary>
        ///     Gets or sets the yield.
        /// </summary>
        /// <value>The yield.</value>
        public int Yield { get; set; }
    }
}