// FileInformation: nyanya/Services.WebAPI.Common/OrderedFilterProvider.cs
// CreatedTime: 2014/04/02   2:45 PM
// LastUpdatedTime: 2014/04/02   3:25 PM

using Services.WebAPI.Common.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Services.WebAPI.Common.Providers
{
    public class OrderedFilterProvider : IFilterProvider
    {
        #region IFilterProvider Members

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            // controller-specific
            IEnumerable<FilterInfo> controllerSpecificFilters = this.OrderFilters(actionDescriptor.ControllerDescriptor.GetFilters(), FilterScope.Controller);

            // action-specific
            IEnumerable<FilterInfo> actionSpecificFilters = this.OrderFilters(actionDescriptor.GetFilters(), FilterScope.Action);

            return controllerSpecificFilters.Concat(actionSpecificFilters);
        }

        #endregion IFilterProvider Members

        private IEnumerable<FilterInfo> OrderFilters(IEnumerable<IFilter> filters, FilterScope scope)
        {
            return filters.OfType<IOrderedFilter>()
                .OrderBy(filter => filter.Order)
                .Select(instance => new FilterInfo(instance, scope));
        }
    }
}