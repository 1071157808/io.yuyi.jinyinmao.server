// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-23  11:54 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  12:10 AM
// ***********************************************************************
// <copyright file="RetryPolicyFactory.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     RetryPolicyFactory.
    /// </summary>
    internal static class RetryPolicyHelper
    {
        private static readonly Lazy<RetryPolicy> SqlDatabaseRetryPolicy = new Lazy<RetryPolicy>(
            () => new RetryPolicy(new SqlDatabaseTransientErrorDetectionStrategy(), new ExponentialBackoff(
                "SqlDatabase", 10, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(1), true)));

        /// <summary>
        ///     Gets the SQL database retry policy.
        /// </summary>
        internal static RetryPolicy SqlDatabase => SqlDatabaseRetryPolicy.Value;
    }
}