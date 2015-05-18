// FileInformation: nyanya/nyanya.Internal/FilterConfig.cs
// CreatedTime: 2014/08/26   12:46 PM
// LastUpdatedTime: 2014/08/26   1:17 PM

using System.Web.Mvc;

namespace nyanya.Internal
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