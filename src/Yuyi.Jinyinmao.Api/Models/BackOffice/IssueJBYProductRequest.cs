// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  1:49 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  12:40 PM
// ***********************************************************************
// <copyright file="IssueJBYProductRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models.BackOffice
{
    /// <summary>
    ///     IssueJBYProductRequest.
    /// </summary>
    public class IssueJBYProductRequest : IRequest
    {
        /// <summary>
        ///     第一份协议内容，一般为委托协议内容
        /// </summary>
        [Required, JsonProperty("agreement1")]
        public string Agreement1 { get; set; }

        /// <summary>
        ///     第二份协议内容，一般为抵押协议内容
        /// </summary>
        [Required, JsonProperty("agreement2")]
        public string Agreement2 { get; set; }

        /// <summary>
        ///     停售时间
        /// </summary>
        [Required, JsonProperty("endSellTime")]
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     最大融资额度，以“分”为单位
        /// </summary>
        [Required, Range(1, int.MaxValue), JsonProperty("financingSumAmount")]
        public int FinancingSumAmount { get; set; }

        /// <summary>
        ///     发行期数，可以重复，必须大于0
        /// </summary>
        [Required, Range(0, 1000000000), JsonProperty("issueNo")]
        public int IssueNo { get; set; }

        /// <summary>
        ///     产品分类
        /// </summary>
        [JsonProperty("productCategory")]
        public long ProductCategory { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        [Required, MaxLength(50), JsonProperty("productName")]
        public string ProductName { get; set; }

        /// <summary>
        ///     产品编号
        /// </summary>
        [Required, MaxLength(40), JsonProperty("productNo")]
        public string ProductNo { get; set; }

        /// <summary>
        ///     开售时间
        /// </summary>
        [Required, JsonProperty("startSellTime")]
        public DateTime StartSellTime { get; set; }

        /// <summary>
        ///     单价，以“分”为单位，10000即每份100元
        /// </summary>
        [Required, Range(1, int.MaxValue), JsonProperty("unitPrice")]
        public int UnitPrice { get; set; }

        /// <summary>
        ///     起息方式
        /// </summary>
        [JsonProperty("valueDateMode")]
        public int ValueDateMode { get; set; }
    }
}