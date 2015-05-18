// FileInformation: nyanya/Infrastructure.Lib.CQRS/CommandHandlerBase.cs
// CreatedTime: 2014/07/01   1:28 PM
// LastUpdatedTime: 2014/07/07   1:23 AM

using System;
using Infrastructure.Lib.CQRS.Bus;
using Infrastructure.Lib.CQRS.Log;
using Infrastructure.Lib.CQRS.MessageLogs;

namespace Infrastructure.Lib.CQRS.Messages.Implementation
{
    public abstract class CommandHandlerBase
    {
        private readonly CommandLogStore commandLogStore;
        private readonly ILogger logger;

        protected CommandHandlerBase()
        {
            this.commandLogStore = new CommandLogStore();
            //this.logger = CqrsConfigration.Loggers.CommandHandlerLogger;
        }

        public string Name
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }

        protected void OnHandled(ICommand command)
        {
            // ReSharper disable once CSharpWarnings::CS4014
            // this.commandLogStore.Handled(command);
            this.logger.Info("Handled Command {0} With Handler {1}.", command.CommandId, this.Name);
        }

        protected void OnHandledFailed(ICommand command, Exception e)
        {
            // ReSharper disable once CSharpWarnings::CS4014
            // this.commandLogStore.Handled(command, false);
            this.logger.Error(e, "Exception Duraing Handling Command {0} With Handler {1}.\n{2}", command.CommandId, this.Name, e.Message);
        }
    }
}