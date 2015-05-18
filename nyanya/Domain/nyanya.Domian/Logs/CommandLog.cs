// FileInformation: nyanya/Domian/CommandLog.cs
// CreatedTime: 2014/07/06   8:43 PM
// LastUpdatedTime: 2014/07/06   11:33 PM

using System;

namespace Domian.Logs
{
    public class CommandLog
    {
        public Guid CommandId { get; set; }

        public bool? Handled { get; set; }

        public DateTime? HandledTime { get; set; }

        public string Message { get; set; }

        public string Name { get; set; }

        public string Payload { get; set; }

        public DateTime ReceiveTime { get; set; }

        public string Source { get; set; }
    }
}