// FileInformation: nyanya/nyanya.Internal/NyanyaServiceRunner.cs
// CreatedTime: 2014/08/27   5:45 PM
// LastUpdatedTime: 2014/08/28   10:50 AM

using System;
using Domian.Commands;
using Domian.Config;
using Infrastructure.Lib.Logs;
using Infrastructure.Lib.Utility;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Web;

namespace nyanya.Internal.Service.Config
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
            if (requestDto is Command)
            {
                this.Logger.Fatal(ex, ex.Message);
            }

            return base.HandleException(request, requestDto, ex);
        }

        public override void OnBeforeExecute(IRequest requestContext, T request)
        {
            if (request is Command)
            {
                this.Logger.Info(request.GetType() + "|" + request.ToJson());
            }

            base.OnBeforeExecute(requestContext, request);
        }
    }
}