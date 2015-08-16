// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : NLogTraceWriter.cs
// Created          : 2015-08-16  21:45
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-16  23:57
// ***********************************************************************
// <copyright file="NLogTraceWriter.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http.Tracing;
using Moe.AspNet.Utility;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Log
{
    /// <summary>
    ///     Asp.Net TraceWriter with NLog.
    /// </summary>
    public sealed class NLogTraceWriter : ITraceWriter
    {
        /// <summary>
        ///     The loggers
        /// </summary>
        private static readonly Lazy<Dictionary<TraceLevel, Action<string>>> Loggers =
            new Lazy<Dictionary<TraceLevel, Action<string>>>(() =>
                new Dictionary<TraceLevel, Action<string>>
                {
                    { TraceLevel.Debug, LogManager.GetTraceLogger().Debug },
                    { TraceLevel.Info, LogManager.GetTraceLogger().Info },
                    { TraceLevel.Error, LogManager.GetTraceLogger().Error },
                    { TraceLevel.Warn, LogManager.GetTraceLogger().Warn },
                    { TraceLevel.Fatal, LogManager.GetTraceLogger().Fatal }
                }
                );

        /// <summary>
        ///     Gets the current logger.
        /// </summary>
        /// <value>The current logger.</value>
        private static Dictionary<TraceLevel, Action<string>> CurrentLogger
        {
            get { return Loggers.Value; }
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
            LogToNlog(record);
        }

        #endregion ITraceWriter Members

        private static void LogToLogger(string category, string logMessage)
        {
            if (category == "BackOffice")
            {
                LogManager.GetBackOfficeLogger().Info(logMessage);
            }
        }

        /// <summary>
        ///     Logs to nlog.
        /// </summary>
        /// <param name="traceRecord">The trace record.</param>
        private static void LogToNlog(TraceRecord traceRecord)
        {
            StringBuilder messageBuilder = new StringBuilder();

            if (traceRecord.Request != null)
            {
                if (traceRecord.Request.Method != null)
                {
                    messageBuilder.Append(traceRecord.Request.Method);
                }
                else
                {
                    messageBuilder.Append("-");
                }

                if (traceRecord.Request.RequestUri != null)
                {
                    messageBuilder.Append(" " + traceRecord.Request.RequestUri);
                }
                else
                {
                    messageBuilder.Append(" -");
                }

                messageBuilder.Append(traceRecord.Status);

                if ((int)traceRecord.Status >= 400)
                {
                    messageBuilder.Append(Environment.NewLine);
                    string request = traceRecord.Request.ToHttpContext().Request.Dump();
                    messageBuilder.Append(request);
                }
            }

            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append(!string.IsNullOrWhiteSpace(traceRecord.Message) ? traceRecord.Message : "-");

            if (traceRecord.Exception != null)
            {
                messageBuilder.Append(Environment.NewLine);
                messageBuilder.Append(traceRecord.Exception.GetExceptionString());
            }

            string logMessage = messageBuilder.ToString();
            CurrentLogger[traceRecord.Level](logMessage);
            LogToLogger(traceRecord.Category, logMessage);
        }
    }
}