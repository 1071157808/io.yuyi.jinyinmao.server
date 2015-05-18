// FileInformation: nyanya/Infrastructure.Lib.CQRS/MessageLog.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/06/19   9:18 AM

using System;

namespace Infrastructure.Lib.CQRS.MessageLogs
{
    internal class MessageLog : IMessageLog
    {
        #region IMessageLog Members

        public DateTime BuildTime { get; set; }

        public bool? Delivered { get; set; }

        public DateTime? DeliveredTime { get; set; }

        public Guid Guid { get; set; }

        public bool? Handled { get; set; }

        public DateTime? HandledTime { get; set; }

        public string Name { get; set; }

        public string Payload { get; set; }

        public string Source { get; set; }

        #endregion IMessageLog Members
    }
}