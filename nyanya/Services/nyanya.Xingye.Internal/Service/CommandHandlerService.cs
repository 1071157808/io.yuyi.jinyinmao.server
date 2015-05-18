// FileInformation: nyanya/nyanya.Xingye.Internal/CommandHandlerService.cs
// CreatedTime: 2014/09/03   10:05 AM
// LastUpdatedTime: 2014/09/03   10:09 AM

using System;
using System.Threading.Tasks;
using Domian.Commands;
using Domian.Config;
using Domian.Logs;
using Infrastructure.Lib;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Logs;
using Infrastructure.SMS;

namespace nyanya.Xingye.Internal.Service
{
    public partial class CommandHandlerService : ServiceStack.Service
    {
        private readonly CqrsConfiguration config;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandHandlerService" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public CommandHandlerService(CqrsConfiguration config)
        {
            this.config = config;
        }

        private ISmsAlertsService AlertService
        {
            get { return this.config.SmsAlertsService; }
        }

        private ILogger CommandHandlerLogger
        {
            get { return this.config.CommandHandlerLogger; }
        }

        private ICommandHandlers CommandHandlers
        {
            get { return this.config.CommandHandlers; }
        }

        private ICommandLogStore CommandLogStore
        {
            get { return this.config.CommandLogStore; }
        }

        private async Task<CommandResult> Handler<T>(T command) where T : ICommand
        {
            this.CommandLogStore.Create(command);
            this.CommandHandlerLogger.Info("Begin => {0}".FmtWith(command.CommandId));
            try
            {
                ICommandHandler<T> handler = this.CommandHandlers.GetHandler<T>();
                if (handler.IsNull())
                {
                    throw new CqrsConfigException(NyanyaResources.CommandService_CanNotFindCommandHandler, command.GetType().ToString());
                }

                await handler.Handler(command);
            }
            catch (CommandExcuteFaildException e)
            {
                this.CommandLogStore.Handled(command.CommandId, false, "{0} {1}".FmtWith(e.FriendlyMessage, e.Message));
                this.CommandHandlerLogger.Error(e, e.Message);
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_CommandService_CommandExcuteFaildException.FmtWith(command.CommandId, e.FriendlyMessage, e.Message));
                this.CommandHandlerLogger.Info("Error => {0}".FmtWith(command.CommandId));
                return new CommandResult(command.CommandId, false, e.FriendlyMessage);
            }
            catch (Exception e)
            {
                this.CommandLogStore.Handled(command.CommandId, false, e.Message);
                this.CommandHandlerLogger.Error(e, e.Message);
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_CommandService_UnexpectedException.FmtWith(e.GetType(), command.CommandId, e.Message));
                this.CommandHandlerLogger.Info("Error => {0}".FmtWith(command.CommandId));
                return new CommandResult(command.CommandId, false, "系统错误");
            }

            this.CommandLogStore.Handled(command.CommandId);
            this.CommandHandlerLogger.Info("End => {0}".FmtWith(command.CommandId));
            return new CommandResult(command.CommandId);
        }
    }
}