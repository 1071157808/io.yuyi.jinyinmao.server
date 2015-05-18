// FileInformation: nyanya/Infrastructure.Gateway.OldCat/OldPlatformGateway.cs
// CreatedTime: 2014/08/29   11:38 AM
// LastUpdatedTime: 2014/09/01   2:17 PM

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.EL.TransientFaultHandling;
using Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy;
using Infrastructure.Lib.Disposal;
using Infrastructure.Lib.Extensions;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Infrastructure.Gateway.OldCat
{
    /// <summary>
    /// </summary>
    public class OldPlatformGateway : DisposableObject, IOldPlatformGateway
    {
        /// <summary>
        ///     The query authentication request with parame
        /// </summary>
        private const string QueryAuthRequestWithParame = "{0}?user_uuid={1}&token={2}&md5_value={3}";

        /// <summary>
        ///     The old platform gateway host
        /// </summary>
        private static readonly string OldPlatformGatewayHost;

        /// <summary>
        ///     The old platform login URL
        /// </summary>
        private static readonly string OldPlatformLoginUrl;

        /// <summary>
        ///     The retry policy
        /// </summary>
        private static readonly RetryPolicy RetryPolicy;

        /// <summary>
        ///     The client
        /// </summary>
        private readonly Lazy<HttpClient> client;

        /// <summary>
        ///     Initializes the <see cref="OldPlatformGateway" /> class.
        /// </summary>
        static OldPlatformGateway()
        {
            OldPlatformLoginUrl = "/passport/api/v1/users/auto_sign_in.json";
            //OldPlatformGatewayHost = "http://10.1.9.15:6010";//
            OldPlatformGatewayHost = "https://api.jinyinmao.com.cn";
            RetryPolicy = new RetryPolicy<HttpTransientErrorDetectionStrategy>(RetryStrategyFactory.GetHttpRetryPolicy());
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OldPlatformGateway" /> class.
        /// </summary>
        public OldPlatformGateway()
        {
            this.client = new Lazy<HttpClient>(this.InitHttpClient);
        }

        /// <summary>
        ///     Gets the client.
        /// </summary>
        /// <value>
        ///     The client.
        /// </value>
        private HttpClient Client
        {
            get { return this.client.Value; }
        }

        #region IOldPlatformGateway Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or
        ///     resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            this.Client.Dispose();
            base.Dispose();
        }

        /// <summary>
        ///     Users the login request asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        public async Task UserLoginRequestAsync(OldPlatformParameter parameter)
        {
            string url = QueryAuthRequestWithParame.FormatWith(OldPlatformLoginUrl, parameter.UserIdentifier, parameter.AmpAuthToken, parameter.CheckCode);
            await RetryPolicy.ExecuteAsync(() => this.Client.GetAsync(url));
        }

        #endregion IOldPlatformGateway Members

        /// <summary>
        ///     Initializes the HTTP client.
        /// </summary>
        /// <returns></returns>
        private HttpClient InitHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(OldPlatformGatewayHost);
            //            httpClient.DefaultRequestHeaders.Accept.Clear();
            //            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            httpClient.Timeout = new TimeSpan(0, 0, 10, 0);
            return httpClient;
        }
    }
}