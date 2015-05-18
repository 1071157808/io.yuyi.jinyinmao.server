// FileInformation: nyanya/Infrastructure.Cache.Couchbase/CouchbaseClientExtensions.cs
// CreatedTime: 2014/09/12   12:44 PM
// LastUpdatedTime: 2014/09/12   1:58 PM

using System;
using System.Collections.Generic;
using Couchbase;
using Enyim.Caching.Memcached;
using Enyim.Caching.Memcached.Results;
using Infrastructure.EL.TransientFaultHandling;
using Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy;
using Infrastructure.Lib.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Infrastructure.Cache.Couchbase
{
    internal static class CouchbaseClientExtensions
    {
        private static readonly RetryPolicy retryPolicy = new RetryPolicy<CouchbaseErrorDetectionStrategy>(RetryStrategyFactory.GetCouchbaseContextRetryPolicy());

        internal static IStoreOperationResult CasData(this CouchbaseClient client, StoreMode mode, string key, object value, ulong cas)
        {
            return retryPolicy.ExecuteAction(() => client.ExecuteCasData(mode, key, value, cas));
        }

        internal static IStoreOperationResult CasData(this CouchbaseClient client, StoreMode mode, string key, object value, DateTime expiresAt, ulong cas)
        {
            return retryPolicy.ExecuteAction(() => client.ExecuteCasData(mode, key, value, expiresAt, cas));
        }

        internal static IStoreOperationResult CasData(this CouchbaseClient client, StoreMode mode, string key, object value, TimeSpan validFor, ulong cas)
        {
            return retryPolicy.ExecuteAction(() => client.ExecuteCasData(mode, key, value, validFor, cas));
        }

        internal static object GetData(this CouchbaseClient client, string key)
        {
            return retryPolicy.ExecuteAction(() => client.ExecuteGetData(key));
        }

        internal static IDictionary<string, object> GetData(this CouchbaseClient client, IEnumerable<string> keys)
        {
            return retryPolicy.ExecuteAction(() => client.ExecuteGetData(keys));
        }

        internal static IStoreOperationResult StoreData(this CouchbaseClient client, StoreMode mode, string key, object value)
        {
            return retryPolicy.ExecuteAction(() => client.ExecuteStoreData(mode, key, value));
        }

        internal static IStoreOperationResult StoreData(this CouchbaseClient client, StoreMode mode, string key, object value, DateTime expiresAt)
        {
            return retryPolicy.ExecuteAction(() => client.ExecuteStoreData(mode, key, value, expiresAt));
        }

        internal static IStoreOperationResult StoreData(this CouchbaseClient client, StoreMode mode, string key, object value, TimeSpan validFor)
        {
            return retryPolicy.ExecuteAction(() => client.ExecuteStoreData(mode, key, value, validFor));
        }

        private static IStoreOperationResult ExecuteCasData(this CouchbaseClient client, StoreMode mode, string key, object value, ulong cas)
        {
            IStoreOperationResult result = client.ExecuteCas(mode, key, value, cas);
            if (!result.Success)
            {
                throw new CouchbaseOperationFailedException(result.StatusCode.GetValueOrDefault(), result.Message, result.Exception);
            }

            return result;
        }

        private static IStoreOperationResult ExecuteCasData(this CouchbaseClient client, StoreMode mode, string key, object value, DateTime expiresAt, ulong cas)
        {
            IStoreOperationResult result = client.ExecuteCas(mode, key, value, expiresAt, cas);
            if (!result.Success)
            {
                throw new CouchbaseOperationFailedException(result.StatusCode.GetValueOrDefault(), result.Message, result.Exception);
            }

            return result;
        }

        private static IStoreOperationResult ExecuteCasData(this CouchbaseClient client, StoreMode mode, string key, object value, TimeSpan validFor, ulong cas)
        {
            IStoreOperationResult result = client.ExecuteCas(mode, key, value, validFor, cas);
            if (!result.Success)
            {
                throw new CouchbaseOperationFailedException(result.StatusCode.GetValueOrDefault(), result.Message, result.Exception);
            }

            return result;
        }

        private static object ExecuteGetData(this CouchbaseClient client, string key)
        {
            object value;
            IGetOperationResult result = client.ExecuteTryGet(key, out value);
            if (!result.Success)
            {
                throw new CouchbaseOperationFailedException(result.StatusCode.GetValueOrDefault(), result.Message, result.Exception);
            }
            return value;
        }

        private static IDictionary<string, object> ExecuteGetData(this CouchbaseClient client, IEnumerable<string> keys)
        {
            IDictionary<string, object> result = client.Get(keys);
            return result;
        }

        private static IStoreOperationResult ExecuteStoreData(this CouchbaseClient client, StoreMode mode, string key, object value)
        {
            IStoreOperationResult result = client.ExecuteStore(mode, key, value);
            if (!result.Success)
            {
                throw new CouchbaseOperationFailedException(result.StatusCode.GetValueOrDefault(), result.Message, result.Exception);
            }

            return result;
        }

        private static IStoreOperationResult ExecuteStoreData(this CouchbaseClient client, StoreMode mode, string key, object value, DateTime expiresAt)
        {
            IStoreOperationResult result = client.ExecuteStore(mode, key, value, expiresAt);
            if (!result.Success)
            {
                throw new CouchbaseOperationFailedException(result.StatusCode.GetValueOrDefault(), result.Message, result.Exception);
            }

            return result;
        }

        private static IStoreOperationResult ExecuteStoreData(this CouchbaseClient client, StoreMode mode, string key, object value, TimeSpan validFor)
        {
            IStoreOperationResult result = client.ExecuteStore(mode, key, value, validFor);
            if (!result.Success)
            {
                throw new CouchbaseOperationFailedException(result.StatusCode.GetValueOrDefault(), result.Message, result.Exception);
            }

            return result;
        }
    }
}