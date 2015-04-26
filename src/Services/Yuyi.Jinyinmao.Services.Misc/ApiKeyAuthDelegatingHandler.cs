// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-20  11:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-20  11:38 AM
// ***********************************************************************
// <copyright file="ApiKeyAuthDelegatingHandler.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     ApiKeyAuthDelegatingHandler.
    /// </summary>
    public class ApiKeyAuthDelegatingHandler : DelegatingHandler
    {
        /// <summary>
        ///     The application identifier
        /// </summary>
        private static readonly string AppId;

        /// <summary>
        ///     The API key
        /// </summary>
        private static readonly string AppKey;

        static ApiKeyAuthDelegatingHandler()
        {
            string appId = ConfigurationManager.AppSettings.Get("SmsServiceAppId");
            AppId = appId.IsNullOrEmpty() ? "541a74bc-cdf0-455d-9093-1aa5ec3cb7d3" : appId;
            string appKey = ConfigurationManager.AppSettings.Get("SmsServiceAppKey");
            AppKey = appKey.IsNullOrEmpty() ? "rNAhdng2Tu1iXpH72jU2zLSW/hhxuGBxpBNSBBjwQEA=" : appKey;
        }

        /// <summary>
        ///     send as an asynchronous operation.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            string requestContentBase64String = string.Empty;

            string requestUri = HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());

            string requestHttpMethod = request.Method.Method;

            //Calculate UNIX time
            string requestTimeStamp = DateTime.UtcNow.UnixTimeStamp().ToString();

            //create random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");

            //Checking if the request contains body, usually will be null with HTTP GET and DELETE
            if (request.Content != null)
            {
                byte[] content = await request.Content.ReadAsByteArrayAsync();
                MD5 md5 = MD5.Create();
                //Hashing the request body, any change in request body will result in different hash, we'll incure message integrity
                byte[] requestContentHash = md5.ComputeHash(content);
                requestContentBase64String = Convert.ToBase64String(requestContentHash);
            }

            //Creating the raw signature string
            string signatureRawData = String.Format("{0}{1}{2}{3}{4}{5}", AppId, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);

            var secretKeyByteArray = Convert.FromBase64String(AppKey);

            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                //Setting the values in the Authorization header using custom scheme (amx)
                request.Headers.Authorization = new AuthenticationHeaderValue("jas", string.Format("{0}:{1}:{2}:{3}", AppId, requestSignatureBase64String, nonce, requestTimeStamp));
            }

            response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
