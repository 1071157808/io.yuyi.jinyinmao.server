using System.Web.Http;

namespace Yuyi.Jinyinmao.Api.Link
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();
            
        }
    }
}
