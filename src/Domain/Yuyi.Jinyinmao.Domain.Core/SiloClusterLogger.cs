// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SiloClusterLogger.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  17:17
// ***********************************************************************
// <copyright file="SiloClusterLogger.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Text;
using Moe.Lib;
using NLog;
using LogManager = Yuyi.Jinyinmao.Log.LogManager;

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
            Logger = LogManager.GetErrorLogger();
        }

        /// <summary>
        ///     Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="messageTemplate">The message template.</param>
        /// <param name="propertyValues">The property values.</param>
        public static void Log(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(DateTime.UtcNow.ToChinaStandardTime().ToString("O"));
            messageBuilder.Append("\r\n");
            messageBuilder.Append(messageTemplate.FormatWith(propertyValues));
            messageBuilder.Append("\r\n");
            messageBuilder.Append(exception.GetExceptionString());

            Logger.Error(messageBuilder);
        }

        /// <summary>
        ///     Logs the specified message template.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <param name="propertyValues">The property values.</param>
        public static void Log(string messageTemplate, params object[] propertyValues)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(DateTime.UtcNow.ToChinaStandardTime().ToString("O"));
            messageBuilder.Append("\r\n");
            messageBuilder.Append(messageTemplate.FormatWith(propertyValues));

            Logger.Error(messageBuilder);
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
            Logger = LogManager.GetApplicationLogger();
        }

        /// <summary>
        ///     Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="messageTemplate">The message template.</param>
        /// <param name="propertyValues">The property values.</param>
        public static void Log(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(DateTime.UtcNow.ToChinaStandardTime().ToString("O"));
            messageBuilder.Append("\r\n");
            messageBuilder.Append(messageTemplate.FormatWith(propertyValues));
            messageBuilder.Append("\r\n");
            messageBuilder.Append(exception.GetExceptionString());

            Logger.Error(messageBuilder);
        }

        /// <summary>
        ///     Logs the specified message template.
        /// </summary>
        /// <param name="messageTemplate">The message template.</param>
        /// <param name="propertyValues">The property values.</param>
        public static void Log(string messageTemplate, params object[] propertyValues)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(DateTime.UtcNow.ToChinaStandardTime().ToString("O"));
            messageBuilder.Append("\r\n");
            messageBuilder.Append(messageTemplate.FormatWith(propertyValues));

            Logger.Error(messageBuilder);
        }
    }
}