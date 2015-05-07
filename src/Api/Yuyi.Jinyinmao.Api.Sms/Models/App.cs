// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  6:17 PM
// ***********************************************************************
// <copyright file="App.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Api.Sms.Models
{
    /// <summary>
    ///     App.
    /// </summary>
    public class App : TableEntity
    {
        /// <summary>
        ///     Gets or sets the application key.
        /// </summary>
        /// <value>The application key.</value>
        public string ApiKey { get; set; }

        /// <summary>
        ///     Gets or sets the application identifier.
        /// </summary>
        /// <value>The application identifier.</value>
        public Guid AppId { get; set; }

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