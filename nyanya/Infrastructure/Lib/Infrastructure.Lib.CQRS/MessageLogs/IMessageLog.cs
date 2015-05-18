// FileInformation: nyanya/Infrastructure.Lib.CQRS/IMessageLog.cs
// CreatedTime: 2014/06/23   2:03 PM
// LastUpdatedTime: 2014/06/24   2:29 PM

using System;

namespace Infrastructure.Lib.CQRS.MessageLogs
{
    internal interface IMessageLog
    {
        DateTime BuildTime { get; set; }

        bool? Delivered { get; set; }

        DateTime? DeliveredTime { get; set; }

        Guid Guid { get; set; }

        bool? Handled { get; set; }

        DateTime? HandledTime { get; set; }

        string Name { get; }

        string Payload { get; set; }

        string Source { get; set; }
    }
}