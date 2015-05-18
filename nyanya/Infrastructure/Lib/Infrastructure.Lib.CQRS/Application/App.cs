// FileInformation: nyanya/Infrastructure.Lib.CQRS/App.cs
// CreatedTime: 2014/06/04   1:02 PM
// LastUpdatedTime: 2014/06/06   4:05 PM

using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Infrastructure.Lib.CQRS.Config;

namespace Infrastructure.Lib.CQRS.Application
{
    public class App : IApp
    {
        #region Private Fields

        private readonly IConfigSource configSource;
        private readonly List<IInterceptor> interceptors;
        private readonly ObjectContainer objectContainer;

        #endregion Private Fields

        #region Ctor

        /// <summary>
        ///     Initializes a new instance of <c>Application</c> class.
        /// </summary>
        /// <param name="configSource">
        ///     The <see cref="Infrastructure.Lib.CQRS.Config.IConfigSource" /> instance that was used
        ///     for configuring the application.
        /// </param>
        public App(IConfigSource configSource)
        {
            //if (configSource == null)
            //    throw new ArgumentNullException("configSource");
            //if (configSource.Config == null)
            //    throw new ConfigException("ApworksConfigSection has not been defined in the ConfigSource instance.");
            //if (configSource.Config.ObjectContainer == null)
            //    throw new ConfigException("No ObjectContainer instance has been specified in the ApworksConfigSection.");
            //this.configSource = configSource;
            //string objectContainerProviderName = configSource.Config.ObjectContainer.Provider;
            //if (string.IsNullOrEmpty(objectContainerProviderName) ||
            //    string.IsNullOrWhiteSpace(objectContainerProviderName))
            //    throw new ConfigException("The ObjectContainer provider has not been defined in the ConfigSource.");
            //Type objectContainerType = Type.GetType(objectContainerProviderName);
            //if (objectContainerType == null)
            //    throw new InfrastructureException("The ObjectContainer defined by type {0} doesn't exist.", objectContainerProviderName);
            //this.objectContainer = (ObjectContainer)Activator.CreateInstance(objectContainerType);
            //if (this.configSource.Config.ObjectContainer.InitFromConfigFile)
            //{
            //    string sectionName = this.configSource.Config.ObjectContainer.SectionName;
            //    if (!string.IsNullOrEmpty(sectionName) && !string.IsNullOrWhiteSpace(sectionName))
            //    {
            //        this.objectContainer.InitializeFromConfigFile(sectionName);
            //    }
            //    else
            //        throw new ConfigException("Section name for the ObjectContainer configuration should also be provided when InitFromConfigFile has been set to true.");
            //}
            //this.interceptors = new List<IInterceptor>();
            //if (this.configSource.Config.Interception != null &&
            //    this.configSource.Config.Interception.Interceptors != null)
            //{
            //    foreach (InterceptorElement interceptorElement in this.configSource.Config.Interception.Interceptors)
            //    {
            //        Type interceptorType = Type.GetType(interceptorElement.Type);
            //        if (interceptorType == null)
            //            continue;
            //        IInterceptor interceptor = (IInterceptor)Activator.CreateInstance(interceptorType);
            //        this.interceptors.Add(interceptor);
            //    }
            //}
        }

        #endregion Ctor

        #region Private Methods

        private void DoInitialize()
        {
            EventHandler<AppInitEventArgs> d = this.Initialize;
            if (d != null)
                d(this, new AppInitEventArgs(this.configSource, this.objectContainer));
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        ///     Starts the application
        /// </summary>
        protected virtual void OnStart()
        {
        }

        #endregion Protected Methods

        #region IApp Members

        /// <summary>
        ///     The event that occurs when the application is initializing.
        /// </summary>
        public event EventHandler<AppInitEventArgs> Initialize;

        /// <summary>
        ///     Gets the <see cref="Apworks.Config.IConfigSource" /> instance that was used
        ///     for configuring the application.
        /// </summary>
        public IConfigSource ConfigSource
        {
            get { return this.configSource; }
        }

        /// <summary>
        ///     Gets a list of <see cref="Castle.DynamicProxy.IInterceptor" /> instances that are
        ///     registered on the current application.
        /// </summary>
        public IEnumerable<IInterceptor> Interceptors
        {
            get { return this.interceptors; }
        }

        /// <summary>
        ///     Gets the <see cref="Apworks.IObjectContainer" /> instance with which the application
        ///     registers or resolves the object dependencies.
        /// </summary>
        public ObjectContainer ObjectContainer
        {
            get { return this.objectContainer; }
        }

        /// <summary>
        ///     Starts the application.
        /// </summary>
        public void Start()
        {
            this.DoInitialize();
            this.OnStart();
        }

        #endregion IApp Members
    }
}