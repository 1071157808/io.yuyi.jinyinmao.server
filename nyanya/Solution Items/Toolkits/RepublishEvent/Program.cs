// FileInformation: nyanya/RepublishEvent/Program.cs
// CreatedTime: 2014/09/11   10:33 AM
// LastUpdatedTime: 2014/09/25   9:46 AM

using System.Configuration;
using System.Linq;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql.SqlDialects;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;

namespace RepublishEvent
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string sourceId = "47837e0852af422c9e710148ac0a85de";
            IStoreEvents store = Wireup.Init()
                .UsingSqlPersistence("EventStore")
                .WithDialect(new MsSqlDialect())
                .WithStreamIdHasher(arg => arg ?? "Anonymous")
                .InitializeStorageEngine()
                .UsingJsonSerialization()
                .UsingAsynchronousDispatchScheduler()
                .DispatchTo(new Dispatcher())
                .Build();

            RabbitMqServer mqServer = new RabbitMqServer(ConfigurationManager.AppSettings.Get("EventProcessorAddress"));
            mqServer.AutoReconnect = true;
            mqServer.DisablePriorityQueues = true;

            using (IEventStream stream = store.OpenStream(sourceId, 0))
            {
                EventMessage @event = stream.CommittedEvents.First();
                using (IMessageProducer mqClient = mqServer.CreateMessageProducer())
                {
                    //mqClient.Publish(@event.Body);
                }
            }
        }

        #region Nested type: Dispatcher

        public class Dispatcher : IDispatchCommits
        {
            #region IDispatchCommits Members

            /// <summary>
            ///     Dispatches the commit specified to the messaging infrastructure.
            /// </summary>
            /// <param name="commit">The commmit to be dispatched.</param>
            public void Dispatch(ICommit commit)
            {
            }

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
            }

            #endregion IDispatchCommits Members
        }

        #endregion Nested type: Dispatcher
    }
}