// FileInformation: nyanya/Services.WebAPI.V1.nyanya/DevAdminConfigController.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/27   5:23 PM

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Infrastructure.Lib.Extensions;
using Newtonsoft.Json.Linq;
using Services.WebAPI.V1.nyanya.Filters;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     DevAdmin Config(非业务接口，用于配置管理)
    /// </summary>
    [RoutePrefix("DevAdmin/Config")]
    public class DevAdminConfigController : ApiController
    {
        private const string AppConfigNamespace = "Services.WebAPI.V1.nyanya.App_Config";

        /// <summary>
        ///     Gets the cluster configuration. <see cref="AppBannersConfig" />
        /// </summary>
        /// <param name="configName">Name of the configuration.</param>
        /// <returns>Configurations</returns>
        [HttpGet]
        [Route("Cluster/{configName:minlength(1)}")]
        [TokenAuthorize(AllowLocal = true, Roles = "Administrator")]
        public async Task<IHttpActionResult> GetClusterConfig(string configName)
        {
            string relativeUri = string.Format("api/v1/DevAdmin/Config/{0}", configName);
            return await this.CollectServersResponses(relativeUri);
        }

        /// <summary>
        ///     Gets the configuration.
        /// </summary>
        /// <param name="configName">Name of the configuration.</param>
        /// <returns>Configuration</returns>
        [HttpGet]
        [Route("{configName:minlength(1)}")]
        [TokenAuthorize(AllowLocal = true, Roles = "Administrator")]
        public IHttpActionResult GetConfig(string configName)
        {
            Type applicationConfigType = typeof(ApplicationConfig);
            PropertyInfo configPropertyInfo = applicationConfigType.GetProperty(configName);
            if (configPropertyInfo == null)
            {
                return this.NotFound();
            }
            object config = configPropertyInfo.GetValue(configPropertyInfo);
            JObject configJson = JObject.FromObject(config);
            configJson.Add("Timestamp", ((Config)config).GetConfigTimestamp());
            return this.Ok(configJson);
        }

        /// <summary>
        ///     Reset Configuration
        /// </summary>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns>
        ///     Config
        /// </returns>
        [HttpGet]
        [Route("Reset/{configName:minlength(1)}/{Timestamp:minlength(1)}")]
        [TokenAuthorize(AllowLocal = true, Roles = "Administrator")]
        public IHttpActionResult Reset(string configName, string timestamp)
        {
            Type applicationConfigType = typeof(ApplicationConfig);
            PropertyInfo configPropertyInfo = applicationConfigType.GetProperty(configName);
            if (configPropertyInfo == null)
            {
                return this.NotFound();
            }
            string fullName = AppConfigNamespace + "." + configName + "Config";
            object config = Assembly.GetExecutingAssembly().CreateInstance(fullName);
            if (config == null || ((Config)config).GetConfigTimestamp() != timestamp)
            {
                return this.BadRequest(timestamp);
            }

            configPropertyInfo.SetValue(configPropertyInfo, config);
            Config currentConfig = (Config)configPropertyInfo.GetValue(configPropertyInfo);
            JObject configJson = JObject.FromObject(currentConfig);
            configJson.Add("Timestamp", currentConfig.GetConfigTimestamp());
            return this.Ok(configJson);
        }

        /// <summary>
        ///     Resets the cluster configuration.
        /// </summary>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns>Configurations</returns>
        [HttpGet]
        [Route("Cluster/Reset/{configName:minlength(1)}/{Timestamp:minlength(1)}")]
        [TokenAuthorize(AllowLocal = true, Roles = "Administrator")]
        public async Task<IHttpActionResult> ResetClusterConfig(string configName, string timestamp)
        {
            string relativeUri = string.Format("api/v1/DevAdmin/Config/Reset/{0}/{1}", configName, timestamp);
            return await this.CollectServersResponses(relativeUri);
        }

        /// <summary>
        ///     Collects the servers responses.
        /// </summary>
        /// <param name="relativeUri">The relative URI.</param>
        private async Task<IHttpActionResult> CollectServersResponses(string relativeUri)
        {
            IEnumerable<string> servers = ApplicationConfig.ApplicationServers.Servers;
            if (!servers.Any())
            {
                return this.InternalServerError();
            }

            JObject json = new JObject();
            using (HttpClient client = new HttpClient())
            {
                foreach (string server in servers)
                {
                    Uri baseAddress = new Uri(string.Format("http://{0}/".FmtWith(server)));
                    Uri requestUri = new Uri(baseAddress, relativeUri);

                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

                    HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);

                    Stream responseStream = await responseMessage.Content.ReadAsStreamAsync();
                    if (responseMessage.Content.Headers.ContentEncoding.Select(e => e.ToLower()).Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (responseMessage.Content.Headers.ContentEncoding.Select(e => e.ToLower()).Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    string responseContent = reader.ReadToEnd();

                    // 如果出现问题，json解析会出现问题，则不能直接解析json
                    try
                    {
                        json.Add(server, JObject.FromObject(new { Code = (int)responseMessage.StatusCode, Config = JObject.Parse(responseContent) }));
                    }
                    catch (Exception)
                    {
                        json.Add(server, JObject.FromObject(new { Code = (int)responseMessage.StatusCode, Config = responseContent }));
                    }

                    responseStream.Close();
                    reader.Close();
                }
            }

            return this.Ok(json);
        }
    }
}