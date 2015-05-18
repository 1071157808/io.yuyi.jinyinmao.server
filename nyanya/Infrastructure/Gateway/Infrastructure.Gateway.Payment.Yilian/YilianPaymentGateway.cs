// FileInformation: nyanya/Infrastructure.Gateway.Payment.Yilian/YilianPaymentGateway.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/09/24   10:09 AM

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Infrastructure.EL.TransientFaultHandling;
using Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy;
using Infrastructure.Lib;
using Infrastructure.Lib.Disposal;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Logs.Implementation;
using Infrastructure.SMS;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Infrastructure.Cache.Couchbase;

namespace Infrastructure.Gateway.Payment.Yilian
{
    public class YilianPaymentGatewayService : DisposableObject, IYilianPaymentGatewayService
    {
        private const string QueryAuthRequestWithParameters = "{0}?p={{BATCH_NO:\"{1}\"}}";
        private static readonly NLogger Logger = new NLogger("YilianPaymentGatewayServiceLogger");
        private static readonly string PaymentGatewayHost;
        private static readonly string PaymentRequestReturnUrl;
        private static readonly string PaymentRequestUrl;
        private static readonly string QueryAuthRequestUrl;
        private static readonly RetryPolicy RetryPolicy;
        private static readonly ISmsAlertsService SmsAlertsService;
        private static readonly string UserAuthRequestReturnUrl;
        private static readonly string UserAuthRequestUrl;
        private readonly Lazy<HttpClient> client;

        static YilianPaymentGatewayService()
        {
            PaymentGatewayHost = ConfigurationManager.AppSettings.Get("YilianPaymentGatewayHost");
            UserAuthRequestUrl = ConfigurationManager.AppSettings.Get("YilianUserAuthRequestUrl");
            QueryAuthRequestUrl = ConfigurationManager.AppSettings.Get("YilianQueryAuthRequestUrl");

            UserAuthRequestReturnUrl = ConfigurationManager.AppSettings.Get("YilianUserAuthRequestReturnUrl");
            PaymentRequestUrl = ConfigurationManager.AppSettings.Get("YilianPaymentRequestUrl");
            PaymentRequestReturnUrl = ConfigurationManager.AppSettings.Get("YilianPaymentRequestReturnUrl");
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(RetryStrategyFactory.GetHttpRetryPolicy());
            SmsAlertsService = new SmsService();
        }

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
        ///     Performs application-defined tasks associated with freeing, releasing, or
        ///     resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            this.Client.Dispose();
            base.Dispose();
        }

        public async Task<PaymentRequestResult> PaymentRequestAsync(PaymentRequestParameter parameter)
        {
            string json = "";
            string message = "支付失败";
            string responseString = "";
            try
            {
                parameter.TRANS_DETAILS.First().MERCHANT_URL = PaymentRequestReturnUrl;
                json = JsonConvert.SerializeObject(parameter);
                Logger.Info(JsonConvert.SerializeObject(json));

                Dictionary<string, string> parameters = new Dictionary<string, string> { { "p", JsonConvert.SerializeObject(parameter) } };
                HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(PaymentRequestUrl, new FormUrlEncodedContent(parameters)));
                responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Logger.Info(response.StatusCode + " " + response.ReasonPhrase + "\n" + json + "\n" + responseString);
                    bool success = JObject.Parse(responseString).SelectToken("success").Value<bool>();
                    if (success)
                        return new PaymentRequestResult
                        {
                            Result = true,
                            Message = message,
                            ResponseString = responseString
                        };
                }

                Logger.Fatal(response.StatusCode + " " + response.ReasonPhrase + "\n" + json + "\n" + responseString);
            }
            catch (Exception e)
            {
                message = e.Message;
                Logger.Fatal(e, NyanyaResources.YilianGateway_RequestSendingFailed.FormatWith(json));
            }

            await SmsAlertsService.AlertAsync(NyanyaResources.Alert_YilianGateway_RequestSendingFailed.FormatWith(json));
            return new PaymentRequestResult { Message = message, Result = false, ResponseString = responseString };
        }

        public async Task<RequestQueryResult> QueryRequestAsync(string batchNo, bool isPayment)
        {
            string json = "";
            try
            {
                Logger.Info(batchNo);

                string url = QueryAuthRequestWithParameters.FormatWith(QueryAuthRequestUrl, batchNo);
                HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => this.Client.GetAsync(url));
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Logger.Info(response.StatusCode + " " + response.ReasonPhrase + "\n" + json + "\n" + responseString);
                    JObject resultJObjects = this.ValidateJsonSchema(responseString);
                    JArray jArray = resultJObjects.SelectToken("TRANS_DETAILS").Value<JArray>();
                    string result = jArray.First.SelectToken("PAY_STATE").Value<string>();
                    string message = jArray.First.SelectToken("REMARK").Value<string>();

                    // 易联很坑爹，现在这样写，00A4 需要忽略
                    if (result.ToUpper() == "00A4")
                    {
                        Logger.Warning("00A4 {0}".FmtWith(batchNo));
                        return null;
                    }

                    bool queryResult = result == "0000" || (!isPayment && (result == "0051" || result.ToUpper() == "T425" || result.ToUpper() == "T212" || result.ToUpper() == "U011"));

                    if (isPayment)
                    {
                    }

                    return new RequestQueryResult
                    {
                        Result = queryResult,
                        Message =  message,
                        ResponseString = responseString
                    };
                }

                Logger.Fatal(response.StatusCode + " " + response.ReasonPhrase + "\n" + json + "\n" + responseString);
            }
            catch (Exception e)
            {
                //message = e.Message;
                Logger.Fatal(e, NyanyaResources.YilianGateway_RequestSendingFailed.FormatWith(json));
            }

            await SmsAlertsService.AlertAsync(NyanyaResources.Alert_YilianGateway_RequestSendingFailed.FormatWith(json));
            //return new RequestQueryResult { Message = message, Result = false, ResponseString = responseString };
            return null;
        }

        public async Task<UserAuthRequestResult> UserAuthRequestAsync(UserAuthRequestParameter parameter)
        {
            string json = "";
            string message = "ok";
            string responseString = "";
            try
            {
                parameter.TRANS_DETAILS.First().MERCHANT_URL = UserAuthRequestReturnUrl;
                json = JsonConvert.SerializeObject(parameter);
                Logger.Info(JsonConvert.SerializeObject(json));

                Dictionary<string, string> parameters = new Dictionary<string, string> { { "p", JsonConvert.SerializeObject(parameter) } };
                HttpResponseMessage response = await RetryPolicy.ExecuteAsync(() => this.Client.PostAsync(UserAuthRequestUrl, new FormUrlEncodedContent(parameters)));
                responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Logger.Info(response.StatusCode + " " + response.ReasonPhrase + "\n" + json + "\n" + responseString);
                    bool success = JObject.Parse(responseString).SelectToken("success").Value<bool>();
                    if (success)
                        return new UserAuthRequestResult
                        {
                            Result = true,
                            Message = message,
                            ResponseString = responseString
                        };
                }

                Logger.Fatal(response.StatusCode + " " + response.ReasonPhrase + "\n" + json + "\n" + responseString);
            }
            catch (Exception e)
            {
                message = e.Message;
                Logger.Fatal(e, NyanyaResources.YilianGateway_RequestSendingFailed.FormatWith(json));
            }

            await SmsAlertsService.AlertAsync(NyanyaResources.Alert_YilianGateway_RequestSendingFailed.FormatWith(json));
            return new UserAuthRequestResult { Message = message, Result = false, ResponseString = responseString };
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

            if (responseString.IsNullOrEmpty())
            {
                throw new BusinessValidationFailedException("responseString is empty.");
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
                throw new BusinessValidationFailedException("YilianAuthRequestCallbackReceived json bad format");
            }
            return reply;
        }
    }
}
