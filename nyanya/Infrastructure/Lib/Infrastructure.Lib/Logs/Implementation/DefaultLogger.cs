// FileInformation: nyanya/Infrastructure.Lib/DefaultLogger.cs
// CreatedTime: 2014/07/06   9:28 PM
// LastUpdatedTime: 2014/07/06   9:28 PM

using System;

namespace Infrastructure.Lib.Logs.Implementation
{
    public class DefaultLogger : ILogger
    {
        #region ILogger Members

        public void Debug(string message)
        {
        }

        public void Debug(string message, params object[] args)
        {
        }

        public void Error(string message)
        {
        }

        public void Error(string message, params object[] args)
        {
        }

        public void Error(Exception exception, string message)
        {
        }

        public void Error(Exception exception, string message, params object[] args)
        {
        }

        public void Fatal(string message)
        {
        }

        public void Fatal(string message, params object[] args)
        {
        }

        public void Fatal(Exception exception, string message)
        {
        }

        public void Fatal(Exception exception, string message, params object[] args)
        {
        }

        public void Info(string message)
        {
        }

        public void Info(string message, params object[] args)
        {
        }

        public void Trace(string message)
        {
        }

        public void Trace(string message, params object[] args)
        {
        }

        public void Warning(string message)
        {
        }

        public void Warning(string message, params object[] args)
        {
        }

        #endregion ILogger Members
    }
}