// FileInformation: nyanya/Services.WebAPI.Common/GenericGlobalExceptionHandler.cs
// CreatedTime: 2014/03/30   11:05 PM
// LastUpdatedTime: 2014/03/30   11:06 PM

using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Services.WebAPI.Common.MessageHandlers
{
    public class GenericGlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new InternalServerErrorResult(context.Request);
        }
    }
}