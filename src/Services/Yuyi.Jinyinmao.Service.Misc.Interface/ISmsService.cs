// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  4:26 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  5:07 PM
// ***********************************************************************
// <copyright file="ISmsService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Interface ISmsService
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        ///     Gets the balance asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> GetBalanceAsync();

        /// <summary>
        ///     Sends the message asynchronous.
        /// </summary>
        /// <param name="cellphones">The cellphones.</param>
        /// <param name="message">The message.</param>
        /// <param name="signature">The signature.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> SendMessageAsync(string cellphones, string message, string signature = "金银猫");
    }
}
