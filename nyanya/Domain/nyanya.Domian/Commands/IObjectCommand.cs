using ServiceStack;
using System;

namespace Domian.Commands
{
    public interface IObjectCommand : IReturn<ObjectCommandResult>
    {
        Guid CommandId { get; }

        string Source { get; }
    }
}