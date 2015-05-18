// FileInformation: nyanya/Infrastructure.Lib.CQRS/IEvent.cs
// CreatedTime: 2014/06/12   9:58 AM
// LastUpdatedTime: 2014/06/24   11:37 AM

using System;

namespace Infrastructure.Lib.CQRS.Bus
{
    public interface IEvent : IMessage
    {
        Guid SourceGuid { get; }
    }
}