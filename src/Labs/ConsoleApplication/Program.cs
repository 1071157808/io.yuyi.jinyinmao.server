// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-29  8:49 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Helper;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CryptographyHelper.Check("123456", "585a59a9c2a94e6a92817fec0134b772", "B98AA59AEB52AE86E507A94BB474E2691660C380788B1E28D069F75240BEB7DD");
        }

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
                this.PartitionKey = "api.sms.config.appkeys";
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
}