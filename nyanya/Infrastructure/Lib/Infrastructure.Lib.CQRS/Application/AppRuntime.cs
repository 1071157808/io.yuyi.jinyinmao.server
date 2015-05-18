// FileInformation: nyanya/Infrastructure.Lib.CQRS/AppRuntime.cs
// CreatedTime: 2014/06/04   1:04 PM
// LastUpdatedTime: 2014/06/04   1:05 PM

using System;
using Infrastructure.Lib.CQRS.Config;

namespace Infrastructure.Lib.CQRS.Application
{
    /// <summary>
    ///     Represents the Application Runtime from where the application
    ///     is created, initialized and started.
    /// </summary>
    public sealed class AppRuntime
    {

        private static readonly AppRuntime instance = new AppRuntime();
        private static readonly object lockObj = new object();


        #region Private Fields

        private IApp currentApplication;

        #endregion Private Fields

        #region Ctor

        static AppRuntime()
        {
        }

        private AppRuntime()
        {
        }

        #endregion Ctor

        #region Public Static Properties

        /// <summary>
        ///     Gets the instance of the current <c>ApplicationRuntime</c> class.
        /// </summary>
        public static AppRuntime Instance
        {
            get { return instance; }
        }

        #endregion Public Static Properties

        #region Public Properties

        /// <summary>
        ///     Gets the instance of the currently running application.
        /// </summary>
        public IApp CurrentApplication
        {
            get { return this.currentApplication; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///     Creates and initializes a new application instance.
        /// </summary>
        /// <param name="configSource">
        ///     The <see cref="Infrastructure.Lib.CQRS.Config.IConfigSource" /> instance that
        ///     is used for initializing the application.
        /// </param>
        /// <returns>The initialized application instance.</returns>
        public static IApp Create(IConfigSource configSource)
        {
            lock (lockObj)
            {
                if (instance.currentApplication == null)
                {
                    lock (lockObj)
                    {
                        //if (configSource.Config == null ||
                        //    configSource.Config.Application == null)
                        //    throw new ConfigException("Either apworks configuration or apworks application configuration has not been initialized in the ConfigSource instance.");
                        //string typeName = configSource.Config.Application.Provider;
                        //if (string.IsNullOrEmpty(typeName))
                        //    throw new ConfigException("The provider type has not been defined in the ConfigSource.");
                        //Type type = Type.GetType(typeName);
                        //if (type == null)
                        //    throw new ConfigException("The application provider defined by type '{0}' doesn't exist.", typeName);
                        //instance.currentApplication = (IApp)Activator.CreateInstance(type, new object[] { configSource });
                    }
                }
            }
            return instance.currentApplication;
        }

        #endregion Public Methods
    }
}