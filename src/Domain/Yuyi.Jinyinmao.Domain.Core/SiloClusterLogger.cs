// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SiloClusterLogger.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  21:48
// ***********************************************************************
// <copyright file="SiloClusterLogger.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using NLog;
using Yuyi.Jinyinmao.Log;
using LogManager = Yuyi.Jinyinmao.Log.LogManager;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     SiloClusterApplicationLogger.
    /// </summary>
    public static class SiloClusterApplicationLogger
    {
        private static readonly ILogger Logger;

        /// <summary>
        ///     Initializes static members of the <see cref="SiloClusterApplicationLogger" /> class.
        /// </summary>
        static SiloClusterApplicationLogger()
        {
            Logger = LogManager.GetApplicationLogger();
        }

        /// <summary>
        ///     Logs the specified exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void LogError(string message, Exception exception)
        {
            Logger.LogError(message, exception);
        }

        /// <summary>
        ///     Logs the specified exception.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogError(string message)
        {
            Logger.LogError(message);
        }

        /// <summary>
        ///     Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogMessage(string message)
        {
            Logger.LogMessage(message);
        }
    }

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
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void LogError(string message, Exception exception)
        {
            Logger.LogError(message, exception);
        }

        /// <summary>
        ///     Logs the specified exception.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogError(string message)
        {
            Logger.LogError(message);
        }

        /// <summary>
        ///     Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogMessage(string message)
        {
            Logger.LogMessage(message);
        }
    }
}