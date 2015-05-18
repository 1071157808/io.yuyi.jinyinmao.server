namespace Domian.Commands
{
    public interface ICommandHandlers
    {
        void Clear();
        ICommandHandler<T> GetHandler<T>() where T : ICommand;

        /// <summary>
        ///     Registers a command handler into command handlers.
        /// </summary>
        /// <typeparam name="T">The type of the command.</typeparam>
        /// <param name="handler">The handler to be registered.</param>
        void Register<T>(ICommandHandler<T> handler) where T : ICommand;

        /// <summary>
        ///     Unregisters a command handler from the command handler.
        /// </summary>
        /// <typeparam name="T">The type of the command.</typeparam>
        void UnRegister<T>() where T : ICommand;
    }
}