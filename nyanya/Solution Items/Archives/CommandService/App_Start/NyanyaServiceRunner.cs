// FileInformation: nyanya/CommandService/NyanyaServiceRunner.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/07/30   5:29 PM

using System;
using Cqrs.Domain.Config;
using Infrastructure.Lib.Logs;
using Infrastructure.Lib.Utility;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Web;

namespace CommandService.App_Start
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
            get { return this.config.CommandHandlerLogger; }
        }

        public override object HandleException(IRequest request, T requestDto, Exception ex)
        {
            this.Logger.LogError(request, ex);
            return base.HandleException(request, requestDto, ex);
        }

        // GlobalRequestFilter => 记录原始请求
        // requestDto = null(未绑定，未验证),req.Response和res都是默认值，200 ok
        // 未考虑出现异常的情况
        //
        // ModelValidation
        // Excepion => 跳过所有环节 => 客户端异常
        //
        // BeforeEachRequest 内部调用 OnBeforeExecute => 记录模型绑定后的请求
        // requestContext.Dto == request != null 已经进行了模型绑定和模型验证,response 还是默认值，200 ok
        // 未考虑出现异常的情况
        //
        // Action => 一定要返回Task<T> 而不能是Task => 自身内创建CommandLog，并且记录错误信息
        //
        // HandleException => 无效果
        //
        // AfterEachRequest 内部调用 OnAfterExecute
        // requestContext.Dto == request != null 已经进行了模型绑定和模型验证,response => Task, WaitingForActivation
        // 未考虑出现异常的情况
        //
        // 执行Action中返回的Task
        // Excepion => UncaughtExceptionHandler => 客户端的返回值为null => 不会再经过任何filter
        // UncaughtExceptionHandler中req的Response和res都是成功值,operationName是Dto类型名 => 记录特殊异常
        //
        // GlobalResponseFilter
        // Task已经执行完成，并且没有异常。res.Dto == null,但是dto已经有数据了
        public override void OnBeforeExecute(IRequest requestContext, T request)
        {
            this.Logger.LogRequestDto(requestContext);
            base.OnBeforeExecute(requestContext, request);
        }
    }
}