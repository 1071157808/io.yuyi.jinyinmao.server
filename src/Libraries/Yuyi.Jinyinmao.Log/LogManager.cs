// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : LogManager.cs
// Created          : 2015-08-16  21:08
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  0:00
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
        private static readonly Lazy<ILogger> BackOfficeLogger = new Lazy<ILogger>(() => InitBackOfficeLogger());
        private static readonly Lazy<ILogger> DaemonLogger = new Lazy<ILogger>(() => InitDaemonLogger());
        private static readonly Lazy<ILogger> ExceptionLogger = new Lazy<ILogger>(() => InitExceptionLogger());
        private static readonly Lazy<ILogger> TraceLogger = new Lazy<ILogger>(() => InitTraceLogger());

        static LogManager()
        {
            LoggingConfiguration config = new LoggingConfiguration();

            AzureTableStorageTarget azureErrorTarget = new AzureTableStorageTarget();
            AzureTableStorageTarget azureTraceTarget = new AzureTableStorageTarget();
            AzureTableStorageTarget azureBackOfficeTarget = new AzureTableStorageTarget();
            AzureTableStorageTarget azureDaemonTarget = new AzureTableStorageTarget();

            config.AddTarget("azureErrorTarget", azureErrorTarget);
            config.AddTarget("azureTraceTarget", azureTraceTarget);
            config.AddTarget("azureBackOfficeTarget", azureBackOfficeTarget);
            config.AddTarget("azureDaemonTarget", azureDaemonTarget);

            azureErrorTarget.ConnectionStringKey = "DataConnectionString";
            azureErrorTarget.Layout = "[date:${longdate}] [level:${level}] [appdomain:${appdomain}] [machine:${machinename}] [message:${message}]";
            azureErrorTarget.TableName = "JYMErrorLogs";

            azureTraceTarget.ConnectionStringKey = "DataConnectionString";
            azureTraceTarget.Layout = "[date:${longdate}] [level:${level}] [appdomain:${appdomain}] [machine:${machinename}] [message:${message}]";
            azureTraceTarget.TableName = "JYMTraceLogs";

            azureBackOfficeTarget.ConnectionStringKey = "DataConnectionString";
            azureBackOfficeTarget.Layout = "[date:${longdate}] [level:${level}] [appdomain:${appdomain}] [machine:${machinename}] [message:${message}]";
            azureBackOfficeTarget.TableName = "JYMBackOfficeLogs";

            azureDaemonTarget.ConnectionStringKey = "DataConnectionString";
            azureDaemonTarget.Layout = "[date:${longdate}] [level:${level}] [appdomain:${appdomain}] [machine:${machinename}] [message:${message}]";
            azureDaemonTarget.TableName = "JYMDaemonLogs";

            config.LoggingRules.Add(new LoggingRule("ExceptionLogger", LogLevel.Error, azureErrorTarget));
            config.LoggingRules.Add(new LoggingRule("TraceLogger", LogLevel.Info, azureTraceTarget));
            config.LoggingRules.Add(new LoggingRule("BackOfficeLogger", LogLevel.Info, azureBackOfficeTarget));
            config.LoggingRules.Add(new LoggingRule("DaemonLogger", LogLevel.Info, azureDaemonTarget));

            NLog.LogManager.Configuration = config;
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
        ///     Gets the daemon logger.
        /// </summary>
        /// <returns>ILogger.</returns>
        public static ILogger GetDaemonLogger()
        {
            return DaemonLogger.Value;
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

        private static ILogger InitBackOfficeLogger()
        {
            return NLog.LogManager.GetLogger("BackOfficeLogger");
        }

        private static ILogger InitDaemonLogger()
        {
            return NLog.LogManager.GetLogger("DaemonLogger");
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