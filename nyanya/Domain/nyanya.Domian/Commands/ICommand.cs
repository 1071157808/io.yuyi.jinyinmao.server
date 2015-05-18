// FileInformation: nyanya/Domian/ICommand.cs
// CreatedTime: 2014/07/06   7:45 PM
// LastUpdatedTime: 2014/07/13   1:37 AM

using ServiceStack;
using System;

namespace Domian.Commands
{
    public interface ICommand : IReturn<CommandResult>
    {
        Guid CommandId { get; }

        string Source { get; }
    }
}