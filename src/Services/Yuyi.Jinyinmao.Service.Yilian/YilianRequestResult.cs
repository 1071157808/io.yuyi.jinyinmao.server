// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  6:19 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  6:20 PM
// ***********************************************************************
// <copyright file="YilianRequestResult.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     YilianRequestResult.
    /// </summary>
    public class YilianRequestResult
    {
        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the response string.
        /// </summary>
        /// <value>The response string.</value>
        public string ResponseString { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="YilianRequestResult" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }
    }
}
