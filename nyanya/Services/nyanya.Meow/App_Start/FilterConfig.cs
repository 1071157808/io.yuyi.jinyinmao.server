// FileInformation: nyanya/nyanya.Meow/FilterConfig.cs
// CreatedTime: 2014/08/28   6:33 PM
// LastUpdatedTime: 2014/08/29   2:12 PM

using System.Web.Mvc;

namespace nyanya.Meow
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