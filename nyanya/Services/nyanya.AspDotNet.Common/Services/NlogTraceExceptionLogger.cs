// FileInformation: nyanya/nyanya.AspDotNet.Common/NlogTraceExceptionLogger.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:18 AM

using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Http.ExceptionHandling;
using Infrastructure.Lib.Utility;
using NLog;

namespace nyanya.AspDotNet.Common.Services.ExceptionLoggers
{
    public class NlogTraceExceptionLogger : ExceptionLogger
    {
        #region Private Fields

        private const string HttpContextBaseKey = "MS_HttpContext";

        private readonly Logger logger;
        private readonly DirectoryInfo requestDirectory;

        #endregion Private Fields

        #region Public Constructors

        public NlogTraceExceptionLogger()
        {
            this.logger = LogManager.GetLogger("GlobalExceptionLogger");
            if (this.logger == null)
            {
                throw new NLogConfigurationException("Can not find GlobalExceptionLogger");
            }

            DirectoryInfo directory = new DirectoryInfo(ConfigurationManager.AppSettings["RequestLogFile"]);
            if (!directory.Exists)
            {
                directory.Create();
            }
            this.requestDirectory = directory;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Log(ExceptionLoggerContext context)
        {
            try
            {
                // Retrieve the current HttpContext instance for this request.
                HttpContext httpContext = HttpUtils.GetHttpContext(context.Request);

                if (httpContext == null)
                {
                    return;
                }

                string requestid = Guid.NewGuid().ToString();
                string path = Path.Combine(this.requestDirectory.FullName, requestid + ".request");
                try
                {
                    httpContext.Request.SaveAs(path, true);
                }
                catch (HttpException e)
                {
                    this.logger.Fatal("NlogTraceExceptionLoggerError_HttpException" + e.Message, e);
                }

                StringBuilder messageBuilder = new StringBuilder();

                messageBuilder.Append(" " + requestid);
                messageBuilder.Append(" " + httpContext.Request.HttpMethod);
                messageBuilder.Append(" " + httpContext.Request.RawUrl);
                messageBuilder.Append(" " + httpContext.Response.StatusCode);
                messageBuilder.Append("\n");
                messageBuilder.Append(context.Exception.Message);
                messageBuilder.Append("\n");
                messageBuilder.Append(context.Exception.StackTrace);

                this.logger.Fatal(messageBuilder.ToString(), context.Exception);
            }
            catch (Exception e)
            {
                this.logger.Fatal("NlogTraceExceptionLoggerError" + e.Message, e);
            }
        }

        #endregion Public Methods
    }
}