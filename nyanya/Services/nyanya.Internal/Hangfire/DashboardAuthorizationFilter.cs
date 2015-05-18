// FileInformation: nyanya/nyanya.Internal/DashboardAuthorizationFilter.cs
// CreatedTime: 2014/08/28   5:19 PM
// LastUpdatedTime: 2014/09/11   1:42 PM

using System.Collections.Generic;
using Hangfire.Dashboard;
using Microsoft.Owin;

namespace nyanya.Internal.Hangfire
{
    /// <summary>
    ///     DashboardAuthorizationFilter
    /// </summary>
    public class DashboardAuthorizationFilter : IAuthorizationFilter
    {
        #region IAuthorizationFilter Members

        /// <summary>
        ///     Authorizes the specified owin environment.
        /// </summary>
        /// <param name="owinEnvironment">The owin environment.</param>
        /// <returns></returns>
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            OwinContext context = new OwinContext(owinEnvironment);
            return context.Request.Query.Get("token").ToUpperInvariant() == "HANGFIRE";
        }

        #endregion IAuthorizationFilter Members
    }
}