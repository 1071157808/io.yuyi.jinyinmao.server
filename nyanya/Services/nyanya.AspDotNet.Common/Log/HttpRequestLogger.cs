// FileInformation: nyanya/nyanya.AspDotNet.Common/HttpRequestLogger.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:14 AM

using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using Infrastructure.Lib.Logs.Implementation;
using Infrastructure.Lib.Utility;

namespace nyanya.AspDotNet.Common.Log
{
    public class HttpRequestLogger : NLogger
    {
        private readonly DirectoryInfo requestDirectory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpRequestLogger" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public HttpRequestLogger(string name)
            : base(name)
        {
            DirectoryInfo directory = new DirectoryInfo(ConfigurationManager.AppSettings["CallbackRequests"]);
            if (!directory.Exists)
            {
                directory.Create();
            }
            this.requestDirectory = directory;
        }

        public void Log(HttpRequestMessage request, string message)
        {
            this.LogToFile(request, message);
        }

        private void LogToFile(HttpRequestMessage request, string message)
        {
            try
            {
                HttpContext httpContext = HttpUtils.GetHttpContext(request);
                if (httpContext == null)
                {
                    return;
                }

                string requestid = GuidUtils.NewGuidString();
                string path = Path.Combine(this.requestDirectory.FullName, requestid + ".request");
                try
                {
                    httpContext.Request.SaveAs(path, true);
                }
                catch (HttpException e)
                {
                    this.Fatal("NlogTraceExceptionLoggerError_HttpException" + e.Message, e);
                }

                StringBuilder messageBuilder = new StringBuilder();

                messageBuilder.Append(requestid);
                messageBuilder.Append("|" + message);

                this.Info(messageBuilder.ToString());
            }
            catch (Exception e)
            {
                this.Fatal("NlogTraceExceptionLoggerError" + e.Message, e);
            }
        }
    }
}