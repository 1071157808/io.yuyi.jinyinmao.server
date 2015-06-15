// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-22  6:41 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-14  11:49 PM
// ***********************************************************************
// <copyright file="SiloClusterLogger.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Serilog;
using Yuyi.Jinyinmao.Log;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     SiloClusterLogger.
    /// </summary>
    public static class SiloClusterErrorLogger
    {
        private static readonly ILogger Logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SiloClusterErrorLogger" /> class.
        /// </summary>
        static SiloClusterErrorLogger()
        {
            CloudStorageAccount storage = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            ILogger log = new LoggerConfiguration().WriteTo.AzureTableStorage(storage, "Errors").CreateLogger();
            Serilog.Log.Logger = log;
            Logger = log;
        }

        /// <summary>
        ///     Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="messageTemplate">The message template.</param>
        /// <param name="propertyValues">The property values.</param>
        public static void Log(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Logger.Error(exception, messageTemplate, propertyValues);
        }

        /// <summary>
        ///     Logs the specified message template.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <param name="propertyValues">The property values.</param>
        public static void Log(string messageTemplate, params object[] propertyValues)
        {
            Logger.Error(messageTemplate, propertyValues);
        }
    }

    /// <summary>
    ///     SiloClusterTraceLogger.
    /// </summary>
    public static class SiloClusterTraceLogger
    {
        private static readonly ILogger Logger;

        /// <summary>
        ///     Initializes static members of the <see cref="SiloClusterTraceLogger" /> class.
        /// </summary>
        static SiloClusterTraceLogger()
        {
            CloudStorageAccount storage = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            ILogger log = new LoggerConfiguration().WriteTo.AzureTableStorage(storage, "Logs").CreateLogger();
            Serilog.Log.Logger = log;
            Logger = log;
        }

        /// <summary>
        ///     Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="messageTemplate">The message template.</param>
        /// <param name="propertyValues">The property values.</param>
        public static void Log(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Logger.Error(exception, messageTemplate, propertyValues);
        }

        /// <summary>
        ///     Logs the specified message template.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <param name="propertyValues">The property values.</param>
        public static void Log(string messageTemplate, params object[] propertyValues)
        {
            Logger.Information(messageTemplate, propertyValues);
        }
    }
}