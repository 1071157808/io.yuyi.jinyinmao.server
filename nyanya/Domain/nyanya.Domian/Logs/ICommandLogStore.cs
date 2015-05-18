// FileInformation: nyanya/Domian/ICommandLogStore.cs
// CreatedTime: 2014/07/06   8:43 PM
// LastUpdatedTime: 2014/07/12   10:07 PM

using System;
using Domian.Commands;

namespace Domian.Logs
{
    public interface ICommandLogStore
    {
        void Create<T>(T command) where T : ICommand;

        void Handled(Guid commandId, bool successed = true, string message = "");
    }
}