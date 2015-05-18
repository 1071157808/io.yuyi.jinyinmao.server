// FileInformation: nyanya/Services.WebAPI.V1.nyanya/AppUpgradeController.cs
// CreatedTime: 2014/07/21   12:20 AM
// LastUpdatedTime: 2014/07/21   12:42 AM

using System.Web.Http;
using Infrastructure.Cache.Couchbase;
using Newtonsoft.Json.Linq;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     App/Upgrade
    /// </summary>
    [RoutePrefix("App/Upgrade")]
    public class UpgradeController : ApiController
    {
        /// <summary>
        ///     Gets the upgrade information.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="v">The version.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(string source, string v)
        {
            decimal version;

            //  老版本App升级统一传参数 source = "ios"
            if (source.ToLower() == "ios")
            {
                source = "iphone";
            }

            if ((source.ToLower() == "iphone" || source.ToLower() == "android") && decimal.TryParse(v, out version))
            {
                JObject cache = AppUpgradeCache.Get(source.ToLower());
                decimal currentVersion;
                if (decimal.TryParse(cache.GetValue("version").ToString(), out currentVersion) && currentVersion > version)
                {
                    return this.Ok(new { status = 200, msg = "ok", data = cache });
                }
            }

            return this.Ok(new { status = 200, msg = "ok", data = "" });
        }
    }
}