// FileInformation: nyanya/Infrastructure.Lib.CQRS/CommandLogStore.cs
// CreatedTime: 2014/07/01   1:28 PM
// LastUpdatedTime: 2014/07/01   3:22 PM

using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Infrastructure.EL.TransientFaultHandling;
using Infrastructure.Lib.CQRS.Log;
using Infrastructure.Lib.CQRS.Log.Implementation;
using Infrastructure.Lib.Extensions;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Infrastructure.Lib.CQRS.MessageLogs
{
    public class CommandLogStore : ICommandLogStore
    {
        private static readonly string connectionString;
        private static readonly ILogger logger;
        private static readonly RetryPolicy retryPolicy;

        static CommandLogStore()
        {
            retryPolicy = new RetryPolicy<SqlTransientErrorDetectionStrategy>(RetryStrategyFactory.GetSqlDbContextRetryPolicy());
            //connectionString = ConfigurationManager.ConnectionStrings["CommandLogStore"].ToString();
            logger = new NLogger("CommandLogStore");
        }

        public CommandLogStore()
        {
        }

        public CommandLogStore(string connectionString)
        {
            connectionString = connectionString;
        }

        public CommandLogStore(string connectionString, ILogger logger)
        {
            connectionString = connectionString;
            logger = logger;
        }

        #region ICommandLogStore Members

        public async Task Create(CommandLog log)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    // ReSharper disable once AccessToDisposedClosure
                    await retryPolicy.ExecuteAction(() => connection.ExecuteAsync(@"insert CommandLogs(CommandId,Name,Source,Payload,Handled,HandledTime,Exception)
                                        values (@Guid,@Name,@Source,@Payload,@Handled,@HandledTime)", log));
                }
                catch (Exception e)
                {
                    logger.Fatal(e, "Exceptions During Creating Command Log.\n{0}\n{1}\n{2}".FormatWith(log.Name, log.Source, log.Payload));
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task Handled(Guid commandId, bool successed = true, Exception exception = null)
        {
            string updateStatement = @"Update CommandLogs Set Handled = @Handled, HandledTime = @HandledTime, Exception = @Exception Where CommandId = @Guid ";
            string exceptionMessage = exception == null ? "" : "{0}\n{1}".FormatWith(exception.Message, exception.StackTrace);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    // ReSharper disable once AccessToDisposedClosure
                    await retryPolicy.ExecuteAction(() => connection.ExecuteAsync(updateStatement, new { Handled = successed, HandledTime = DateTime.Now, commandId, Exception = exceptionMessage }));
                }
                catch (Exception e)
                {
                    logger.Fatal(e, "Exceptions During Updating Command Log Handled To {0}".FormatWith(successed));
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        #endregion ICommandLogStore Members
    }
}