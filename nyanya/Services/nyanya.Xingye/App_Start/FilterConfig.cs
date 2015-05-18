// FileInformation: nyanya/nyanya.Xingye/FilterConfig.cs
// CreatedTime: 2014/09/01   10:08 AM
// LastUpdatedTime: 2014/09/01   10:10 AM

using System.Web.Mvc;

namespace nyanya.Xingye
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