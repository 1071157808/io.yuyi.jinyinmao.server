// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : NLoggerSiloTraceWriter.cs
// Created          : 2015-08-17  12:58
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  20:03
// ***********************************************************************
// <copyright file="NLoggerSiloTraceWriter.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Net;
using System.Threading;
using Moe.Lib;
using NLog;
using Orleans.Runtime;
using Logger = Orleans.Runtime.Logger;

namespace Yuyi.Jinyinmao.Log
{
    /// <summary>
    ///     The Log Writer class is a convenient wrapper around the Nlog Trace class.
    /// </summary>
    public class NLoggerSiloTraceWriter : LogWriterBase
    {
        private readonly ILogger logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NLoggerSiloTraceWriter" /> class.
        /// </summary>
        public NLoggerSiloTraceWriter()
        {
            this.logger = LogManager.GetTraceLogger();
        }

        /// <summary>
        ///     The method to call during logging to format the log info into a string ready for output.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="severity">The severity of the message being traced.</param>
        /// <param name="loggerType">The type of logger the message is being traced through.</param>
        /// <param name="caller">The name of the logger tracing the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="myIpEndPoint">The <see cref="IPEndPoint" /> of the Orleans client/server if known. May be null.</param>
        /// <param name="exception">The exception to log. May be null.</param>
        /// <param name="errorCode">Numeric event code for this log entry. May be zero, meaning 'Unspecified'.</param>
        /// <returns>System.String.</returns>
        protected override string FormatLogMessage(DateTime timestamp, Logger.Severity severity, TraceLogger.LoggerType loggerType, string caller, string message, IPEndPoint myIpEndPoint, Exception exception, int errorCode)
        {
            string ip = myIpEndPoint?.ToString() ?? string.Empty;
            if (loggerType.Equals(TraceLogger.LoggerType.Grain))
            {
                // Grain identifies itself, so I don't want an additional long string in the prefix.
                // This is just a temporal solution to ease the dev. process, can remove later.
                ip = string.Empty;
            }
            string exc = exception.GetExceptionString();
            string msg = $"[{TraceLogger.PrintTime(timestamp)} {Thread.CurrentThread.ManagedThreadId,5}\t{(int)severity}\t{errorCode}\t{caller}\t{ip}]\t{message}\t{exc}";

            return msg;
        }

        /// <summary>
        ///     The method to call during logging to write the log message by this log.
        /// </summary>
        /// <param name="msg">Message string to be writter</param>
        /// <param name="severity">The severity level of this message</param>
        protected override void WriteLogMessage(string msg, Logger.Severity severity)
        {
            switch (severity)
            {
                case Logger.Severity.Off:
                    break;

                case Logger.Severity.Error:
                    this.logger.Error(msg);
                    break;

                case Logger.Severity.Warning:
                    //this.logger.Warn(msg);
                    break;

                case Logger.Severity.Info:
                    //this.logger.Info(msg);
                    break;

                case Logger.Severity.Verbose:
                    //this.logger.Debug(msg);
                    break;

                case Logger.Severity.Verbose2:
                case Logger.Severity.Verbose3:
                    //this.logger.Debug(msg);
                    break;
            }
        }
    }
}