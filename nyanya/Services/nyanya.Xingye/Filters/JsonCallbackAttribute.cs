using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using nyanya.AspDotNet.Common.Filters;

namespace nyanya.Xingye.Filters
{
    /// <summary>
    /// Jsonp支持
    /// </summary>
    public class JsonpCallbackAttribute : OrderedActionFilterAttribute
    {
        private const string CallbackQueryParameter = "callback";

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            string callback;

            if (IsJsonp(out callback))
            {
                var jsonBuilder = new StringBuilder(callback);
                var contentResult = context.Response.Content.ReadAsStringAsync().Result;
                //此处屏蔽可能会多次调用导致调用结果不正确
                if (!contentResult.Contains(callback))
                {
                    jsonBuilder.AppendFormat("({0})", contentResult);
                    context.Response.Content = new StringContent(jsonBuilder.ToString());
                }
            }

            base.OnActionExecuted(context);
        }

        private bool IsJsonp(out string callback)
        {
            callback = HttpContext.Current.Request.QueryString[CallbackQueryParameter];

            return !string.IsNullOrEmpty(callback);
        }
    }
}