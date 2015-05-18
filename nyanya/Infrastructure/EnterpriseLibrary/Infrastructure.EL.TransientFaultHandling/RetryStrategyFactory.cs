// FileInformation: nyanya/Infrastructure.EL.TransientFaultHandling/RetryStrategyFactory.cs
// CreatedTime: 2014/07/01   1:28 PM
// LastUpdatedTime: 2014/07/19   2:44 PM

using System;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Infrastructure.EL.TransientFaultHandling
{
    public static class RetryStrategyFactory
    {
        public static RetryStrategy GetCouchbaseContextRetryPolicy()
        {
            return new ExponentialBackoff(5, TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(10)) { FastFirstRetry = true };
        }

        public static RetryStrategy GetHttpRetryPolicy()
        {
            return new FixedInterval("HttpRetryPolicy", 5, TimeSpan.FromSeconds(3), true);
        }

        public static RetryStrategy GetMySqlDbContextRetryPolicy()
        {
            return new ExponentialBackoff(5, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0.5)) { FastFirstRetry = true };
        }

        public static RetryStrategy GetRedisDbContextRetryPolicy()
        {
            return new ExponentialBackoff(5, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3)) { FastFirstRetry = false };
        }

        public static RetryStrategy GetSqlDbContextRetryPolicy()
        {
            return new ExponentialBackoff(3, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0.5)) { FastFirstRetry = true };
        }
    }
}