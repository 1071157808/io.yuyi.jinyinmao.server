// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-25  3:46 PM
// ***********************************************************************
// <copyright file="AuthenticationRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.AspNet.Validations;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     AuthenticationRequest.
    /// </summary>
    public class AuthenticationRequest : IRequest
    {
        /// <summary>
        ///     银行卡号，15到19位
        /// </summary>
        [Required, StringLength(19, MinimumLength = 15), JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行名称：浦发银行,深发银行,平安银行,民生银行,工商银行,农业银行,建设银行,招商银行,广发银行,广州银行,邮储银行,兴业银行,光大银行,华夏银行,中信银行,广州农商行,中国银行,富滇银行
        /// </summary>
        [Required, JsonProperty("bankName"), AvailableValues("浦发银行", "深发银行", "平安银行", "民生银行", "工商银行", "农业银行", "建设银行", "招商银行", "广发银行", "广州银行", "邮储银行", "兴业银行", "光大银行", "华夏银行", "中信银行", "广州农商行", "中国银行", "富滇银行")]
        public string BankName { get; set; }

        /// <summary>
        ///     开户城市，必须是 省份|城市 的格式
        /// </summary>
        [Required, RegularExpression(@"^.+\|.+$"), JsonProperty("cityName"), AvailableValues("上海|上海", "广东|广州", "广东|深圳")]
        public string CityName { get; set; }

        /// <summary>
        ///     证件类型。10 => 身份证， 20 => 护照，30 => 台湾， 40=> 军官证
        /// </summary>
        [Required, JsonProperty("credential"), AvailableValues(Credential.IdCard, Credential.Junguan, Credential.Passport, Credential.Taiwan)]
        public Credential Credential { get; set; }

        /// <summary>
        ///     证件编号
        /// </summary>
        [Required, JsonProperty("credentialNo"), MinLength(5)]
        public string CredentialNo { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        [Required, JsonProperty("realName"), MinLength(2)]
        public string RealName { get; set; }
    }
}