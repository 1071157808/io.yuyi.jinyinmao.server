// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SetSettleAccountTransactionResultRequest.cs
// Created          : 2015-08-04  12:46 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-04  12:49 PM
// ***********************************************************************
// <copyright file="SetSettleAccountTransactionResultRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Moe.AspNet.Models;

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
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        ///     Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        public string TransactionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        public string TransDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user identifeir.
        /// </summary>
        /// <value>The user identifeir.</value>
        public string UserIdentifier { get; set; }
    }
}