// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-22  6:41 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-10  11:27 AM
// ***********************************************************************
// <copyright file="SiloClusterLogger.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     SiloClusterLogger.
    /// </summary>
    public static class SiloClusterErrorLogger
    {
        /// <summary>
        ///     Logs this instance.
        /// </summary>
        public static void Log(ErrorLog log)
        {
            SiloClusterConfig.ErrorLogsTable.Execute(TableOperation.InsertOrReplace(log));
        }
    }

    /// <summary>
    ///     ErrorLog.
    /// </summary>
    public class ErrorLog : TableEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ErrorLog" /> class.
        /// </summary>
        public ErrorLog()
        {
            this.Data = string.Empty;
            this.TimeStamp = DateTime.UtcNow.Ticks;
        }

        /// <summary>
        ///     Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data { get; set; }

        /// <summary>
        ///     Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public string Exception { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public long TimeStamp { get; set; }
    }
}