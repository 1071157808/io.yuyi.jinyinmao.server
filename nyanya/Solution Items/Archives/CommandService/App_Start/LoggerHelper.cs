// FileInformation: nyanya/CommandService/LoggerHelper.cs
// CreatedTime: 2014/07/30   5:20 PM
// LastUpdatedTime: 2014/07/30   5:29 PM

using System;
using System.Text;
using Infrastructure.Lib.Logs;
using ServiceStack;
using ServiceStack.Web;

namespace CommandService.App_Start
{
    internal static class LoggerHelper
    {
        internal static void LogError(this ILogger logger, IRequest request, Exception ex)
        {
            StringBuilder messageBuilder = new StringBuilder();
            if (request.RemoteIp != null)
            {
                messageBuilder.Append(" " + request.RemoteIp);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (request.Verb != null)
            {
                messageBuilder.Append(" " + request.Verb);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (request.AbsoluteUri != null)
            {
                messageBuilder.Append(" " + request.AbsoluteUri);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (request.Dto != null)
            {
                messageBuilder.Append(" " + request.Dto.ToJson());
            }
            else
            {
                messageBuilder.Append(" -");
            }

            logger.Error(ex, messageBuilder.ToString());
        }

        internal static void LogFatalError(this ILogger logger, IRequest request, Exception ex)
        {
            StringBuilder messageBuilder = new StringBuilder();
            if (request.RemoteIp != null)
            {
                messageBuilder.Append(" " + request.RemoteIp);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (request.Verb != null)
            {
                messageBuilder.Append(" " + request.Verb);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (request.AbsoluteUri != null)
            {
                messageBuilder.Append(" " + request.AbsoluteUri);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (request.Dto != null)
            {
                messageBuilder.Append(" " + request.Dto.ToJson());
            }
            else
            {
                messageBuilder.Append(" -");
            }

            logger.Fatal(ex, messageBuilder.ToString());
        }

        internal static void LogRequest(this ILogger logger, IRequest requestContext)
        {
            StringBuilder messageBuilder = new StringBuilder();
            if (requestContext.RemoteIp != null)
            {
                messageBuilder.Append(" " + requestContext.RemoteIp);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (requestContext.Verb != null)
            {
                messageBuilder.Append(" " + requestContext.Verb);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (requestContext.AbsoluteUri != null)
            {
                messageBuilder.Append(" " + requestContext.AbsoluteUri);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (requestContext.OperationName != null)
            {
                messageBuilder.Append(" " + requestContext.OperationName.ToJson());
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (requestContext.RawUrl != null)
            {
                messageBuilder.Append(" " + requestContext.RawUrl.ToJson());
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (requestContext.GetRawBody() != null)
            {
                messageBuilder.Append(" " + requestContext.GetRawBody().ToJson());
            }
            else
            {
                messageBuilder.Append(" -");
            }

            logger.Info(messageBuilder.ToString());
        }

        internal static void LogRequestDto(this ILogger logger, IRequest requestContext)
        {
            StringBuilder messageBuilder = new StringBuilder();
            if (requestContext.RemoteIp != null)
            {
                messageBuilder.Append(" " + requestContext.RemoteIp);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (requestContext.Verb != null)
            {
                messageBuilder.Append(" " + requestContext.Verb);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (requestContext.AbsoluteUri != null)
            {
                messageBuilder.Append(" " + requestContext.AbsoluteUri);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (requestContext.OperationName != null)
            {
                messageBuilder.Append(" " + requestContext.OperationName.ToJson());
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (requestContext.Dto != null)
            {
                messageBuilder.Append(" " + requestContext.Dto.ToJson());
            }
            else
            {
                messageBuilder.Append(" -");
            }

            logger.Info(messageBuilder.ToString());
        }
    }
}