// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-06-05  12:49 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-05  1:01 AM
// ***********************************************************************
// <copyright file="AddExtraInterestRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models.BackOffice
{
    /// <summary>
    ///     AddExtraInterestRequest.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class AddExtraInterestRequest : IRequest
    {
        /// <summary>
        ///     该操作的描述信息，该值必填，会显示给前端用户，5到200字符
        /// </summary>
        [Required, StringLength(200, MinimumLength = 5), JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///     额外的收益金额，以“分”为单位，如果该值大于0，则会直接增加到额外收益上
        /// </summary>
        [Required, Range(0, int.MaxValue), JsonProperty("extraInterest")]
        public int ExtraInterest { get; set; }

        /// <summary>
        ///     额外的本金，以“分”为单位，如果该值大于0，则会使用该订单的相关信息计算出额外收益增加到订单的额外收益上
        /// </summary>
        [Required, Range(0, int.MaxValue), JsonProperty("extraPrincipal")]
        public int ExtraPrincipal { get; set; }

        /// <summary>
        ///     操作唯一标识，防止重试造成多次添加收益
        /// </summary>
        [Required, StringLength(32, MinimumLength = 32), JsonProperty("operationIdentifier")]
        public string OperationIdentifier { get; set; }

        /// <summary>
        ///     关联订单唯一标识，该订单必须是该用户的订单
        /// </summary>
        [Required, StringLength(32, MinimumLength = 32), JsonProperty("orderIdentifier")]
        public string OrderIdentifier { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        [Required, StringLength(32, MinimumLength = 32), JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}