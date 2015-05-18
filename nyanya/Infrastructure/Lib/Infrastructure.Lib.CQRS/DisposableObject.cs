// FileInformation: nyanya/Infrastructure.Lib.CQRS/DisposableObject.cs
// CreatedTime: 2014/06/05   1:01 AM
// LastUpdatedTime: 2014/06/05   1:02 AM

using System;
using System.Runtime.ConstrainedExecution;

namespace Infrastructure.Lib.CQRS
{
    /// <summary>
    ///     Represents that the derived classes are disposable objects.
    /// </summary>
    public abstract class DisposableObject : CriticalFinalizerObject, IDisposable
    {
        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or
        ///     resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.ExplicitDispose();
        }

        #endregion IDisposable Members

        /// <summary>
        ///     Finalizes the object.
        /// </summary>
        ~DisposableObject()
        {
            this.Dispose(false);
        }

        /// <summary>
        ///     Disposes the object.
        /// </summary>
        /// <param name="disposing">
        ///     A <see cref="System.Boolean" /> value which indicates whether
        ///     the object should be disposed explicitly.
        /// </param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        ///     Provides the facility that disposes the object in an explicit manner,
        ///     preventing the Finalizer from being called after the object has been
        ///     disposed explicitly.
        /// </summary>
        protected void ExplicitDispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}