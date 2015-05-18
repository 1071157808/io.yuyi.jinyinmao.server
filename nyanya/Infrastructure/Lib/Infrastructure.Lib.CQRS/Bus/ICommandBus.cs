// FileInformation: nyanya/Infrastructure.Lib.CQRS/ICommandBus.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/07/06   7:58 PM

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Lib.CQRS.Bus
{
    public enum ExecutionMode
    {
        RequestAndResponse = 0,
        FireAndForget = 1
    }

    public interface ICommandBus : IBus
    {
        Task<bool> Send<T>(T command, ExecutionMode mode = ExecutionMode.RequestAndResponse) where T : ICommand;

        Task<bool> Send<T>(IEnumerable<T> commands, ExecutionMode mode = ExecutionMode.RequestAndResponse) where T : ICommand;
    }
}