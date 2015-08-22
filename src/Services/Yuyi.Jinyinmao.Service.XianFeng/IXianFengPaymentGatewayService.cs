// ***********************************************************************
// Assembly         : Yuyi.Jinyinmao.Service.XianFeng
// Author           : wang.bing
// Created          : 08-21-2015
//
// Last Modified By : wang.bing
// Last Modified On : 08-21-2015
// ***********************************************************************
// <copyright file="IXianFengPaymentGatewayService.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Service.XianFeng
{
    /// <summary>
    /// Interface IXianFengPaymentGatewayService
    /// </summary>
    interface IXianFengPaymentGatewayService
    {

        /// <summary>
        /// Pres the payment payment request asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;RequestParameter&gt;.</returns>
        Task<XianFengRequestResult> PrePaymentPaymentRequestAsync(RequestParameter parameter);

        /// <summary>
        /// Payments the payment request asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;RequestParameter&gt;.</returns>
        Task<XianFengRequestResult> PaymentPaymentRequestAsync(RequestParameter parameter);

        /// <summary>
        /// Gets the banks asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;RequestParameter&gt;.</returns>
        Task<XianFengRequestResult> GetBanksAsync(RequestParameter parameter);

        /// <summary>
        /// Retries the send MSG asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;RequestParameter&gt;.</returns>
        Task<XianFengRequestResult> RetrySendMsgAsync(RequestParameter parameter);


        /// <summary>
        /// Queries the request asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;XianFengRequestResult&gt;.</returns>
        Task<XianFengRequestResult> QueryRequestAsync(RequestParameter parameter);

    }
}
