// FileInformation: nyanya/Infrastructure.Lib.CQRS/AppInitEventArgs.cs
// CreatedTime: 2014/06/04   1:03 PM
// LastUpdatedTime: 2014/06/04   5:04 PM

using System;
using Infrastructure.Lib.CQRS.Config;

namespace Infrastructure.Lib.CQRS.Application
{
    /// <summary>
    ///     Represents the class that contains the event data
    ///     for application initialization.
    /// </summary>
    public class AppInitEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        ///     Gets the <see cref="Infrastructure.Lib.CQRS.Config.IConfigSource" /> instance that was used
        ///     for configuring the application.
        /// </summary>
        public IConfigSource ConfigSource { get; private set; }

        /// <summary>
        ///     Gets the <see cref="Infrastructure.Lib.CQRS.IObjectContainer" /> instance with which the application
        ///     registers or resolves the object dependencies.
        /// </summary>
        public ObjectContainer ObjectContainer { get; private set; }

        #endregion Public Properties

        #region Ctor

        /// <summary>
        ///     Initializes a new instance of <c>AppInitEventArgs</c> class.
        /// </summary>
        public AppInitEventArgs()
            : this(null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of <c>AppInitEventArgs</c> class.
        /// </summary>
        /// <param name="configSource">
        ///     The <see cref="Infrastructure.Lib.CQRS.Config.IConfigSource" /> instance that was used
        ///     for configuring the application.
        /// </param>
        /// <param name="objectContainer">
        ///     The <see cref="Infrastructure.Lib.CQRS.IObjectContainer" /> instance with which the application
        ///     registers or resolves the object dependencies.
        /// </param>
        public AppInitEventArgs(IConfigSource configSource, ObjectContainer objectContainer)
        {
            this.ConfigSource = configSource;
            this.ObjectContainer = objectContainer;
        }

        #endregion Ctor
    }
}