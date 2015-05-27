// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-23  11:22 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  8:11 AM
// ***********************************************************************
// <copyright file="ErrorCode.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Enum ErrorCode
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        ///     The providers base
        /// </summary>
        ProvidersBase = 200000,

        // Azure storage provider related
        /// <summary>
        ///     The azure table provider base
        /// </summary>
        AzureTableProviderBase = ProvidersBase + 100,

        /// <summary>
        ///     The azure table provider_ read
        /// </summary>
        AzureTableProviderRead = AzureTableProviderBase + 1,

        /// <summary>
        ///     The azure table provider_ write
        /// </summary>
        AzureTableProviderWrite = AzureTableProviderBase + 2,

        /// <summary>
        ///     The azure table provider_ delete
        /// </summary>
        AzureTableProviderDelete = AzureTableProviderBase + 3,

        /// <summary>
        ///     The azure table provider_ initialize provider
        /// </summary>
        AzureTableProviderInitProvider = AzureTableProviderBase + 4,

        /// <summary>
        ///     The azure table provider_ parameter connection string
        /// </summary>
        AzureTableProviderParamConnectionString = AzureTableProviderBase + 5,

        // Azure sql database storage provider related
        /// <summary>
        ///     The SQL database provider base
        /// </summary>
        SqlDatabaseProviderBase = ProvidersBase + 200,

        /// <summary>
        ///     The SQL database provider_ read
        /// </summary>
        SqlDatabaseProviderRead = SqlDatabaseProviderBase + 1,

        /// <summary>
        ///     The SQL database provider_ write
        /// </summary>
        SqlDatabaseProviderWrite = SqlDatabaseProviderBase + 2,

        /// <summary>
        ///     The SQL database provider_ delete
        /// </summary>
        SqlDatabaseProviderDelete = SqlDatabaseProviderBase + 3,

        /// <summary>
        ///     The SQL database provider_ initialize provider
        /// </summary>
        SqlDatabaseProviderInitProvider = SqlDatabaseProviderBase + 4,

        /// <summary>
        ///     The SQL database provider_ parameter connection string
        /// </summary>
        SqlDatabaseProviderParamConnectionString = SqlDatabaseProviderBase + 5
    }
}
