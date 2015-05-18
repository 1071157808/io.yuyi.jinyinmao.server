// FileInformation: nyanya/Domian/IEvent.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/07/18   1:44 PM

using System;
using ServiceStack;

namespace Domian.Events
{
    public interface IEvent : IReturnVoid
    {
        Guid EventId { get; }

        string SourceId { get; }

        Type SourceType { get; set; }
    }
}