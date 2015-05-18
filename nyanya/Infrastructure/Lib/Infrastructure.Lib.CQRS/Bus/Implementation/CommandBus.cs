// FileInformation: nyanya/Infrastructure.Lib.CQRS/CommandBus.cs
// CreatedTime: 2014/06/30   2:14 PM
// LastUpdatedTime: 2014/07/01   10:03 AM

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Lib.CQRS.Config;
using Infrastructure.Lib.CQRS.Exceptions;
using Infrastructure.Lib.CQRS.Log;
using Infrastructure.Lib.CQRS.MessageLogs;
using Infrastructure.Lib.CQRS.Messages;

namespace Infrastructure.Lib.CQRS.Bus.Implementation
{
    public class CommandBus : DisposableObject, ICommandBus
    {
        private readonly CommandLogStore commandLogStore;
        private readonly ICommandDispatcher dispatcher;
        private readonly ILogger logger;

        public CommandBus(ICommandDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            //this.logger = CqrsConfigration.Loggers.CommandBusLogger;
            this.commandLogStore = new CommandLogStore();
        }

        #region ICommandBus Members

        public async Task<bool> Send<T>(T command, ExecutionMode mode = ExecutionMode.RequestAndResponse) where T : ICommand
        {
            bool result;

            try
            {
                //await this.commandLogStore.Create(command);
                switch (mode)
                {
                    case ExecutionMode.RequestAndResponse:
                        this.DoSend(command, mode);
                        break;

                    case ExecutionMode.FireAndForget:
                        throw new NotImplementedException();
                    default:
                        throw new UnexpectedCodePathException();
                }
                result = true;
            }
            catch (Exception e)
            {
                this.logger.Fatal(e, "Unknown Exception During Sending Command(CommandGuid: {0}, ExecutionMode: {1}).\n{2}", command.CommandId, mode, e.Message);
                result = false;
            }
            return result;
        }

        public Task<bool> Send<T>(IEnumerable<T> commands, ExecutionMode mode = ExecutionMode.RequestAndResponse) where T : ICommand
        {
            throw new NotImplementedException();
        }

        #endregion ICommandBus Members

        /// <summary>
        ///     Disposes the object.
        /// </summary>
        /// <param name="disposing">
        ///     A <see cref="System.Boolean" /> value which indicates whether
        ///     the object should be disposed explicitly.
        /// </param>
        protected override void Dispose(bool disposing)
        {
        }

        private void DoSend<T>(T command, ExecutionMode mode = ExecutionMode.RequestAndResponse) where T : ICommand
        {
            if (mode == ExecutionMode.FireAndForget)
            {
                // TODO 实现 FireAndForget 模式
                throw new NotImplementedException();
            }
            try
            {
                this.dispatcher.Dispatch(command);
                this.logger.Info("Sended Command {0}.", command.CommandId);
            }
            catch (Exception e)
            {
                this.logger.Error(e, "Exception During Sending Command(CommandGuid: {0}, ExecutionMode: {1}).\n{2}", command.CommandId, mode, e.Message);
            }
        }
    }
}