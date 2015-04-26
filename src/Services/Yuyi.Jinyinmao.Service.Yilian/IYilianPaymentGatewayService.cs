// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  6:18 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  10:27 PM
// ***********************************************************************
// <copyright file="IYilianPaymentGatewayService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     Interface IYilianPaymentGatewayService
    /// </summary>
    public interface IYilianPaymentGatewayService
    {
        /// <summary>
        ///     Users the authentication request asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;YilianRequestResult&gt;.</returns>
        Task<YilianRequestResult> AuthRequestAsync(AuthRequestParameter parameter);

        /// <summary>
        ///     Payments the request asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;YilianRequestResult&gt;.</returns>
        Task<YilianRequestResult> PaymentRequestAsync(PaymentRequestParameter parameter);

        /// <summary>
        ///     Queries the request asynchronous.
        /// </summary>
        /// <param name="batchNo">The batch no.</param>
        /// <param name="isPayment">if set to <c>true</c> [is payment].</param>
        /// <returns>Task&lt;YilianRequestResult&gt;.</returns>
        Task<YilianRequestResult> QueryRequestAsync(string batchNo, bool isPayment);
    }
}
