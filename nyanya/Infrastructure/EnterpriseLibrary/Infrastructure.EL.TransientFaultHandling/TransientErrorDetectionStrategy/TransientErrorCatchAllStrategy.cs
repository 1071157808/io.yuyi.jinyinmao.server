// FileInformation: nyanya/Infrastructure.EL.TransientFaultHandling/TransientErrorCatchAllStrategy.cs
// CreatedTime: 2014/05/21 4:23 PM
// LastUpdatedTime: 2014/05/21 4:23 PM

using System;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy
{
    /// <summary>
    /// Implements a strategy that treats all exceptions as transient errors.
    /// </summary>
    public sealed class TransientErrorCatchAllStrategy : ITransientErrorDetectionStrategy
    {
        #region ITransientErrorDetectionStrategy Members

        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>Always true.</returns>
        public bool IsTransient(Exception ex)
        {
            return true;
        }

        #endregion ITransientErrorDetectionStrategy Members
    }
}