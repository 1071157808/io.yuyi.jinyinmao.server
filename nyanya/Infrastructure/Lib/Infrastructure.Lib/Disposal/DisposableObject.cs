// FileInformation: nyanya/Infrastructure.Lib/DisposableObject.cs
// CreatedTime: 2014/07/02   1:41 PM
// LastUpdatedTime: 2014/07/02   1:43 PM

using System;
using System.Runtime.ConstrainedExecution;

namespace Infrastructure.Lib.Disposal
{
    /// <summary>
    ///     Represents that the derived classes are disposable objects.
    /// </summary>
    public abstract class DisposableObject : CriticalFinalizerObject, IDisposableObject, IDisposable
    {
        #region IDisposableObject Members

        /// <summary>
        ///     Gets a value indicating whether this instance is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or
        ///     resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            this.ExplicitDispose();
        }

        #endregion IDisposableObject Members

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
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!disposing || this.IsDisposed)
                    return;
                this.IsDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

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