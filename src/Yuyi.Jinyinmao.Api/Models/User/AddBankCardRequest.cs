// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-03  2:56 AM
// ***********************************************************************
// <copyright file="AddBankCardRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Moe.AspNet.Models;
using Moe.AspNet.Validations;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     AddBankCardRequest.
    /// </summary>
    public class AddBankCardRequest : IRequest
    {
        /// <summary>
        ///     银行卡号，15到19位
        /// </summary>
        [Required, RegularExpression(@"^\d{15,19}$")]
        [JsonProperty("bankCardNo")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行名称：浦发银行,深发银行,平安银行,民生银行,工商银行,农业银行,建设银行,招商银行,广发银行,广州银行,邮储银行,兴业银行,光大银行,华夏银行,中信银行,广州农商行,中国银行,富滇银行
        /// </summary>
        [Required, AvailableValues("浦发银行", "深发银行", "平安银行", "民生银行", "工商银行", "农业银行", "建设银行", "招商银行", "广发银行", "广州银行", "邮储银行", "兴业银行", "光大银行", "华夏银行", "中信银行", "广州农商行", "中国银行", "富滇银行")]
        [JsonProperty("bankName")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string BankName { get; set; }

        /// <summary>
        ///     开户城市，必须是 省份|城市 的格式
        /// </summary>
        [Required, RegularExpression(@"^.+\|.+$"), JsonProperty("cityName")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string CityName { get; set; }
    }
}