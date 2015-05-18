// FileInformation: nyanya/Infrastructure.EL.TransientFaultHandling/CouchbaseErrorDetectionStrategy.cs
// CreatedTime: 2014/09/12   12:17 PM
// LastUpdatedTime: 2014/09/12   1:31 PM

using System;
using System.Collections.Generic;
using Infrastructure.Lib.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy
{
    public class RedisErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        private readonly List<int> temporaryFailureErrorCodes = new List<int> { 0x85, 0x86, 0x91, 0x94 };

        #region ITransientErrorDetectionStrategy Members

        /// <summary>
        ///     Determines whether the specified exception represents a transient failure that can be
        ///     compensated by a retry.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>True if the specified exception is considered as transient, otherwise false.</returns>
        public bool IsTransient(Exception ex)
        {
            if (ex != null)
            {
                CouchbaseOperationFailedException exception;
                if ((exception = ex as CouchbaseOperationFailedException) != null)
                {
                    return this.temporaryFailureErrorCodes.Contains(exception.ErrorCode);
                }
            }
            return false;
        }

        #endregion ITransientErrorDetectionStrategy Members
    }
}