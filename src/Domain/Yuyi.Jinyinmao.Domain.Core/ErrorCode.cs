// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ErrorCode.cs
// Created          : 2015-04-23  11:22 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-11  7:58 PM
// ***********************************************************************
// <copyright file="ErrorCode.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Enum ErrorCode
    /// </summary>
    public static class ErrorCode
    {
        /// <summary>
        ///     The base
        /// </summary>
        public static readonly int Base = 100000;

        /// <summary>
        ///     The providers base
        /// </summary>
        public static readonly int ProvidersBase = Base * 2;

        // Azure storage provider related
        /// <summary>
        ///     The azure table provider base
        /// </summary>
        public static readonly int AzureTableProviderBase = ProvidersBase + 100;

        /// <summary>
        ///     The azure table provider_ read
        /// </summary>
        public static readonly int AzureTableProviderRead = AzureTableProviderBase + 1;

        /// <summary>
        ///     The azure table provider_ write
        /// </summary>
        public static readonly int AzureTableProviderWrite = AzureTableProviderBase + 2;

        /// <summary>
        ///     The azure table provider_ delete
        /// </summary>
        public static readonly int AzureTableProviderDelete = AzureTableProviderBase + 3;

        /// <summary>
        ///     The azure table provider_ initialize provider
        /// </summary>
        public static readonly int AzureTableProviderInitProvider = AzureTableProviderBase + 4;

        /// <summary>
        ///     The azure table provider_ parameter connection string
        /// </summary>
        public static readonly int AzureTableProviderParamConnectionString = AzureTableProviderBase + 5;

        // Azure sql database storage provider related
        /// <summary>
        ///     The SQL database provider base
        /// </summary>
        public static readonly int SqlDatabaseProviderBase = ProvidersBase + 200;

        /// <summary>
        ///     The SQL database provider_ read
        /// </summary>
        public static readonly int SqlDatabaseProviderRead = SqlDatabaseProviderBase + 1;

        /// <summary>
        ///     The SQL database provider_ write
        /// </summary>
        public static readonly int SqlDatabaseProviderWrite = SqlDatabaseProviderBase + 2;

        /// <summary>
        ///     The SQL database provider_ delete
        /// </summary>
        public static readonly int SqlDatabaseProviderDelete = SqlDatabaseProviderBase + 3;

        /// <summary>
        ///     The SQL database provider_ initialize provider
        /// </summary>
        public static readonly int SqlDatabaseProviderInitProvider = SqlDatabaseProviderBase + 4;

        /// <summary>
        ///     The SQL database provider_ parameter connection string
        /// </summary>
        public static readonly int SqlDatabaseProviderParamConnectionString = SqlDatabaseProviderBase + 5;

        /// <summary>
        ///     The application base
        /// </summary>
        public static readonly int ApplicationBase = Base * 3;

        /// <summary>
        /// The application grain identifier conflict
        /// </summary>
        public static readonly int ApplicationGrainIdConflict = ApplicationBase + 1;
    }
}