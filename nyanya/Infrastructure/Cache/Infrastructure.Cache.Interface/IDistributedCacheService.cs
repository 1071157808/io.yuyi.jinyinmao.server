// FileInformation: nyanya/Infrastructure.Cache.Interface/IDistributedCacheService.cs
// CreatedTime: 2014/04/23   2:26 PM
// LastUpdatedTime: 2014/04/23   4:53 PM

using Newtonsoft.Json.Linq;

namespace Infrastructure.Cache.Interface
{
    public interface IDistributedCacheService
    {
        JObject Get(string key);

        bool SetValue(string key, string value);
    }
}