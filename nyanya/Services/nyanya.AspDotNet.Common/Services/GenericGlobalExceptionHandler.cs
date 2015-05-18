// FileInformation: nyanya/nyanya.AspDotNet.Common/GenericGlobalExceptionHandler.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:18 AM

using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace nyanya.AspDotNet.Common.MessageHandlers
{
    public class GenericGlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new InternalServerErrorResult(context.Request);
        }
    }
}