// FileInformation: nyanya/CommandService/AppHost.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/27   5:44 PM

using Cqrs.Domain.Config;
using Funq;
using Infrastructure.Lib.Logs;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.MsgPack;
using ServiceStack.Text;
using ServiceStack.Validation;
using ServiceStack.Web;

namespace CommandService.App_Start
{
    /// <summary>
    ///     AppHost
    /// </summary>
    public class AppHost : AppHostBase
    {
        private readonly CqrsConfiguration config = new CqrsConfiguration();

        //Tell ServiceStack the name of your application and where to find your services
        /// <summary>
        ///     Initializes a new instance of the <see cref="AppHost" /> class.
        /// </summary>
        public AppHost()
            : base("Command Handler Services", typeof(CommandHandlerService).Assembly)
        {
            CQRSConfig.Configurate(this.config);
        }

        private ILogger Logger
        {
            get { return this.config.CommandHandlerLogger; }
        }

        public override void Configure(Container container)
        {
            //register any dependencies your services use, e.g:
            //container.Register<ICacheClient>(new MemoryCacheClient());
            //container.Register<IValidator<AddTheUser>>(new AddTheUserValidator());

            //register also can use c.Resolve<IDependency>(),e.g:
            //container.Register(c => new MyType(c.Resolve<IDependency>(), connectionString));
            container.Register(this.config);

            //FluentValidation for request dtos
            //Plugins.Add(new CorsFeature(allowedMethods: "GET, POST"));
            this.Plugins.Add(new ValidationFeature());
            this.Plugins.Add(new MsgPackFormat());
            DtoRegistry.RegisterDtos(container, this.config);

            // Json Configure
            JsConfig.DateHandler = DateHandler.ISO8601;
            JsConfig.IncludeNullValues = true;
            JsConfig.TreatEnumAsInteger = true;

            //Add a request filter to check if the user has a session initialized
            //this.GlobalRequestFilters.Add((req, res, requestDto) => this.Logger.LogRequest(req));

            //Handle Unhandled Exceptions occurring outside of Services
            //E.g. Exceptions during Request binding or in filters:
            this.UncaughtExceptionHandlers.Add((req, res, operationName, ex) =>
            {
                this.Logger.LogFatalError(req, ex);
                res.EndRequest(true);
            });

            //Add a response filter to add a 'Content-Disposition' header so browsers treat it as a native .csv file
            //this.GlobalResponseFilters.Add((req, res, dto) => { this.LogToNlog(req); });

            ////Handle Exceptions occurring in Services:
            //// 还没发现用途
            //this.ServiceExceptionHandlers.Add((httpReq, request, exception) =>
            //{
            //    this.LogFatalErrorToNlog(httpReq, exception);
            //    //call default exception handler or prepare your own custom response
            //    return DtoUtils.CreateErrorResponse(request, exception);
            //});
        }

        /// <summary>
        ///     Create a service runner for IService actions
        /// </summary>
        public override IServiceRunner<TRequest> CreateServiceRunner<TRequest>(ActionContext actionContext)
        {
            return new NyanyaServiceRunner<TRequest>(this, actionContext, this.config);
        }
    }
}