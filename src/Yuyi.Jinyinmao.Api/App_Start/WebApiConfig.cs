// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : WebApiConfig.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  13:41
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
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Log;

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
            config.Filters.Add(new GlobalExceptionFilterAttribute());

            config.UseNLog();

            //config.EnableSystemDiagnosticsTracing();

            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateFormatString = "G",
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Include
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
            this.policy = new CorsPolicy
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = true,
                PreflightMaxAge = 300L
            };

            this.policy.ExposedHeaders.Add("Set-Cookie");
            this.policy.ExposedHeaders.Add("Date");
            this.policy.ExposedHeaders.Add("X-JYM-Auth");
            this.policy.ExposedHeaders.Add("x-jym-auth");
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