// FileInformation: nyanya/Infrastructure.EL.TransientFaultHandling/RedisErrorDetectionStrategy.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/09/12   2:09 PM

using System;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy
{
    public class CouchbaseErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        #region ITransientErrorDetectionStrategy Members

        /// <summary>
        ///     Determines whether the specified exception represents a transient failure that can be
        ///     compensated by a retry.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>True if the specified exception is considered as transient, otherwise false.</returns>
        public bool IsTransient(Exception ex)
        {
            return true;
        }

        #endregion ITransientErrorDetectionStrategy Members
    }
}