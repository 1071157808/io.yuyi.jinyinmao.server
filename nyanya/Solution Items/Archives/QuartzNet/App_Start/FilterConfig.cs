// FileInformation: nyanya/Services.WebAPI.V1.nyanya/FilterConfig.cs
// CreatedTime: 2014/03/31   1:21 AM
// LastUpdatedTime: 2014/04/21   10:49 PM

using System.Web.Mvc;

namespace QuartzNet
{
    /// <summary>
    ///     FilterConfig
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        ///     Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}