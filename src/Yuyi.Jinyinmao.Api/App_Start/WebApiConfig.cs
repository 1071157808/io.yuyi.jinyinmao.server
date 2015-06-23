// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-23  1:11 PM
// ***********************************************************************
// <copyright file="WebApiConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using Moe.AspNet;
using Moe.AspNet.MessageHandlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting.Jsonp;
using Yuyi.Jinyinmao.Api.Log;

namespace Yuyi.Jinyinmao.Api
{
    /// <summary>
    ///     Class WebApiConfig.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        ///     Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            config.UseOrderedFilter();
            config.UseSeriLog();

            config.EnableSystemDiagnosticsTracing();

            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    NullValueHandling = NullValueHandling.Include,
                    DateFormatString = "G",
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Formatting = Formatting.None,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            config.Formatters.Clear();
            config.Formatters.Add(formatter);
            config.AddJsonpFormatter(formatter);

            config.Routes.MapHttpBatchRoute("WebApiBatch", "$batch", new BatchHandler(GlobalConfiguration.DefaultServer));
            config.MapHttpAttributeRoutes();

            config.SetCorsPolicyProviderFactory(new CorsPolicyFactory());
            config.EnableCors();

            NinjectConfig.RegisterDependencyResolver(config);
        }
    }

    /// <summary>
    ///     CorsPolicyFactory.
    /// </summary>
    public class CorsPolicyFactory : ICorsPolicyProviderFactory
    {
        private readonly ICorsPolicyProvider provider = new CorsPolicyProvider();

        #region ICorsPolicyProviderFactory Members

        /// <summary>
        ///     Gets the cors policy provider.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ICorsPolicyProvider.</returns>
        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            return this.provider;
        }

        #endregion ICorsPolicyProviderFactory Members
    }

    /// <summary>
    ///     CorsPolicyProvider.
    /// </summary>
    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        private readonly CorsPolicy policy;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CorsPolicyProvider" /> class.
        /// </summary>
        public CorsPolicyProvider()
        {
            // Create a CORS policy.
            this.policy = new CorsPolicy();

            this.policy.ExposedHeaders.Add("x-jym");
            this.policy.ExposedHeaders.Add("Set-Cookie");
            this.policy.ExposedHeaders.Add("Date");

            this.policy.Headers.Add("x-jym");
            this.policy.Headers.Add("content-type");
            this.policy.Headers.Add("accept");
            this.policy.Headers.Add("set-cookie");
            this.policy.Headers.Add("date");

            this.policy.Origins.Add("http://localhost:8100");
            this.policy.Origins.Add("http://*.jinyinmao.com.cn");
            this.policy.Origins.Add("http://*.yuyidev.com");
            this.policy.Origins.Add("https://*.jinyinmao.com.cn");
            this.policy.Origins.Add("https://*.yuyidev.com");

            this.policy.Methods.Add("GET");
            this.policy.Methods.Add("POST");
            this.policy.Methods.Add("PUT");
            this.policy.Methods.Add("DELETE");

            this.policy.PreflightMaxAge = 1000000000L;

            this.policy.SupportsCredentials = true;
        }

        #region ICorsPolicyProvider Members

        /// <summary>
        ///     Gets the <see cref="T:System.Web.Cors.CorsPolicy" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:System.Web.Cors.CorsPolicy" />.
        /// </returns>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.policy);
        }

        #endregion ICorsPolicyProvider Members
    }
}