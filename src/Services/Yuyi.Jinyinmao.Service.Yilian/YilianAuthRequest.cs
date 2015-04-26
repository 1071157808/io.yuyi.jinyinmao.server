// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  2:24 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  2:28 AM
// ***********************************************************************
// <copyright file="YilianRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     YilianRequest.
    /// </summary>
    public class YilianAuthRequest
    {
        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public decimal Amount { get; set; }

        /// <summary>
        ///     Gets or sets the sequence no.
        /// </summary>
        /// <value>The sequence no.</value>
        public string SequenceNo { get; set; }

        /// <summary>
        ///     Gets or sets the type code.
        /// </summary>
        /// <value>The type code.</value>
        public string TypeCode { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier { get; set; }
    }
}
