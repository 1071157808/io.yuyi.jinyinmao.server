// FileInformation: nyanya/Infrastructure.Cache.Redis/RedisClient.cs
// CreatedTime: 2014/06/03   3:35 PM
// LastUpdatedTime: 2014/06/03   4:45 PM

using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using Infrastructure.Cache.Interface;
using Infrastructure.EL.TransientFaultHandling;
using Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.Cache.Redis
{
    public class RedisClient : ICacheClient
    {
        private static ConnectionMultiplexer redis;

        static RedisClient()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("Redis"); // "redis0:6380,redis1:6380,allowAdmin=true"
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ConfigurationErrorsException("Can not find Redis ConnectionString");
            }
            // https://github.com/StackExchange/StackExchange.Redis/issues/42
            // RedisConnectionException at each first Connect(), while redis-py and hiredis work fine

            RetryPolicy connectRetryPolicy = new RetryPolicy<RedisErrorDetectionStrategy>(RetryStrategyFactory.GetRedisDbContextRetryPolicy());
            connectRetryPolicy.Retrying += (s, e) =>
                Trace.TraceError("An error occurred in attempt number {1} to connect to redis server with connection string - {2}: {0}", e.LastException.Message, e.CurrentRetryCount, connectionString);
            connectRetryPolicy.ExecuteAction(() => { redis = ConnectionMultiplexer.Connect(connectionString); });
        }

        public static IDatabase Database
        {
            get { return Redis.GetDatabase(); }
        }

        public static ConnectionMultiplexer Redis
        {
            get { return redis; }
        }

        /*
            The simplest configuration example is just the host name:

            var conn = ConnectionMultiplexer.Connect("localhost");
            This will connect to a single server on the local machine using the default redis port (6379). Additional options are simply appended (comma-delimited). Ports are represented with a colon (:) as is usual. Configuration options include an = after the name. For example:

            var conn = ConnectionMultiplexer.Connect("redis0:6380,redis1:6380,allowAdmin=true");
            An extensive mapping between the string and ConfigurationOptions representation is not documented here, but you can switch between them trivially:

            ConfigurationOptions options = ConfigurationOptions.Parse(configString);

            A common usage is to store the basic details in a string, and then apply specific details at runtime:

            string configString = GetRedisConfiguration();
            var options = ConfigurationOptions.Parse(configString);
            options.ClientName = GetAppName(); // only known at runtime
            options.AllowAdmin = true;
            conn = ConnectionMultiplexer.Connect(options);

            The ConfigurationOptions object has a wide range of properties, all of which are fully documented in intellisense. Some of the more common options to use include:

            AllowAdmin - enables potentially harmful system commands that are not needed by data-oriented clients
            ClientName - sets a name against the connections to identify them (visible to redis maintenance tools)
            CommandMap - renames or disables individual redis commands
            DefaultVersion - the redis version to assume if it cannot auto-configure
            EndPoints - the set of redis nodes to connect to
            Password - the password to authenticate (AUTH) against the redis server
            SyncTimeout - the timeout to apply when performing synchronous operations
         */

        #region ICacheClient Members

        public async Task<T> Get<T>(string key)
        {
            try
            {
                RedisValue cache = await Database.StringGetAsync(key);
                return cache.IsNull ? default(T) : JsonConvert.DeserializeObject<T>(cache.ToString());
            }
            catch (Exception e)
            {
                // UNDONE
                throw new NotImplementedException(e.Message, e);
            }
        }

        public async Task<string> Get(string key)
        {
            try
            {
                RedisValue cache = await Database.StringGetAsync(key);
                return cache.IsNull ? "" : cache.ToString();
            }
            catch (Exception e)
            {
                // UNDONE
                throw new NotImplementedException(e.Message, e);
            }
        }

        public Task Set<T>(string key, T t, TimeSpan? timeSpan = null)
        {
            try
            {
                string json = JsonConvert.SerializeObject(t);
                return this.Set(key, json, timeSpan);
            }
            catch (Exception e)
            {
                // UNDONE
                throw new NotImplementedException(e.Message, e);
            }
        }

        public Task Set(string key, string value, TimeSpan? timeSpan = null)
        {
            try
            {
                return Database.StringSetAsync(key, value, timeSpan, When.Always, CommandFlags.FireAndForget);
            }
            catch (Exception e)
            {
                // UNDONE
                throw new NotImplementedException(e.Message, e);
            }
        }

        #endregion ICacheClient Members
    }
}