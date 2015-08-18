using System.Web.Http;

namespace Yuyi.Jinyinmao.Api.Link
{
    /// <summary>
    /// WebApiConfig.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}