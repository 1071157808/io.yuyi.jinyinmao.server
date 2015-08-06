// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SetJBYAccountTransactionResultRequest.cs
// Created          : 2015-08-05  11:04 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-05  11:04 PM
// ***********************************************************************
// <copyright file="SetJBYAccountTransactionResultRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Moe.AspNet.Models;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     SetJBYAccountTransactionResultRequest.
    /// </summary>
    public class SetJBYAccountTransactionResultRequest : IRequest
    {
        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="SetJBYAccountTransactionResultRequest" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        ///     Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        public string TransactionIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets the user identifeir.
        /// </summary>
        /// <value>The user identifeir.</value>
        public string UserIdentifier { get; set; }
    }
}