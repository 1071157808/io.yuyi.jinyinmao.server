// FileInformation: nyanya/Infrastructure.Lib.CQRS/ICommand.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/07/06   7:52 PM

using System;
using System.Collections.Generic;

namespace Infrastructure.Lib.CQRS.Bus
{
    public interface ICommand
    {
        Guid CommandId { get; }

        Dictionary<string, string> Headers { get; }

        string Payload { get; }

        string Source { get; }
    }
}