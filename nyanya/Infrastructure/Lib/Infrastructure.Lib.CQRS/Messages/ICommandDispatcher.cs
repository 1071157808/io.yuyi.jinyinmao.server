// FileInformation: nyanya/Infrastructure.Lib.CQRS/ICommandDispatcher.cs
// CreatedTime: 2014/06/08   5:15 PM
// LastUpdatedTime: 2014/06/09   1:01 AM

using Infrastructure.Lib.CQRS.Bus;

namespace Infrastructure.Lib.CQRS.Messages
{
    public interface ICommandDispatcher
    {
        /// <summary>
        ///     Dispatches the command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command to be dispatched.</param>
        /// <returns>Dispatch Successed</returns>
        bool Dispatch<T>(T command) where T : ICommand;

        /// <summary>
        ///     Registers a command handler into command dispatcher.
        /// </summary>
        /// <typeparam name="T">The type of the command.</typeparam>
        /// <param name="handler">The handler to be registered.</param>
        void Register<T>(ICommandHandler<T> handler) where T : ICommand;

        /// <summary>
        ///     Unregisters a command handler from the command dispatcher.
        /// </summary>
        /// <typeparam name="T">The type of the command.</typeparam>
        void UnRegister<T>() where T : ICommand;
    }
}