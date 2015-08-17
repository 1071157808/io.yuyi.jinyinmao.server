// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : LogManager.cs
// Created          : 2015-08-16  21:08
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  12:48
// ***********************************************************************
// <copyright file="LogManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using NLog;
using NLog.Config;
using NLog.Extensions.AzureTableStorage;

namespace Yuyi.Jinyinmao.Log
{
    /// <summary>
    ///     LogManager.
    /// </summary>
    public static class LogManager
    {
        private static readonly Lazy<ILogger> ApplicationLogger = new Lazy<ILogger>(() => InitApplicationLogger());
        private static readonly Lazy<ILogger> BackOfficeLogger = new Lazy<ILogger>(() => InitBackOfficeLogger());
        private static readonly Lazy<ILogger> ExceptionLogger = new Lazy<ILogger>(() => InitExceptionLogger());
        private static readonly Lazy<ILogger> TraceLogger = new Lazy<ILogger>(() => InitTraceLogger());

        static LogManager()
        {
            LoggingConfiguration config = new LoggingConfiguration();

            AzureTableStorageTarget azureErrorTarget = new AzureTableStorageTarget();
            AzureTableStorageTarget azureTraceTarget = new AzureTableStorageTarget();
            AzureTableStorageTarget azureBackOfficeTarget = new AzureTableStorageTarget();
            AzureTableStorageTarget azureApplicationTarget = new AzureTableStorageTarget();

            config.AddTarget("azureErrorTarget", azureErrorTarget);
            config.AddTarget("azureTraceTarget", azureTraceTarget);
            config.AddTarget("azureBackOfficeTarget", azureBackOfficeTarget);
            config.AddTarget("azureApplicationTarget", azureApplicationTarget);

            azureErrorTarget.ConnectionStringKey = "DataConnectionString";
            azureErrorTarget.Layout = "${message}";
            azureErrorTarget.TableName = "JYMErrorLogs";

            azureTraceTarget.ConnectionStringKey = "DataConnectionString";
            azureTraceTarget.Layout = "${message}";
            azureTraceTarget.TableName = "JYMTraceLogs";

            azureBackOfficeTarget.ConnectionStringKey = "DataConnectionString";
            azureBackOfficeTarget.Layout = "${message}";
            azureBackOfficeTarget.TableName = "JYMBackOfficeLogs";

            azureApplicationTarget.ConnectionStringKey = "DataConnectionString";
            azureApplicationTarget.Layout = "${message}";
            azureApplicationTarget.TableName = "JYMApplicationLogs";

            config.LoggingRules.Add(new LoggingRule("ExceptionLogger", LogLevel.Error, azureErrorTarget));
            config.LoggingRules.Add(new LoggingRule("TraceLogger", LogLevel.Info, azureTraceTarget));
            config.LoggingRules.Add(new LoggingRule("BackOfficeLogger", LogLevel.Info, azureBackOfficeTarget));
            config.LoggingRules.Add(new LoggingRule("ApplicationLogger", LogLevel.Info, azureApplicationTarget));

            NLog.LogManager.Configuration = config;
        }

        /// <summary>
        ///     Gets the Application logger.
        /// </summary>
        /// <returns>ILogger.</returns>
        public static ILogger GetApplicationLogger()
        {
            return ApplicationLogger.Value;
        }

        /// <summary>
        ///     Gets the back office logger.
        /// </summary>
        /// <returns>ILogger.</returns>
        public static ILogger GetBackOfficeLogger()
        {
            return BackOfficeLogger.Value;
        }

        /// <summary>
        ///     Gets the exception logger.
        /// </summary>
        /// <returns>ILogger.</returns>
        public static ILogger GetExceptionLogger()
        {
            return ExceptionLogger.Value;
        }

        /// <summary>
        ///     Gets the trace logger.
        /// </summary>
        /// <returns>ILogger.</returns>
        public static ILogger GetTraceLogger()
        {
            return TraceLogger.Value;
        }

        private static ILogger InitApplicationLogger()
        {
            return NLog.LogManager.GetLogger("ApplicationLogger");
        }

        private static ILogger InitBackOfficeLogger()
        {
            return NLog.LogManager.GetLogger("BackOfficeLogger");
        }

        private static ILogger InitExceptionLogger()
        {
            return NLog.LogManager.GetLogger("ExceptionLogger");
        }

        private static ILogger InitTraceLogger()
        {
            return NLog.LogManager.GetLogger("TraceLogger");
        }
    }
}