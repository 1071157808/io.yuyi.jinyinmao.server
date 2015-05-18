// FileInformation: nyanya/nyanya.Internal/HelpController.cs
// CreatedTime: 2014/09/01   10:42 AM
// LastUpdatedTime: 2014/09/09   11:37 AM

using System;
using System.Web.Http;
using System.Web.Mvc;
using Infrastructure.Lib.Extensions;
using nyanya.Internal.Areas.HelpPage.Models;

namespace nyanya.Internal.Areas.HelpPage.Controllers
{
    /// <summary>
    ///     The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HelpController" /> class.
        /// </summary>
        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HelpController" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public HelpController(HttpConfiguration config)
        {
            this.Configuration = config;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        ///     Gets the configuration.
        /// </summary>
        /// <value>
        ///     The configuration.
        /// </value>
        public HttpConfiguration Configuration { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///     APIs the specified API identifier.
        /// </summary>
        /// <param name="apiId">The API identifier.</param>
        /// <returns></returns>
        public ActionResult Api(string apiId)
        {
            if (!this.IsLocalhostOrDev())
            {
                return this.Redirect("/");
            }

            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = this.Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return this.View("Error");
        }

        /// <summary>
        ///     Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!this.IsLocalhostOrDev())
            {
                return this.Redirect("/");
            }

            this.ViewBag.DocumentationProvider = this.Configuration.Services.GetDocumentationProvider();
            return this.View(this.Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        #endregion Public Methods
    }
}