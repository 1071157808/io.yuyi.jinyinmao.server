// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  5:39 PM
// ***********************************************************************
// <copyright file="YilianPaymentGatewayService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Yuyi.Jinyinmao.Packages;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     YilianPaymentGatewayService.
    /// </summary>
    public sealed class YilianPaymentGatewayService : IYilianPaymentGatewayService
    {
        private const string QueryAuthRequestWithParameters = "{0}?p={{BATCH_NO:\"{1}\"}}";
        private static readonly string PaymentGatewayHost;
        private static readonly string PaymentRequestReturnUrl;
        private static readonly string PaymentRequestUrl;
        private static readonly string QueryRequestUrl;
        private static readonly RetryPolicy RetryPolicy;
        private static readonly string UserAuthRequestReturnUrl;
        private static readonly string UserAuthRequestUrl;
        private readonly Lazy<HttpClient> client;

        static YilianPaymentGatewayService()
        {
            PaymentGatewayHost = ConfigurationManager.AppSettings.Get("YilianPaymentGatewayHost");
            UserAuthRequestUrl = "/paycore/services/userAuthRequestService";
            PaymentRequestUrl = "/paycore/services/easyLinkGatherRequestService";
            QueryRequestUrl = "/paycore/services/easyLinkGatherQueryRequestService";

            UserAuthRequestReturnUrl = "https://api.jinyinmao.com.cn/";
            PaymentRequestReturnUrl = "https://api.jinyinmao.com.cn/";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(5, TimeSpan.FromSeconds(2));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianPaymentGatewayService" /> class.
        /// </summary>
        public YilianPaymentGatewayService()
        {
            this.client = new Lazy<HttpClient>(this.InitHttpClient);
        }

        private HttpClient Client
        {
            get { return this.client.Value; }
        }

        #region IYilianPaymentGatewayService Members

        /// <summary>
        ///     user authentication request as an asynchronous operation.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;YilianRequestResult&gt;.</returns>
        public async Task<YilianRequestResult> AuthRequestAsync(AuthRequestParameter parameter)
        {
            string message = "ok";
            string responseString = "";
            try
            {
                parameter.TRANS_DETAILS.First().MERCHANT_URL = UserAuthRequestReturnUrl;
                JsonConvert.SerializeObject(parameter);

                Dictionary<string, string> parameters = new Dictionary<string, string> { { "p", JsonConvert.SerializeObject(parameter) } };
                HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(UserAuthRequestUrl, new FormUrlEncodedContent(parameters)));
                responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    bool success = JObject.Parse(responseString).SelectToken("success").Value<bool>();
                    if (success)
                    {
                        return new YilianRequestResult
                        {
                            Result = true,
                            Message = message,
                            ResponseString = responseString
                        };
                    }
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return new YilianRequestResult { Message = message, Result = false, ResponseString = responseString };
        }

        /// <summary>
        ///     payment request as an asynchronous operation.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;YilianRequestResult&gt;.</returns>
        public async Task<YilianRequestResult> PaymentRequestAsync(PaymentRequestParameter parameter)
        {
            string message = "支付失败";
            string responseString = "";
            try
            {
                parameter.TRANS_DETAILS.First().MERCHANT_URL = PaymentRequestReturnUrl;
                JsonConvert.SerializeObject(parameter);

                Dictionary<string, string> parameters = new Dictionary<string, string> { { "p", JsonConvert.SerializeObject(parameter) } };
                HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(PaymentRequestUrl, new FormUrlEncodedContent(parameters)));
                responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    bool success = JObject.Parse(responseString).SelectToken("success").Value<bool>();
                    if (success)
                    {
                        return new YilianRequestResult
                        {
                            Result = true,
                            Message = "Success",
                            ResponseString = responseString
                        };
                    }
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return new YilianRequestResult { Message = message, Result = false, ResponseString = responseString };
        }

        /// <summary>
        ///     query request as an asynchronous operation.
        /// </summary>
        /// <param name="batchNo">The batch no.</param>
        /// <param name="isPayment">if set to <c>true</c> [is payment].</param>
        /// <returns>Task&lt;YilianRequestResult&gt;.</returns>
        public async Task<YilianRequestResult> QueryRequestAsync(string batchNo, bool isPayment)
        {
            try
            {
                string url = string.Format(QueryAuthRequestWithParameters, QueryRequestUrl, batchNo);
                HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => this.Client.GetAsync(url));
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    JObject resultJObjects = this.ValidateJsonSchema(responseString);
                    JArray jArray = resultJObjects.SelectToken("TRANS_DETAILS").Value<JArray>();
                    string result = jArray.First.SelectToken("PAY_STATE").Value<string>();
                    string message = jArray.First.SelectToken("REMARK").Value<string>();

                    // 易联很坑爹，现在这样写，00A4 需要忽略
                    if (result.ToUpper() == "00A4")
                    {
                        return null;
                    }

                    bool queryResult = result == "0000" || (!isPayment && (result == "0051" || result.ToUpper() == "T425" || result.ToUpper() == "T212" || result.ToUpper() == "U011"));

                    return new YilianRequestResult
                    {
                        Result = queryResult,
                        Message = message,
                        ResponseString = responseString
                    };
                }
            }
            catch
            {
            }

            return null;
        }

        #endregion IYilianPaymentGatewayService Members

        /// <summary>
        ///     Initializes the HTTP client.
        /// </summary>
        /// <returns></returns>
        private HttpClient InitHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(PaymentGatewayHost);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            httpClient.Timeout = new TimeSpan(0, 0, 10, 0);
            return httpClient;
        }

        private JObject ValidateJsonSchema(string responseString)
        {
            //{"BATCH_NO":"ACBIFK45564783","MSG_SIGN":"","MSG_TYPE":"200002","TRANS_DETAILS":[{"ACC_CITY":"","ACC_NAME":"","ACC_NO":"","ACC_PROP":"0","ACC_PROVINCE":""
            //,"ACC_TYPE":"00","AMOUNT":"0.00","BANK_CODE":"","BANK_NAME":"","BANK_NO":"","CNY":"CNY","EXCHANGE_RATE":"","ID_NO":"","ID_TYPE":"0"
            //,"MER_ORDER_NO":"","MOBILE_NO":"","PAY_STATE":"T206","QUERY_NO_FLAG":"","REMARK":"无此交易记录！","RESERVE":"","RETURN_URL":"","SETT_AMOUNT":""
            //,"SETT_DATE":"","SN":"","TRANS_DESC":"","USER_LEVEL":""}],"TRANS_STATE":"0000","USER_NAME":"13760136514","VERSION":"2.0"}

            if (string.IsNullOrEmpty(responseString))
            {
                return null;
            }

            string schemaString = @"{
            'description': 'Yilian QueryAuth Callback',
            'type': 'object',
            'properties':
            {
                'BATCH_NO':{'type':'string'},
                'MSG_SIGN':{'type':'string'},
                'MSG_TYPE':{'type':'string'},

                'TRANS_DETAILS':{'type':'array',
                    'items':{'type': 'object',
                             'properties':
                                {
                                    'ACC_CITY':{'type':'string'},
                                    'ACC_NAME':{'type':'string'},
                                    'ACC_NO':{'type':'string'},
                                    'ACC_PROP':{'type':'string'},
                                    'ACC_PROVINCE':{'type':'string'},
                                    'ACC_TYPE':{'type':'string'},
                                    'AMOUNT':{'type':'string'},
                                    'BANK_CODE':{'type':'string'},
                                    'BANK_NO':{'type':'string'},
                                    'CNY':{'type':'string'},
                                    'EXCHANGE_RATE':{'type':'string'},
                                    'ID_NO':{'type':'string'},
                                    'ID_TYPE':{'type':'string'},
                                    'MER_ORDER_NO':{'type':'string'},
                                    'MOBILE_NO':{'type':'string'},
                                    'PAY_STATE':{'type':'string'},
                                    'QUERY_NO_FLAG':{'type':'string'},
                                    'REMARK':{'type':'string'},
                                    'RESERVE':{'type':'string'},
                                    'RETURN_URL':{'type':'string'},
                                    'SETT_AMOUNT':{'type':'string'},
                                    'SETT_DATE':{'type':'string'},
                                    'SN':{'type':'string'},
                                    'TRANS_DESC':{'type':'string'},
                                    'USER_LEVEL':{'type':'string'}
                                }
                    }
                },
                'TRANS_STATE':{'type':'string'},
                'USER_NAME':{'typr':'string'},
                'VERSION':{'typr':'string'},
             }}";

            JsonSchema schema = JsonSchema.Parse(schemaString);

            JObject reply = JObject.Parse(responseString);

            if (!reply.IsValid(schema))
            {
                return null;
            }
            return reply;
        }
    }
}