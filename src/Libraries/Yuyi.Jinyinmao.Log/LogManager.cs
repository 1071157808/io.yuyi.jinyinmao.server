// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : LogManager.cs
// Created          : 2015-08-16  21:08
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  22:04
// ***********************************************************************
// <copyright file="LogManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Text;
using Microsoft.WindowsAzure.ServiceRuntime;
using Moe.Lib;
using NLog;
using NLog.Config;
using NLog.Extensions.AzureTableStorage;
using NLog.Targets.Wrappers;

namespace Yuyi.Jinyinmao.Log
{
    /// <summary>
    ///     ILoggerEx.
    /// </summary>
    public static class LoggerEx
    {
        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        public static void LogError(this ILogger logger, string message)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(DateTime.UtcNow.ToChinaStandardTime().ToString("O"));
            messageBuilder.Append("   ");
            messageBuilder.Append(RoleEnvironment.CurrentRoleInstance.Role.Name + RoleEnvironment.CurrentRoleInstance.Id);
            messageBuilder.Append("   ");
            messageBuilder.Append(message);

            logger.Error(messageBuilder);
        }

        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void LogError(this ILogger logger, string message, Exception exception)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(DateTime.UtcNow.ToChinaStandardTime().ToString("O"));
            messageBuilder.Append("   ");
            messageBuilder.Append(RoleEnvironment.CurrentRoleInstance.Role.Name + RoleEnvironment.CurrentRoleInstance.Id);
            messageBuilder.Append("   ");
            messageBuilder.Append(message);
            messageBuilder.Append("   ");
            messageBuilder.Append(exception.GetExceptionString());

            logger.Error(messageBuilder);
        }

        /// <summary>
        ///     Logs the message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        public static void LogMessage(this ILogger logger, string message)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(DateTime.UtcNow.ToChinaStandardTime().ToString("O"));
            messageBuilder.Append("   ");
            messageBuilder.Append(RoleEnvironment.CurrentRoleInstance.Role.Name + RoleEnvironment.CurrentRoleInstance.Id);
            messageBuilder.Append("   ");
            messageBuilder.Append(message);

            logger.Info(messageBuilder);
        }
    }

    /// <summary>
    ///     LogManager.
    /// </summary>
    public static class LogManager
    {
        private static readonly Lazy<ILogger> ApplicationLogger = new Lazy<ILogger>(() => InitApplicationLogger());
        private static readonly Lazy<ILogger> ErrorLogger = new Lazy<ILogger>(() => InitErrorLogger());
        private static readonly Lazy<ILogger> TraceLogger = new Lazy<ILogger>(() => InitTraceLogger());

        static LogManager()
        {
            LoggingConfiguration config = new LoggingConfiguration();

            AzureTableStorageTarget azureErrorTarget = new AzureTableStorageTarget();
            AzureTableStorageTarget azureTraceTarget = new AzureTableStorageTarget();
            AzureTableStorageTarget azureApplicationTarget = new AzureTableStorageTarget();
            AsyncTargetWrapper azureErrorTargetWrapper = new AsyncTargetWrapper
            {
                WrappedTarget = azureErrorTarget,
                OverflowAction = AsyncTargetWrapperOverflowAction.Grow
            };
            AsyncTargetWrapper azureTraceTargetWrapper = new AsyncTargetWrapper
            {
                WrappedTarget = azureTraceTarget,
                OverflowAction = AsyncTargetWrapperOverflowAction.Grow
            };
            AsyncTargetWrapper azureApplicationTargetWrapper = new AsyncTargetWrapper
            {
                WrappedTarget = azureApplicationTarget,
                OverflowAction = AsyncTargetWrapperOverflowAction.Grow
            };

            config.AddTarget("azureErrorTarget", azureErrorTargetWrapper);
            config.AddTarget("azureTraceTarget", azureTraceTargetWrapper);
            config.AddTarget("azureApplicationTarget", azureApplicationTargetWrapper);

            azureErrorTarget.ConnectionStringKey = "DataConnectionString";
            azureErrorTarget.Layout = "${message}";
            azureErrorTarget.TableName = "JYMErrorLogs";

            azureTraceTarget.ConnectionStringKey = "DataConnectionString";
            azureTraceTarget.Layout = "${message}";
            azureTraceTarget.TableName = "JYMTraceLogs";

            azureApplicationTarget.ConnectionStringKey = "DataConnectionString";
            azureApplicationTarget.Layout = "${message}";
            azureApplicationTarget.TableName = "JYMApplicationLogs";

            config.LoggingRules.Add(new LoggingRule("ErrorLogger", LogLevel.Error, azureErrorTarget));
            config.LoggingRules.Add(new LoggingRule("TraceLogger", LogLevel.Error, azureTraceTarget));
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
        ///     Gets the error logger.
        /// </summary>
        /// <returns>ILogger.</returns>
        public static ILogger GetErrorLogger()
        {
            return ErrorLogger.Value;
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

        private static ILogger InitErrorLogger()
        {
            return NLog.LogManager.GetLogger("ErrorLogger");
        }

        private static ILogger InitTraceLogger()
        {
            return NLog.LogManager.GetLogger("TraceLogger");
        }
    }
}