// FileInformation: nyanya/Infrastructure.Lib.CQRS/IMessage.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/06/24   11:49 AM

using System;

namespace Infrastructure.Lib.CQRS.Bus
{
    public interface IMessage
    {
        Guid Guid { get; }

        string Source { get; }
    }
}