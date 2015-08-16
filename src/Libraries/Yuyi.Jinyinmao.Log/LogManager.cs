// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : LogManager.cs
// Created          : 2015-08-16  21:08
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-16  23:24
// ***********************************************************************
// <copyright file="LogManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using NLog;
using NLog.Config;

namespace Yuyi.Jinyinmao.Log
{
    /// <summary>
    ///     LogManager.
    /// </summary>
    public static class LogManager
    {
        private static readonly Lazy<ILogger> BackOfficeLogger = new Lazy<ILogger>(() => InitBackOfficeLogger());
        private static readonly Lazy<ILogger> ExceptionLogger = new Lazy<ILogger>(() => InitExceptionLogger());

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

        private static ILogger InitBackOfficeLogger()
        {
            return NLog.LogManager.GetLogger("BackOfficeLogger");
        }

        private static ILogger InitExceptionLogger()
        {
            LoggingConfiguration config = new LoggingConfiguration();

            return NLog.LogManager.GetLogger("ExceptionLogger");
        }
    }
}