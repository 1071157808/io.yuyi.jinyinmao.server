// FileInformation: nyanya/Infrastructure.Lib.CQRS/CommandLog.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/07/01   1:27 PM

using System;

namespace Infrastructure.Lib.CQRS.MessageLogs
{
    public class CommandLog
    {
        public Guid Guid { get; set; }

        public bool? Handled { get; set; }

        public DateTime? HandledTime { get; set; }

        public string Name { get; set; }

        public string Payload { get; set; }

        public string Source { get; set; }
    }
}