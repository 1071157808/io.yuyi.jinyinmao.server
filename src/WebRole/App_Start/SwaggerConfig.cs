// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-28  12:44 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-28  1:21 AM
// ***********************************************************************
// <copyright file="SwaggerConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using WebRole;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebRole
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "WebRole");
                    c.IgnoreObsoleteActions();
                    c.IgnoreObsoleteProperties();
                    //c.OperationFilter<OperationFilter>();
                    c.IncludeXmlComments(Path.Combine(HttpRuntime.AppDomainAppPath, "bin", "Yuyi.Jinyinmao.Api.XML"));
                })
                .EnableSwaggerUi(c =>
                {
                    // Use the "InjectStylesheet" option to enrich the UI with one or more additional CSS stylesheets.
                    // The file must be included in your project as an "Embedded Resource", and then the resource's
                    // "Logical Name" is passed to the method as shown below.
                    //
                    //c.InjectStylesheet(containingAssembly, "Swashbuckle.Dummy.SwaggerExtensions.testStyles1.css");

                    // Use the "InjectJavaScript" option to invoke one or more custom JavaScripts after the swagger-ui
                    // has loaded. The file must be included in your project as an "Embedded Resource", and then the resource's
                    // "Logical Name" is passed to the method as shown above.
                    //
                    //c.InjectJavaScript(thisAssembly, "Swashbuckle.Dummy.SwaggerExtensions.testScript1.js");

                    // The swagger-ui renders boolean data types as a dropdown. By default, it provides "true" and "false"
                    // strings as the possible choices. You can use this option to change these to something else,
                    // for example 0 and 1.
                    //
                    //c.BooleanValues(new[] { "0", "1" });

                    // By default, swagger-ui will validate specs against swagger.io's online validator and display the result
                    // in a badge at the bottom of the page. Use these options to set a different validator URL or to disable the
                    // feature entirely.
                    //c.SetValidatorUrl("http://localhost/validator");
                    //c.DisableValidator();

                    // Use this option to control how the Operation listing is displayed.
                    // It can be set to "None" (default), "List" (shows operations for each resource),
                    // or "Full" (fully expanded: shows operations and their details).
                    //
                    //c.DocExpansion(DocExpansion.List);

                    // Use the CustomAsset option to provide your own version of assets used in the swagger-ui.
                    // It's typically used to instruct Swashbuckle to return your version instead of the default
                    // when a request is made for "index.html". As with all custom content, the file must be included
                    // in your project as an "Embedded Resource", and then the resource's "Logical Name" is passed to
                    // the method as shown below.
                    //
                    //c.CustomAsset("index", containingAssembly, "YourWebApiProject.SwaggerExtensions.index.html");

                    // If your API has multiple versions and you've applied the MultipleApiVersions setting
                    // as described above, you can also enable a select box in the swagger-ui, that displays
                    // a discovery URL for each version. This provides a convenient way for users to browse documentation
                    // for different API versions.
                    //
                    //c.EnableDiscoveryUrlSelector();

                    // If your API supports the OAuth2 Implicit flow, and you've described it correctly, according to
                    // the Swagger 2.0 specification, you can enable UI support as shown below.
                    //
                    //c.EnableOAuth2Support("test-client-id", "test-realm", "Swagger UI");
                });
        }
    }

    //    internal class OperationFilter : IOperationFilter
    //    {
    //        #region IOperationFilter Members
    //
    //        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
    //        {
    //            operation.consumes.Clear();
    //            operation.consumes.Add("application/json");
    //            operation.consumes.Add("text/json");
    //            operation.produces.Clear();
    //            operation.produces.Add("application/json");
    //            operation.produces.Add("text/json");
    //            operation.produces.Add("application/javascript");
    //            operation.produces.Add("application/json-p");
    //            operation.produces.Add("text/javascript");
    //        }
    //
    //        #endregion IOperationFilter Members
    //    }
}