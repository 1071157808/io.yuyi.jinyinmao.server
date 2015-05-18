// FileInformation: nyanya/Services.WebAPI.Common/OrderedFilters.cs
// CreatedTime: 2014/04/02   2:46 PM
// LastUpdatedTime: 2014/04/02   3:25 PM

using System.Web.Http.Filters;

namespace Services.WebAPI.Common.Filters
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