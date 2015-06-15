// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-06-14  11:01 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  12:16 AM
// ***********************************************************************
// <copyright file="SeriLog.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Tracing;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Moe.AspNet.Utility;
using Moe.Lib;
using Serilog;
using Yuyi.Jinyinmao.Log;
using TraceLevel = System.Web.Http.Tracing.TraceLevel;

namespace Yuyi.Jinyinmao.Api.Log
{
    /// <summary>
    ///     HttpConfigurationExtensions.
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        ///     Uses the seri log trace writer.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseSeriLog(this HttpConfiguration config)
        {
            CloudStorageAccount storage = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            ILogger log = new LoggerConfiguration().WriteTo.AzureTableStorage(storage, "ApiLogs").CreateLogger();
            Serilog.Log.Logger = log;
            config.Services.Replace(typeof(ITraceWriter), new SeriLogTraceWriter());
            config.Services.Add(typeof(IExceptionLogger), new SeriExceptionLogger());
            return config;
        }
    }

    /// <summary>
    ///     ExceptionLogger.
    /// </summary>
    public class SeriExceptionLogger : ExceptionLogger
    {
        private readonly ILogger logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SeriLogTraceWriter" /> class.
        /// </summary>
        public SeriExceptionLogger()
        {
            this.logger = Serilog.Log.Logger;
        }

        /// <summary>
        ///     When overridden in a derived class, logs the exception synchronously.
        /// </summary>
        /// <param name="context">The exception logger context.</param>
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

                string request = httpContext.Request.Dump();

                StringBuilder messageBuilder = new StringBuilder();

                messageBuilder.Append(" " + httpContext.Request.HttpMethod);
                messageBuilder.Append(" " + httpContext.Request.RawUrl);
                messageBuilder.Append(" " + httpContext.Response.StatusCode);
                messageBuilder.Append("\r\n");
                messageBuilder.Append(request);
                messageBuilder.Append("\r\n");
                messageBuilder.Append(context.Exception.GetExceptionString());

                this.logger.Error(context.Exception, messageBuilder.ToString());

                Trace.TraceError(messageBuilder.ToString());
            }
            catch (Exception e)
            {
                Trace.TraceError("ExceptionLoggerInternalError" + e.Message);
            }
        }
    }

    /// <summary>
    ///     SeriLogTraceWriter.
    /// </summary>
    public class SeriLogTraceWriter : ITraceWriter
    {
        private readonly ILogger logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SeriLogTraceWriter" /> class.
        /// </summary>
        public SeriLogTraceWriter()
        {
            this.logger = Serilog.Log.Logger;
        }

        #region ITraceWriter Members

        /// <summary>
        ///     Invokes the specified traceAction to allow setting values in a new <see cref="T:System.Web.Http.Tracing.TraceRecord" /> if and only if tracing is permitted at the given category and level.
        /// </summary>
        /// <param name="request">The current <see cref="T:System.Net.Http.HttpRequestMessage" />.   It may be null but doing so will prevent subsequent trace analysis  from correlating the trace to a particular request.</param>
        /// <param name="category">The logical category for the trace.  Users can define their own.</param>
        /// <param name="level">The <see cref="T:System.Web.Http.Tracing.TraceLevel" /> at which to write this trace.</param>
        /// <param name="traceAction">The action to invoke if tracing is enabled.  The caller is expected to fill in the fields of the given <see cref="T:System.Web.Http.Tracing.TraceRecord" /> in this action.</param>
        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (level == TraceLevel.Off) return;

            TraceRecord record = new TraceRecord(request, category, level);

            traceAction(record);
            this.LogToSeri(record);
        }

        #endregion ITraceWriter Members

        private Action<Exception, string> GetExceptionLogger(TraceLevel level)
        {
            switch (level)
            {
                case TraceLevel.Off:
                    break;

                case TraceLevel.Fatal:
                case TraceLevel.Error:
                    return (e, s) => this.logger.Error(e, s);

                case TraceLevel.Warn:
                    return (e, s) => this.logger.Warning(e, s);

                case TraceLevel.Info:
                    return (e, s) => this.logger.Information(e, s);

                case TraceLevel.Debug:
                    return (e, s) => this.logger.Debug(e, s);
            }

            return (e, s) => { };
        }

        private Action<string> GetTraceLogger(TraceLevel level)
        {
            switch (level)
            {
                case TraceLevel.Off:
                    break;

                case TraceLevel.Fatal:
                case TraceLevel.Error:
                    return s =>
                    {
                        this.logger.Error(s);
                        System.Diagnostics.Trace.TraceError(s);
                    };

                case TraceLevel.Warn:
                    return s =>
                    {
                        this.logger.Warning(s);
                        System.Diagnostics.Trace.TraceWarning(s);
                    };

                case TraceLevel.Info:
                    return s =>
                    {
                        this.logger.Information(s);
                        System.Diagnostics.Trace.TraceInformation(s);
                    };

                case TraceLevel.Debug:
                    return s => this.logger.Debug(s);
            }

            return s => { };
        }

        private void LogToSeri(TraceRecord traceRecord)
        {
            StringBuilder messageBuilder = new StringBuilder();

            if (traceRecord.Request != null)
            {
                if (traceRecord.Request.Method != null)
                {
                    messageBuilder.Append(" " + traceRecord.Request.Method);
                }
                else
                {
                    messageBuilder.Append(" -");
                }

                if (traceRecord.Request.RequestUri != null)
                {
                    messageBuilder.Append(" " + traceRecord.Request.RequestUri);
                }
                else
                {
                    messageBuilder.Append(" -");
                }

                if (traceRecord.Request.Content != null)
                {
                    messageBuilder.Append(" " + traceRecord.Request.Content);
                }
                else
                {
                    messageBuilder.Append(" -");
                }
            }

            messageBuilder.Append(traceRecord.Status);

            if (!string.IsNullOrWhiteSpace(traceRecord.Category))
            {
                messageBuilder.Append(" " + traceRecord.Category);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (!string.IsNullOrWhiteSpace(traceRecord.Operator))
            {
                messageBuilder.Append(" " + traceRecord.Operator);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (!string.IsNullOrWhiteSpace(traceRecord.Operation))
            {
                messageBuilder.Append(" " + traceRecord.Operation);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (!string.IsNullOrWhiteSpace(traceRecord.Message))
            {
                messageBuilder.Append(" " + traceRecord.Message);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (traceRecord.Exception != null)
            {
                messageBuilder.Append("\r\n" + traceRecord.Exception.GetExceptionString());
            }

            string message = messageBuilder.ToString();

            if (traceRecord.Exception != null)
            {
                this.GetExceptionLogger(traceRecord.Level).Invoke(traceRecord.Exception, message);
            }
            else
            {
                this.GetTraceLogger(traceRecord.Level).Invoke(message);
            }
        }
    }
}