// FileInformation: nyanya/Domain.Meow/IMeowConfigurationService.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/08/11   11:14 AM

using Newtonsoft.Json.Linq;

namespace Domain.Meow.Services
{
    public interface IMeowConfigurationService
    {
        JObject GetAppBanners();

        JObject GetApplicationServers();

        JObject GetAppSettings();

        JObject GetBanners();

        JObject GetDevAccounts();
    }
}