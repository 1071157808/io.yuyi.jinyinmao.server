// FileInformation: nyanya/Infrastructure.Lib.CQRS/NLogger.cs
// CreatedTime: 2014/06/06   11:56 AM
// LastUpdatedTime: 2014/06/06   2:46 PM

using System;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using NLog;

namespace Infrastructure.Lib.CQRS.Log.Implementation
{
    public class NLogger : ILogger
    {
        private readonly Logger logger;

        public NLogger(string name)
        {
            Argument.NotNull(name, "name");
            this.logger = LogManager.GetLogger(name);
        }

        #region ILogger Members

        public void Debug(string message)
        {
            Argument.NotNullOrEmpty(message, "message");
            this.logger.Debug(message);
        }

        public void Debug(string message, params object[] args)
        {
            Argument.NotNullOrEmpty(message, "message");
            Argument.NotNull(args, "args");
            this.logger.Debug(message, args);
        }

        public void Error(string message)
        {
            Argument.NotNullOrEmpty(message, "message");
            this.logger.Error(message);
        }

        public void Error(string message, params object[] args)
        {
            Argument.NotNullOrEmpty(message, "message");
            Argument.NotNull(args, "args");
            this.logger.Error(message, args);
        }

        public void Error(Exception exception, string message)
        {
            Argument.NotNull(exception, "exception");
            Argument.NotNullOrEmpty(message, "message");
            this.logger.ErrorException(message, exception);
        }

        public void Error(Exception exception, string message, params object[] args)
        {
            Argument.NotNull(exception, "exception");
            Argument.NotNullOrEmpty(message, "message");
            Argument.NotNull(args, "args");
            this.logger.ErrorException(message.FormatWith(args), exception);
        }

        public void Fatal(string message)
        {
            Argument.NotNullOrEmpty(message, "message");
            this.logger.Fatal(message);
        }

        public void Fatal(string message, params object[] args)
        {
            Argument.NotNullOrEmpty(message, "message");
            Argument.NotNull(args, "args");
            this.logger.Fatal(message, args);
        }

        public void Fatal(Exception exception, string message)
        {
            Argument.NotNull(exception, "exception");
            Argument.NotNullOrEmpty(message, "message");
            this.logger.FatalException(message, exception);
        }

        public void Fatal(Exception exception, string message, params object[] args)
        {
            Argument.NotNull(exception, "exception");
            Argument.NotNullOrEmpty(message, "message");
            Argument.NotNull(args, "args");
            this.logger.FatalException(message.FormatWith(args), exception);
        }

        public void Info(string message)
        {
            Argument.NotNullOrEmpty(message, "message");
            this.logger.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            Argument.NotNullOrEmpty(message, "message");
            Argument.NotNull(args, "args");
            this.logger.Info(message, args);
        }

        public void Trace(string message)
        {
            Argument.NotNullOrEmpty(message, "message");
            this.logger.Trace(message);
        }

        public void Trace(string message, params object[] args)
        {
            Argument.NotNullOrEmpty(message, "message");
            Argument.NotNull(args, "args");
            this.logger.Trace(message, args);
        }

        public void Warning(string message)
        {
            Argument.NotNullOrEmpty(message, "message");
            this.logger.Warn(message);
        }

        public void Warning(string message, params object[] args)
        {
            Argument.NotNullOrEmpty(message, "message");
            Argument.NotNull(args, "args");
            this.logger.Warn(message, args);
        }

        #endregion ILogger Members
    }
}