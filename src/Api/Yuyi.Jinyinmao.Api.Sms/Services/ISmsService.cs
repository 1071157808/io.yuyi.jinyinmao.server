// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-28  12:40 PM
// ***********************************************************************
// <copyright file="ISmsService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Api.Sms.Services
{
    /// <summary>
    ///     Interface ISmsService
    /// </summary>
    internal interface ISmsService
    {
        /// <summary>
        ///     Gets the balance asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Nullable&lt;System.Int32&gt;&gt;.</returns>
        Task<int?> GetBalanceAsync();

        /// <summary>
        ///     Sends the message asynchronous.
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="cellphones">The cellphones.</param>
        /// <param name="message">The message.</param>
        /// <param name="signature">The signature.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task SendMessageAsync(string appId, string cellphones, string message, string signature);
    }
}
