// FileInformation: nyanya/nyanya.AspDotNet.Common/OrderedFilters.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:12 AM

using System.Web.Http.Filters;

namespace nyanya.AspDotNet.Common.Filters
{
    public interface IOrderedFilter : IFilter
    {
        int Order { get; set; }
    }

    public class OrderedActionFilterAttribute : ActionFilterAttribute, IOrderedFilter
    {
        #region IOrderedFilter Members

        public int Order { get; set; }

        #endregion IOrderedFilter Members
    }

    public class OrderedAuthorizationFilterAttribute : AuthorizationFilterAttribute, IOrderedFilter
    {
        #region IOrderedFilter Members

        public int Order { get; set; }

        #endregion IOrderedFilter Members
    }

    public class OrderedExceptionFilterAttribute : ExceptionFilterAttribute, IOrderedFilter
    {
        #region IOrderedFilter Members

        public int Order { get; set; }

        #endregion IOrderedFilter Members
    }
}