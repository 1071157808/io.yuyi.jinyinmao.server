// FileInformation: nyanya/Domian/CommandLogStore.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/07/31   2:22 AM

using System;
using System.Configuration;
using System.Data.SqlClient;
using Domian.Commands;
using Domian.Config;
using Dapper;
using Infrastructure.EL.TransientFaultHandling;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Logs;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using ServiceStack;

namespace Domian.Logs
{
    public class CommandLogStore : ICommandLogStore
    {
        private readonly CqrsConfiguration config;
        private readonly Lazy<string> connectionString;

        private readonly RetryPolicy retryPolicy;

        public CommandLogStore(CqrsConfiguration config)
        {
            this.retryPolicy = new RetryPolicy<SqlTransientErrorDetectionStrategy>(RetryStrategyFactory.GetSqlDbContextRetryPolicy());
            this.connectionString = new Lazy<string>(this.InitConnectionString);
            this.config = config;
        }

        private string ConnectionString
        {
            get { return this.connectionString.Value; }
        }

        private ILogger Logger
        {
            get { return this.config.CommandStoreLogger; }
        }

        #region ICommandLogStore Members

        public void Create<T>(T command) where T : ICommand
        {
            CommandLog log = new CommandLog
            {
                CommandId = command.CommandId,
                Handled = null,
                HandledTime = null,
                Name = command.GetType().FullName,
                Payload = command.ToJson(),
                ReceiveTime = DateTime.Now,
                Source = command.Source,
                Message = ""
            };

            this.Create(log);
        }

        public void Handled(Guid commandId, bool successed = true, string message = "")
        {
            string updateStatement = @"Update CommandLogs Set Handled = @Handled, HandledTime = @HandledTime, Message = @Message Where CommandId = @CommandId ";

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();
                    // ReSharper disable once AccessToDisposedClosure
                    this.retryPolicy.ExecuteAction(() => connection.Execute(updateStatement, new { CommandId = commandId, Handled = successed, HandledTime = DateTime.Now, Message = message }));
                }
                catch (Exception e)
                {
                    this.Logger.Error(e, "Exceptions During Updating Command Log {0} Handled To {1}".FormatWith(commandId, successed));
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        #endregion ICommandLogStore Members

        private void Create(CommandLog log)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();
                    // ReSharper disable once AccessToDisposedClosure
                    this.retryPolicy.ExecuteAction(() => connection.Execute(@"insert CommandLogs(CommandId,Name,Source,Payload,ReceiveTime,Handled,HandledTime,Message)
                                        values (@CommandId,@Name,@Source,@Payload,@ReceiveTime,@Handled,@HandledTime,@Message)", log));
                }
                catch (Exception e)
                {
                    this.Logger.Error(e, "Exceptions During Creating Command Log.\n{0}\n{1}\n{2}".FormatWith(log.Name, log.Source, log.Payload));
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private string InitConnectionString()
        {
            try
            {
                return ConfigurationManager.ConnectionStrings["CommandLogStore"].ToString();
            }
            catch (Exception e)
            {
                throw new CqrsConfigException("Can not find CommandLogStore connection string.", e);
            }
        }
    }
}