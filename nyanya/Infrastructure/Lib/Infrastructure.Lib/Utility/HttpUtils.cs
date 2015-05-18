// FileInformation: nyanya/Infrastructure.Lib/HttpUtils.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/09/09   11:08 AM

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Infrastructure.Lib.Utility
{
    public static class HttpUtils
    {
        #region Private Fields

        private const string HttpContextBaseKey = "MS_HttpContext";

        #endregion Private Fields

        #region Public Methods

        public static void CopyRequestHeaders(IEnumerable<KeyValuePair<string, IEnumerable<string>>> source, HttpRequestHeaders destination)
        {
            foreach (KeyValuePair<string, IEnumerable<string>> header in source)
            {
                switch (header.Key)
                {
                    case "User-Agent":
                        destination.Add(header.Key, "nyanya/1.0");
                        break;

                    case "Via":
                        break;

                    default:
                        destination.Add(header.Key, String.Join(",", header.Value));
                        break;
                }
            }
        }

        public static HttpContext GetHttpContext(HttpRequestMessage request)
        {
            HttpContextBase contextBase = GetHttpContextBase(request);

            if (contextBase == null)
            {
                return null;
            }

            return ToHttpContext(contextBase);
        }

        public static string GetUserAgent(HttpRequestMessage request)
        {
            HttpContext context = GetHttpContext(request);
            return context == null ? "" : context.Request.UserAgent;
        }

        public static string GetUserHostAddress(HttpRequestMessage request)
        {
            HttpContext context = GetHttpContext(request);
            return context == null ? "" : context.Request.UserHostAddress;
        }

        public static string GetUserHostAddress(HttpContextBase contextBase)
        {
            HttpContext context = ToHttpContext(contextBase);
            return context == null ? "" : context.Request.UserHostAddress;
        }

        public static bool IsMobileDevice(HttpRequestMessage request)
        {
            HttpContext context = GetHttpContext(request);
            return context != null && (context.Request.Browser.IsMobileDevice || GetUserAgent(request).ToUpperInvariant().Contains("IOS")
                || GetUserAgent(request).ToUpperInvariant().Contains("IPAD") || GetUserAgent(request).ToUpperInvariant().Contains("IPHONE")
                || GetUserAgent(request).ToUpperInvariant().Contains("IPOD") || GetUserAgent(request).ToUpperInvariant().Contains("ANDROID"));
        }

        //        public static bool IsMobileDevice(HttpRequestMessage request)
        //        {
        //            HttpContext context = GetHttpContext(request);
        //
        //            string clientType = MobileDeviceValue(context, "Client-Type");
        //
        //            if (string.IsNullOrEmpty(clientType))
        //            {
        //                return false;
        //            }
        //            return true;
        //        }

        public static HttpResponseMessage RedirectTo(HttpRequestMessage request, string uri)
        {
            return RedirectTo(request, new Uri(uri));
        }

        #endregion Public Methods

        #region Private Methods

        public static bool IsDev(HttpContextBase httpContext)
        {
            string ip = GetUserHostAddress(httpContext);
            return !String.IsNullOrEmpty(ip) && ip.StartsWith("10.1");
        }

        public static bool IsIphone(HttpRequestMessage request)
        {
            HttpContext context = GetHttpContext(request);
            if (context != null)
            {
                string StrContext = context.Request.Browser.Browser.ToUpper();
                return StrContext.Contains("IPHONE") || StrContext.Contains("IPAD") || StrContext.Contains("IPOD");
            }
            return false;
        }

        public static bool IsLocalhost(HttpContextBase httpContext)
        {
            return httpContext.Request.IsLocal;
        }

        public static HttpContext ToHttpContext(HttpContextBase contextBase)
        {
            return contextBase.ApplicationInstance.Context;
        }

        private static HttpContextBase GetHttpContextBase(HttpRequestMessage request)
        {
            if (request == null)
            {
                return null;
            }

            object value;

            if (!request.Properties.TryGetValue(HttpContextBaseKey, out value))
            {
                return null;
            }

            return value as HttpContextBase;
        }

        private static string MobileDeviceValue(HttpContext context, string headerKey)
        {
            string headerValue = string.Empty;

            if (context.Request.Headers[headerKey] != null)
            {
                headerValue = context.Request.Headers[headerKey].ToString();
            }
            return headerKey;
        }

        private static HttpResponseMessage RedirectTo(HttpRequestMessage request, Uri uri)
        {
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = uri;
            return response;
        }

        #endregion Private Methods
    }
}