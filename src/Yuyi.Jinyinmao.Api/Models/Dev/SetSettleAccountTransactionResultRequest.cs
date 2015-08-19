// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SetSettleAccountTransactionResultRequest.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-19  18:37
// ***********************************************************************
// <copyright file="SetSettleAccountTransactionResultRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     SetSettleAccountTransactionResultRequest.
    /// </summary>
    public class SetSettleAccountTransactionResultRequest : IRequest
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="SetSettleAccountTransactionResultRequest" /> is result.
        /// </summary>
        [Required]
        [JsonProperty("result")]
        public bool Result { get; set; }

        /// <summary>
        ///     Gets or sets the transaction identifier.
        /// </summary>
        [Required]
        [StringLength(32, MinimumLength = 32)]
        [JsonProperty("transactionIdentifier")]
        public string TransactionIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets the trans desc.
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 2)]
        [JsonProperty("transDesc")]
        public string TransDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user identifeir.
        /// </summary>
        [Required]
        [StringLength(32, MinimumLength = 32)]
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}