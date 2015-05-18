// FileInformation: nyanya/CommandService/Global.asax.cs
// CreatedTime: 2014/07/01   1:28 PM
// LastUpdatedTime: 2014/08/14   5:33 AM

using System;
using System.Web;
using CommandService.App_Start;

namespace CommandService
{
    /// <summary>
    ///     HttpApplication
    /// </summary>
    public class Global : HttpApplication
    {
        #region Protected Methods

        /// <summary>
        ///     Handles the AuthenticateRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Handles the BeginRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Handles the End event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Handles the Start event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }

        /// <summary>
        ///     Handles the End event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Handles the Start event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        #endregion Protected Methods
    }
}