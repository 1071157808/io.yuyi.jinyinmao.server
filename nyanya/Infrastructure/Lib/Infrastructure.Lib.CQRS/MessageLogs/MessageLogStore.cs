// FileInformation: nyanya/Infrastructure.Lib.CQRS/MessageLogStore.cs
// CreatedTime: 2014/06/23   2:03 PM
// LastUpdatedTime: 2014/06/24   2:29 PM

using System;
using System.Threading.Tasks;
using Dapper;
using Infrastructure.EL.TransientFaultHandling;
using Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy;
using Infrastructure.Lib.CQRS.Bus;
using Infrastructure.Lib.CQRS.Log;
using Infrastructure.Lib.CQRS.Log.Implementation;
using Infrastructure.Lib.Extensions;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Infrastructure.Lib.CQRS.MessageLogs
{
    public abstract class MessageLogStore : IMessageLogStore
    {
        private static readonly ILogger logger;
        private static readonly RetryPolicy retryPolicy;
        private readonly string logName;
        private readonly string mySqlConnectionString;
        private MySqlConnection connection;

        static MessageLogStore()
        {
            retryPolicy = new RetryPolicy<MySqlErrorDetectionStrategy>(RetryStrategyFactory.GetMySqlDbContextRetryPolicy());
            logger = new NLogger("MessageStore");
        }

        protected MessageLogStore(string logName, string mySqlConnectionString)
        {
            this.logName = logName;
            this.mySqlConnectionString = mySqlConnectionString;
        }

        #region IMessageLogStore Members

        public async Task Create<T>(T message) where T : IMessage
        {
            string payload = JsonConvert.SerializeObject(message);
            IMessageLog messageLog = new MessageLog
            {
                BuildTime = DateTime.Now,
                Handled = null,
                HandledTime = null,
                Delivered = null,
                DeliveredTime = null,
                Guid = message.Guid,
                Payload = payload,
                Name = message.GetType().AssemblyQualifiedName,
                Source = message.Source
            };

            using (this.connection = new MySqlConnection(this.mySqlConnectionString))
            {
                try
                {
                    await this.connection.OpenAsync();
                    await retryPolicy.ExecuteAction(() => this.connection.ExecuteAsync(@"insert {0}(Guid,Name,Source,Payload,BuildTime,Delivered,DeliveredTime,Handled,HandledTime)
                                        values (@Guid,@Name,@Source,@Payload,@BuildTime,@Delivered,@DeliveredTime,@Handled,@HandledTime)".FormatWith(this.logName), messageLog));
                }
                catch (Exception e)
                {
                    logger.Fatal(e, "Exceptions During Create Message Log.\n" + payload);
                }
                finally
                {
                    if (this.connection != null)
                    {
                        this.connection.Close();
                    }
                }
            }
        }

        public async Task Delivered<T>(T message, bool successed = true) where T : IMessage
        {
            string updateStatement = @"Update {0} Set Delivered = @Delivered, DeliveredTime = @DeliveredTime Where Guid = @Guid ".FormatWith(this.logName);

            using (this.connection = new MySqlConnection(this.mySqlConnectionString))
            {
                try
                {
                    await this.connection.OpenAsync();
                    await retryPolicy.ExecuteAction(() => this.connection.ExecuteAsync(updateStatement, new { Delivered = successed, DeliveredTime = DateTime.Now, message.Guid }));
                }
                catch (Exception e)
                {
                    logger.Fatal(e, "Exceptions During Create Message Log.\n" + JsonConvert.SerializeObject(message));
                }
                finally
                {
                    if (this.connection != null)
                    {
                        this.connection.Close();
                    }
                }
            }
        }

        public async Task Handled<T>(T message, bool successed = true) where T : IMessage
        {
            string updateStatement = @"Update {0} Set Handled = @Handled, HandledTime = @HandledTime Where Guid = @Guid ".FormatWith(this.logName);

            using (this.connection = new MySqlConnection(this.mySqlConnectionString))
            {
                try
                {
                    await this.connection.OpenAsync();
                    await retryPolicy.ExecuteAction(() => this.connection.ExecuteAsync(updateStatement, new { Handled = successed, HandledTime = DateTime.Now, message.Guid }));
                }
                catch (Exception e)
                {
                    logger.Fatal(e, "Exceptions During Create Message Log.\n" + JsonConvert.SerializeObject(message));
                }
                finally
                {
                    if (this.connection != null)
                    {
                        this.connection.Close();
                    }
                }
            }
        }

        #endregion IMessageLogStore Members
    }
}