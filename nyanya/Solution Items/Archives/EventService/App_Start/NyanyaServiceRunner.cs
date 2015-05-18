// FileInformation: nyanya/EventService/NyanyaServiceRunner.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/07/22   10:07 AM

using System;
using System.Text;
using Cqrs.Domain.Config;
using Infrastructure.Lib.Logs;
using Infrastructure.Lib.Utility;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Web;

namespace EventService.App_Start
{
    internal class NyanyaServiceRunner<T> : ServiceRunner<T>
    {
        private readonly CqrsConfiguration config;

        public NyanyaServiceRunner(IAppHost appHost, ActionContext actionContext, CqrsConfiguration config)
            : base(appHost, actionContext)
        {
            Guard.ArgumentNotNull(config, "CqrsConfiguration");
            this.config = config;
        }

        private ILogger Logger
        {
            get { return this.config.EventHandlerLogger; }
        }

        // mq未进入 GlobalRequestFilter
        //
        // ModelValidation
        // Excepion => 跳过所有环节 => mq dlq 其中包含验证信息和时间信息
        //
        // BeforeEachRequest 内部调用 OnBeforeExecute => 记录模型绑定后的请求
        // requestContext.Dto == request != null 已经进行了模型绑定和模型验证,response 还是默认值 mq的默认值 Dto == null StatusCode == 0
        // 未考虑出现异常的情况
        //
        // Action => 返回object => void
        // 执行Action，并且如果遇到异常 => 进入HandleException => ServiceExceptionHandler => mq
        //
        // HandleException => ServiceExceptionHandler => mq dlq(会重试一次) => 不经过其他filters
        //
        // AfterEachRequest 内部调用 OnAfterExecute => mq outq
        // requestContext.Dto == request != null 已经进行了模型绑定和模型验证,response => null
        // 未考虑出现异常的情况
        //
        // mq未进入 GlobalResponseFilter

        public override object HandleException(IRequest request, T requestDto, Exception ex)
        {
            this.LogErrorToNlog(request, ex);

            return base.HandleException(request, requestDto, ex);
        }

        public override void OnBeforeExecute(IRequest requestContext, T request)
        {
            this.LogToNlog(requestContext);

            base.OnBeforeExecute(requestContext, request);
        }

        private void LogErrorToNlog(IRequest request, Exception ex)
        {
            StringBuilder messageBuilder = new StringBuilder();

            if (request.AbsoluteUri != null)
            {
                messageBuilder.Append(" " + request.AbsoluteUri);
            }
            else
            {
                messageBuilder.Append(" -");
            }

            if (request.OperationName != null)
            {
                messageBuilder.Append(" " + request.OperationName);
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

            this.Logger.Error(ex, messageBuilder.ToString());
        }

        private void LogToNlog(IRequest requestContext)
        {
            StringBuilder messageBuilder = new StringBuilder();

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
                messageBuilder.Append(" " + requestContext.OperationName);
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

            this.Logger.Info(messageBuilder.ToString());
        }
    }
}