// FileInformation: nyanya/Infrastructure.Lib/HttpExtensions.cs
// CreatedTime: 2014/09/09   10:55 AM
// LastUpdatedTime: 2014/09/09   10:58 AM

using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Infrastructure.Lib.Utility;

namespace Infrastructure.Lib.Extensions
{
    public static class HttpExtensions
    {
        private const string HttpContextBaseKey = "MS_HttpContext";

        public static HttpContext GetHttpContext(this HttpRequestMessage request)
        {
            return HttpUtils.ToHttpContext(request.GetHttpContextBase());
        }

        public static HttpContext GetHttpContext(this ApiController apiController)
        {
            return HttpUtils.ToHttpContext(apiController.GetHttpContextBase());
        }

        public static HttpContextBase GetHttpContextBase(this HttpRequestMessage request)
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

        public static HttpContextBase GetHttpContextBase(this ApiController apiController)
        {
            return apiController.Request.GetHttpContextBase();
        }

        public static bool IsDev(this HttpContextBase httpContext)
        {
            return HttpUtils.IsDev(httpContext);
        }

        public static bool IsDev(this Controller controller)
        {
            return controller.HttpContext.IsDev();
        }

        public static bool IsDev(this ApiController apiController)
        {
            return apiController.Request.GetHttpContextBase().IsDev();
        }

        public static bool IsLocalhost(this HttpContextBase httpContext)
        {
            return HttpUtils.IsLocalhost(httpContext);
        }

        public static bool IsLocalhost(this Controller controller)
        {
            return controller.HttpContext.IsLocalhost();
        }

        public static bool IsLocalhost(this ApiController apiController)
        {
            return apiController.Request.GetHttpContextBase().IsLocalhost();
        }

        public static bool IsLocalhostOrDev(this Controller controller)
        {
            return controller.HttpContext.IsLocalhost() || controller.HttpContext.IsDev();
        }

        public static CookieCollection ToCookieCollection(this HttpCookieCollection cookies, string domain)
        {
            CookieCollection collection = new CookieCollection();
            for (int j = 0; j < cookies.Count; j++)
            {
                HttpCookie cookie = cookies.Get(j);
                Cookie oC = new Cookie();

                if (cookie != null)
                {
                    // Convert between the System.Net.Cookie to a System.Web.HttpCookie...
                    oC.Domain = domain;
                    oC.Expires = cookie.Expires;
                    oC.Name = cookie.Name;
                    oC.Path = cookie.Path;
                    oC.Secure = cookie.Secure;
                    oC.Value = cookie.Value;

                    collection.Add(oC);
                }
            }
            return collection;
        }
    }
}