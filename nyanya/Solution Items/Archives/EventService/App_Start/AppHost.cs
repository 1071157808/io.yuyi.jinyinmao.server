// FileInformation: nyanya/EventService/AppHost.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/14   9:49 AM

using System.Configuration;
using Cqrs.Domain.Config;
using Funq;
using Infrastructure.Lib.Logs;
using Infrastructure.SMS;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;
using ServiceStack.Text;
using ServiceStack.Validation;

namespace EventService.App_Start
{
    /// <summary>
    ///     AppHost
    /// </summary>
    public class AppHost : AppHostBase
    {
        #region Private Fields

        private readonly CqrsConfiguration config = new CqrsConfiguration();

        #endregion Private Fields

        #region Public Constructors

        //Tell ServiceStack the name of your application and where to find your services
        /// <summary>
        ///     Initializes a new instance of the <see cref="AppHost" /> class.
        /// </summary>
        public AppHost()
            : base("Event Handler Services", typeof(EventHandlerService).Assembly)
        {
            CQRSConfig.Configure(this.config);
        }

        #endregion Public Constructors

        #region Private Properties

        private ILogger Logger
        {
            get { return this.config.EventHandlerLogger; }
        }

        #endregion Private Properties

        #region Public Methods

        public override void Configure(Container container)
        {
            //register any dependencies your services use, e.g:
            //container.Register<ICacheClient>(new MemoryCacheClient());
            //container.Register<IValidator<AddTheUser>>(new AddTheUserValidator());

            //register also can use c.Resolve<IDependency>(),e.g:
            //container.Register(c => new MyType(c.Resolve<IDependency>(), connectionString));
            container.Register(this.config);
            container.Register(this.config.EventHandlers);
            container.Register(this.config.EventHandlerLogger);
            container.Register<ISmsService>(new SmsService());

            //FluentValidation for request dtos
            this.Plugins.Add(new ValidationFeature());

            // Json Configure
            JsConfig.DateHandler = DateHandler.ISO8601;
            JsConfig.IncludeNullValues = true;
            JsConfig.TreatEnumAsInteger = true;

            // Configure Rabbit Message Queue
            this.ConfigureMqServer(container);
        }

        #endregion Public Methods

        #region Private Methods

        private void ConfigureMqServer(Container container)
        {
            RabbitMqServer mq = new RabbitMqServer(ConfigurationManager.AppSettings.Get("EventProcessorAddress"));
            mq.AutoReconnect = true;
            mq.DisablePriorityQueues = true;
            mq.RetryCount = 0;

            mq.RequestFilter = message =>
            {
                this.Logger.Info(message.GetType() + "|" + message.Body.ToJson());
                return message;
            };
            mq.ErrorHandler = exception => this.Logger.Fatal(exception, exception.Message);

            container.Register<IMessageService>(c => mq);

            IMessageService mqServer = container.Resolve<IMessageService>();
            DtoRegistry.RegisterDtos(this, mq, container, this.config);
            mqServer.Start();
        }

        #endregion Private Methods

        ///// <summary>
        /////     Create a service runner for IService actions
        ///// </summary>
        //public override IServiceRunner<TRequest> CreateServiceRunner<TRequest>(ActionContext actionContext)
        //{
        //    return new NyanyaServiceRunner<TRequest>(this, actionContext, this.config);
        //}
    }
}