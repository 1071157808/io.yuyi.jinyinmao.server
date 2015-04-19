// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  3:21 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  3:26 PM
// ***********************************************************************
// <copyright file="App.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConsoleApplication
{
    /// <summary>
    ///     App.
    /// </summary>
    public class App : TableEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="App" /> class.
        /// </summary>
        public App()
        {
            this.PartitionKey = "cn.com.jinyinmao.api.sms.config.appkeys";
            this.RowKey = Guid.NewGuid().ToString();
        }

        /// <summary>
        ///     Gets or sets the application identifier.
        /// </summary>
        /// <value>The application identifier.</value>
        public Guid AppId
        {
            get { return Guid.Parse(this.RowKey); }
            set { this.RowKey = value.ToString(); }
        }

        /// <summary>
        ///     Gets or sets the application key.
        /// </summary>
        /// <value>The application key.</value>
        public string AppKey { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string AppName { get; set; }

        /// <summary>
        ///     Gets or sets the expiry.
        /// </summary>
        /// <value>The expiry.</value>
        public DateTime Expiry { get; set; }

        /// <summary>
        ///     Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }
    }
}
