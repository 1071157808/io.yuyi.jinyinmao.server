// FileInformation: nyanya/Infrastructure.Lib/IDisposableObject.cs
// CreatedTime: 2014/07/02   1:40 PM
// LastUpdatedTime: 2014/07/02   1:40 PM

using System;

namespace Infrastructure.Lib.Disposal
{
    /// <summary>
    ///     An object that can report whether or not it is disposed.
    /// </summary>
    public interface IDisposableObject : IDisposable
    {
        /// <summary>
        ///     Gets a value indicating whether this instance is disposed.
        /// </summary>
        bool IsDisposed { get; }
    }
}