// FileInformation: nyanya/Infrastructure.Cache.Interface/ICacheClient.cs
// CreatedTime: 2014/06/03   4:05 PM
// LastUpdatedTime: 2014/06/03   4:40 PM

using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Infrastructure.Cache.Interface
{
    public interface ICacheClient
    {
        Task<T> Get<T>(string key);

        Task<string> Get(string key);

        Task Set<T>(string key, T t, TimeSpan? timeSpan = null);

        Task Set(string key, string value, TimeSpan? timeSpan = null);
    }
}