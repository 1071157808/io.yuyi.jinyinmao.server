// FileInformation: nyanya/Domian/CqrsDomainConfig.cs
// CreatedTime: 2014/07/14   4:42 PM
// LastUpdatedTime: 2014/07/20   7:16 PM

using Infrastructure.Lib.Logs.Implementation;
using ServiceStack.Text;

namespace Domian.Config
{
    public static class CqrsDomainConfig
    {
        public static readonly CqrsConfiguration Configuration;

        static CqrsDomainConfig()
        {
            Configuration = new CqrsConfiguration();
            Configuration.Logs.EventBusLogger = new NLogger("EventBusLogger");
            Configuration.Logs.EventDispatcherLogger = new NLogger("EventDispatcherLogger");
            JsConfig.DateHandler = DateHandler.ISO8601;
            JsConfig.IncludeNullValues = true;
            JsConfig.TreatEnumAsInteger = true;
        }
    }
}