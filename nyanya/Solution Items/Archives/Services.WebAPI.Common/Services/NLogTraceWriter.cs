// FileInformation: nyanya/Services.WebAPI.Common/NLogTraceWriter.cs
// CreatedTime: 2014/03/30   11:05 PM
// LastUpdatedTime: 2014/03/30   11:07 PM

using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http.Tracing;

namespace Services.WebAPI.Common.Services.NLogTracing
{
    public class NLogTraceWriter : ITraceWriter
    {
        private static readonly Lazy<Dictionary<TraceLevel, Action<string>>> loggers =
            new Lazy<Dictionary<TraceLevel, Action<string>>>(() =>
                new Dictionary<TraceLevel, Action<string>>
                {
                    { TraceLevel.Debug, LogManager.GetCurrentClassLogger().Debug },
                    { TraceLevel.Error, LogManager.GetCurrentClassLogger().Error },
                    { TraceLevel.Fatal, LogManager.GetCurrentClassLogger().Fatal },
                    { TraceLevel.Info, LogManager.GetCurrentClassLogger().Info },
                    { TraceLevel.Warn, LogManager.GetCurrentClassLogger().Warn }
                }
                );

        private Dictionary<TraceLevel, Action<string>> CurrentLogger
        {
            get { return loggers.Value; }
        }

        #region ITraceWriter Members

        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (level == TraceLevel.Off) return;

            TraceRecord record = new TraceRecord(request, category, level);

            traceAction(record);
            this.LogToNlog(record);
        }

        #endregion ITraceWriter Members

        private void LogToNlog(TraceRecord traceRecord)
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

            if (!String.IsNullOrWhiteSpace(traceRecord.Category))
            {
                messageBuilder.Append(" " + traceRecord.Category);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (!String.IsNullOrWhiteSpace(traceRecord.Operator))
            {
                messageBuilder.Append(" " + traceRecord.Operator);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (!String.IsNullOrWhiteSpace(traceRecord.Operation))
            {
                messageBuilder.Append(" " + traceRecord.Operation);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (!String.IsNullOrWhiteSpace(traceRecord.Message))
            {
                messageBuilder.Append(" " + traceRecord.Message);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (traceRecord.Exception != null)
            {
                messageBuilder.Append(" " + traceRecord.Exception.GetBaseException().Message);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            this.CurrentLogger[traceRecord.Level](messageBuilder.ToString());
        }
    }
}