using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yuyi.Jinyinmao.Packages;

namespace Yuyi.Jinyinmao.Service.XianFeng
{
    public class XianFengPaymentGatewayService : IXianFengPaymentGatewayService
    {

        private static readonly string PaymentGatewayHost;
        private static readonly string PrePaymentRequestUrl;
        private static readonly string PaymentRequestUrl;
        private static readonly string QueryBankListUrl;
        private static readonly string RetrySendMsgUrl;
        private static readonly string QueryOrderUrl;
        private static readonly RetryPolicy RetryPolicy;
        private readonly Lazy<HttpClient> client;

        static XianFengPaymentGatewayService()
        {
            PaymentGatewayHost = CloudConfigurationManager.GetSetting("XianFengPaymentGatewayHost");
            PrePaymentRequestUrl = "/payment/payservice/globalPrepay.do";
            PaymentRequestUrl = "/payment/payservice/globalpay.do";
            QueryBankListUrl = "/payment/payservice/globalquerybanklist.do";
            RetrySendMsgUrl = "/payment/payservice/globalverify.do";
            QueryOrderUrl = "/payment/payservice/globalqueryorder.do";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(20, TimeSpan.FromSeconds(2));
        }


        public XianFengPaymentGatewayService()
        {
            this.client = new Lazy<HttpClient>(this.InitHttpClient);
        }

        private HttpClient InitHttpClient()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(PaymentGatewayHost) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            httpClient.Timeout = new TimeSpan(0, 0, 10, 0);
            return httpClient;
        }


        public HttpClient Client => this.client.Value;

        /// <summary>
        /// Pres the payment payment request asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;RequestParameter&gt;.</returns>
        public async Task<XianFengRequestResult> PrePaymentPaymentRequestAsync(RequestParameter parameter)
        {
            string message = "ok";
            string responseString = "";
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(parameter));
                HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(PrePaymentRequestUrl, new ByteArrayContent(bytes)));
                responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    bool success = JObject.Parse(responseString).SelectToken("SUCCESS").Value<bool>();
                    if (success)
                    {
                        return new XianFengRequestResult
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
                message = e.GetExceptionString();
            }

            return new XianFengRequestResult { Message = message, Result = false, ResponseString = responseString };
        }

        /// <summary>
        /// Payments the payment request asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;RequestParameter&gt;.</returns>
        public async Task<XianFengRequestResult> PaymentPaymentRequestAsync(RequestParameter parameter)
        {
            string message = "ok";
            string responseString = "";
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(parameter));
                HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(PaymentRequestUrl, new ByteArrayContent(bytes)));
                responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    bool success = JObject.Parse(responseString).SelectToken("success").Value<bool>();
                    if (success)
                    {
                        return new XianFengRequestResult
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
                message = e.GetExceptionString();
            }

            return new XianFengRequestResult { Message = message, Result = false, ResponseString = responseString };
        }

        /// <summary>
        /// Gets the banks asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;RequestParameter&gt;.</returns>
        public async Task<XianFengRequestResult> GetBanksAsync(RequestParameter parameter)
        {
            string message = "ok";
            string responseString = "";
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(parameter));
                HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(QueryBankListUrl, new ByteArrayContent(bytes)));
                responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    bool success = JObject.Parse(responseString).SelectToken("success").Value<bool>();
                    if (success)
                    {
                        return new XianFengRequestResult
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
                message = e.GetExceptionString();
            }

            return new XianFengRequestResult { Message = message, Result = false, ResponseString = responseString };
        }

        /// <summary>
        /// Retries the send MSG asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task&lt;RequestParameter&gt;.</returns>
        public async Task<XianFengRequestResult> RetrySendMsgAsync(RequestParameter parameter)
        {
            string message = "ok";
            string responseString = "";
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(parameter));
                HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(RetrySendMsgUrl, new ByteArrayContent(bytes)));
                responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    bool success = JObject.Parse(responseString).SelectToken("success").Value<bool>();
                    if (success)
                    {
                        return new XianFengRequestResult
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
                message = e.GetExceptionString();
            }

            return new XianFengRequestResult { Message = message, Result = false, ResponseString = responseString };
        }

        /// <summary>
        /// Queries the request asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>System.Threading.Tasks.Task&lt;Yuyi.Jinyinmao.Service.XianFeng.XianFengRequestResult&gt;.</returns>
        public async Task<XianFengRequestResult> QueryRequestAsync(RequestParameter parameter)
        {

            string message = "ok";
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(parameter));
                HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(QueryOrderUrl, new ByteArrayContent(bytes)));
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    bool success = JObject.Parse(responseString).SelectToken("success").Value<bool>();
                    if (success)
                    {
                        return new XianFengRequestResult
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
                message = e.GetExceptionString();
            }
            return null;
        }
    }
}
