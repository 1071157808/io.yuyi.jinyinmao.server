// FileInformation: nyanya/Infrastructure.EL.TransientFaultHandling/MySqlErrorDetectionStrategy.cs
// CreatedTime: 2014/06/24   3:02 PM
// LastUpdatedTime: 2014/07/01   11:51 AM

using System;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using MySql.Data.MySqlClient;

namespace Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy
{
    /// <summary>
    ///     Provides the transient error detection logic for transient faults that are specific to MySql.
    /// </summary>
    public sealed class MySqlErrorDetectionStrategy : ITransientErrorDetectionStrategy
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
            if (ex != null)
            {
                if (ex is SqlException || ex is MySqlException)
                    return true;

                if (ex is TimeoutException)
                    return true;

                EntityException entityException;
                if ((entityException = ex as EntityException) != null)
                    return this.IsTransient(entityException.InnerException);
            }
            return false;
        }

        #endregion ITransientErrorDetectionStrategy Members
    }
}