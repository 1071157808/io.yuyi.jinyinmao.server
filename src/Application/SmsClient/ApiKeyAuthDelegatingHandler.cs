// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  7:18 PM
// ***********************************************************************
// <copyright file="ApiKeyAuthDelegatingHandler.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
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

namespace SmsClient
{
    /// <summary>
    ///     ApiKeyAuthDelegatingHandler.
    /// </summary>
    public class ApiKeyAuthDelegatingHandler : DelegatingHandler
    {
        /// <summary>
        ///     send as an asynchronous operation.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string appIdConfig = ConfigurationManager.AppSettings.Get("SmsServiceAppId");
            string appId = string.IsNullOrEmpty(appIdConfig) ? "38e9ad41-c1b2-44a0-ad2a-88acba74db9d" : appIdConfig;
            string apiKeyConfig = ConfigurationManager.AppSettings.Get("SmsServiceApiKey");
            string apiKey = string.IsNullOrEmpty(apiKeyConfig) ? "HbX+NpcfkW3oSYRkYKa35dw8CiNEx+bg+4lGRiYYsRUV5YP6sWJ031DYaMS1jgSTOYF8W4gQ+B14oZzJYU1lpxLQCpjBuct299omchoSENoXHEIn7CUxO1i0kbD8FF5f98fZhKCAq4xUHJVpakMkByfoc1MkHcq7GFw45EiwqketEuCZTWx4DLxLh6GyPWD0M5xqtVhVwM9bunnK1R2mcucW8vdONsTKHU5IC9uejom/xMOywS/WkdDDAfKMM6MHuT6nsDD3BMf9/kvjuErei175AQrlmxzLIsEP1qHmhm56bRLTZHAq9NlBvQ64T2pnKlocqF528G1xJnRCZcHAgQ==" : apiKeyConfig;

            string requestContentBase64String = string.Empty;

            string requestUri = HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());

            string requestHttpMethod = request.Method.Method;

            //Calculate UNIX time
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

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
            string signatureRawData = $"{appId}{requestHttpMethod}{requestUri}{requestTimeStamp}{nonce}{requestContentBase64String}";

            byte[] secretKeyByteArray = Convert.FromBase64String(apiKey);

            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                //Setting the values in the Authorization header using custom scheme (jas)
                request.Headers.Authorization = new AuthenticationHeaderValue("jas", $"{appId}:{requestSignatureBase64String}:{nonce}:{requestTimeStamp}");
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}