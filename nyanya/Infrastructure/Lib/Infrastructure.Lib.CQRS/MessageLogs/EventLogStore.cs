// FileInformation: nyanya/Infrastructure.Lib.CQRS/EventLogStore.cs
// CreatedTime: 2014/06/13   1:33 PM
// LastUpdatedTime: 2014/06/13   7:37 PM

using System.Configuration;

namespace Infrastructure.Lib.CQRS.MessageLogs
{
    public class EventLogStore : MessageLogStore
    {
        private static readonly string logName;
        private static readonly string mySqlConnectionString;

        static EventLogStore()
        {
            mySqlConnectionString = ConfigurationManager.ConnectionStrings["EventLogStore"].ToString();
            logName = "EventLogs";
        }

        public EventLogStore()
            : base(logName, mySqlConnectionString)
        {
        }
    }
}