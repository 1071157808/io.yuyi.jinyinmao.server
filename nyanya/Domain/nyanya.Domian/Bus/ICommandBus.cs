// FileInformation: nyanya/Domian/ICommandBus.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/08/06   3:15 PM

using Domian.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domian.Bus
{
    public interface ICommandBus : IBus
    {
        void Excute<T>(T command) where T : ICommand;

        ObjectCommandResult ResultExcute<T>(T command) where T : ICommand;

        void Excute<T>(IEnumerable<T> commands) where T : ICommand;

        ObjectCommandResult ObjectExcute<T>(T command) where T : IObjectCommand;
    }
}