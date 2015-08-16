// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : LogManager.cs
// Created          : 2015-08-16  21:08
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-16  21:11
// ***********************************************************************
// <copyright file="LogManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using NLog;

namespace Yuyi.Jinyinmao.Log
{
    /// <summary>
    ///     LogManager.
    /// </summary>
    public static class LogManager
    {
        /// <summary>
        ///     The exception logger
        /// </summary>
        private static readonly Lazy<ILogger> ExceptionLogger = new Lazy<ILogger>(() => InitExceptionLogger());

        /// <summary>
        ///     Gets the exception logger.
        /// </summary>
        /// <returns>ILogger.</returns>
        public static ILogger GetExceptionLogger()
        {
            return ExceptionLogger.Value;
        }

        private static ILogger InitExceptionLogger()
        {
            return NLog.LogManager.GetLogger("ExceptionLogger");
        }
    }
}