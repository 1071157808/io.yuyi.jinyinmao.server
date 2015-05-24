// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-21  11:06 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-21  5:18 PM
// ***********************************************************************
// <copyright file="VerifyBankCardRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     VerifyBankCardRequest.
    /// </summary>
    public class VerifyBankCardRequest : IRequest
    {
        /// <summary>
        ///     银行卡号，15到19位
        /// </summary>
        [Required, StringLength(19, MinimumLength = 15), JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }
    }
}