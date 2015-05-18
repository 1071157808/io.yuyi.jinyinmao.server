// FileInformation: nyanya/Infrastructure.Lib.CQRS/ICommandHandler.cs
// CreatedTime: 2014/06/10   4:09 PM
// LastUpdatedTime: 2014/06/11   2:05 PM

using System.Threading.Tasks;
using Infrastructure.Lib.CQRS.Bus;

namespace Infrastructure.Lib.CQRS.Messages
{
    public interface ICommandHandler
    {
        string Name { get; }
    }

    public interface ICommandHandler<in T> : ICommandHandler where T : ICommand
    {
        /// <summary>
        ///     Handles the specified command.
        /// </summary>
        /// <param name="command">The command to be handled.</param>
        Task Handle(T command);
    }
}