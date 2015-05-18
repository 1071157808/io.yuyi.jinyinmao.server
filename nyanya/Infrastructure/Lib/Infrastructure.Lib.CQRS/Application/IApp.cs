// FileInformation: nyanya/Infrastructure.Lib.CQRS/IApp.cs
// CreatedTime: 2014/06/04   12:59 PM
// LastUpdatedTime: 2014/06/04   5:04 PM

using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Infrastructure.Lib.CQRS.Config;

namespace Infrastructure.Lib.CQRS.Application
{
    /// <summary>
    ///     Represents that the implemented classes are CQRS applications.
    /// </summary>
    public interface IApp
    {
        /// <summary>
        ///     Gets the <see cref="Infrastructure.Lib.CQRS.Config.IConfigSource" /> instance that was used
        ///     for configuring the application.
        /// </summary>
        IConfigSource ConfigSource { get; }

        /// <summary>
        ///     Gets a list of <see cref="Castle.DynamicProxy.IInterceptor" /> instances that are
        ///     registered on the current application.
        /// </summary>
        IEnumerable<IInterceptor> Interceptors { get; }

        /// <summary>
        ///     Gets the <see cref="Infrastructure.Lib.CQRS.IObjectContainer" /> instance with which the application
        ///     registers or resolves the object dependencies.
        /// </summary>
        ObjectContainer ObjectContainer { get; }

        /// <summary>
        ///     The event that occurs when the application is initializing.
        /// </summary>
        event EventHandler<AppInitEventArgs> Initialize;

        /// <summary>
        ///     Starts the application.
        /// </summary>
        void Start();
    }
}