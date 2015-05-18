// FileInformation: nyanya/Domian/EventResult.cs
// CreatedTime: 2014/07/12   3:03 PM
// LastUpdatedTime: 2014/07/12   3:05 PM

using System;
using System.Runtime.ConstrainedExecution;

namespace Domian.Events
{
    public class EventResult
    {
        public Guid EventId { get; set; }

        public DateTime HandlerTime { get; set; }

        public string Message { get; set; }

        public bool Result { get; set; }

        public static EventResult Failed(IEvent @event, string message)
        {
            return new EventResult
            {
                Result = false,
                EventId = @event.EventId,
                Message = message,
                HandlerTime = DateTime.Now
            };
        }

        public static EventResult Successed(IEvent @event)
        {
            return new EventResult
            {
                Result = true,
                EventId = @event.EventId,
                Message = "",
                HandlerTime = DateTime.Now
            };
        }
    }
}