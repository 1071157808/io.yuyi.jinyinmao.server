// ***********************************************************************
// Project          : nyanya
// Author           : Siqi Lu
// Created          : 2015-03-04  6:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-14  10:49 AM
// ***********************************************************************
// <copyright file="AppHost.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Configuration;
using Domian.Config;
using Funq;
using Infrastructure.Lib.Logs;
using Infrastructure.SMS;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;
using ServiceStack.Text;
using ServiceStack.Validation;
using ServiceStack.Web;

namespace nyanya.Internal.Service.Config
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
            : base("nyanya Internal Services", typeof(AppHost).Assembly)
        {
            CQRSConfig.Configurate(this.config);
        }

        private ILogger CommandHandlerLogger
        {
            get { return this.config.CommandHandlerLogger; }
        }

        private ILogger EventHandlerLogger
        {
            get { return this.config.EventHandlerLogger; }
        }

        /// <summary>
        ///     Configures the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public override void Configure(Container container)
        {
            this.SetConfig(new HostConfig { HandlerFactoryPath = "Service" });

            container.Register(this.config);
            container.Register<ISmsService>(new SmsService());

            this.Plugins.Add(new ValidationFeature());
            DtoRegistry.RegisterCommandDtos(container, this.config);

            JsConfig.DateHandler = DateHandler.ISO8601;
            JsConfig.IncludeNullValues = true;
            JsConfig.TreatEnumAsInteger = true;

            this.UncaughtExceptionHandlers.Add((req, res, operationName, ex) =>
            {
                this.CommandHandlerLogger.Fatal(ex, ex.Message);
                res.EndRequest(true);
            });

            this.ConfigureMqServer(container);
        }

        /// <summary>
        ///     Create a service runner for IService actions
        /// </summary>
        public override IServiceRunner<TRequest> CreateServiceRunner<TRequest>(ActionContext actionContext)
        {
            return new NyanyaServiceRunner<TRequest>(this, actionContext, this.config);
        }

        private void ConfigureMqServer(Container container)
        {
            RabbitMqServer mq = new RabbitMqServer(ConfigurationManager.AppSettings.Get("EventProcessorAddress"));
            mq.AutoReconnect = true;
            mq.DisablePriorityQueues = true;
            mq.RetryCount = 0;

            mq.RequestFilter = message =>
            {
                this.EventHandlerLogger.Info(message.GetType() + "|" + message.Body.ToJson());
                return message;
            };
            mq.ErrorHandler = exception => this.EventHandlerLogger.Fatal(exception, exception.Message);

            container.Register<IMessageService>(c => mq);

            IMessageService mqServer = container.Resolve<IMessageService>();
            DtoRegistry.RegisterDtos(this, mq, container, this.config);
            mqServer.Start();
        }
    }
}