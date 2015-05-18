// FileInformation: nyanya/Domian/DefaultServicesContainer.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/07/20   3:11 PM

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using Domian.Bus;
using Domian.Bus.Implementation;
using Domian.Commands;
using Domian.Events;
using Domian.Logs;
using Infrastructure.Lib;
using Infrastructure.Lib.Dependencies;
using Infrastructure.Lib.Utility;
using Infrastructure.SMS;

namespace Domian.Config
{
    public class DefaultServicesContainer : ServicesContainer
    {
        private readonly CqrsConfiguration configuration;
        private readonly Dictionary<Type, List<object>> defaultServicesMulti = new Dictionary<Type, List<object>>();

        // Mutation operations delegate (throw if applied to wrong set)
        private readonly Dictionary<Type, object> defaultServicesSingle = new Dictionary<Type, object>();

        private readonly HashSet<Type> serviceTypesMulti;
        private readonly HashSet<Type> serviceTypesSingle;
        private ConcurrentDictionary<Type, object[]> cacheMulti = new ConcurrentDictionary<Type, object[]>();
        private ConcurrentDictionary<Type, object> cacheSingle = new ConcurrentDictionary<Type, object>();
        private IDependencyResolver lastKnownDependencyResolver;

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Class needs references to large number of types.")]
        public DefaultServicesContainer(CqrsConfiguration configuration)
        {
            Argument.NotNull(configuration);
            this.configuration = configuration;
            // Initialize the dictionary with all known service types, even if the list for that service type is
            // empty, because we will throw if the developer tries to read or write unsupported types.

            // 使用默认日志，即全部是空日志
            this.SetSingle(new LogsConfig());
            this.SetSingle<ISmsAlertsService>(new SmsService());

            this.SetSingle<ICommandBus>(new CommandBus(configuration));
            this.SetSingle<ICommandLogStore>(new CommandLogStore(configuration));
            this.SetSingle<ICommandHandlers>(new CommandHandlers());
            this.SetSingle<IEventHandlers>(new EventHandlers());
            this.SetSingle<IEventBus>(new EventBus(configuration));

            this.serviceTypesSingle = new HashSet<Type>(this.defaultServicesSingle.Keys);
            this.serviceTypesMulti = new HashSet<Type>(this.defaultServicesMulti.Keys);

            // Reset the caches and the known dependency scope
            this.ResetCache();
        }

        /// <summary>
        ///     This constructor is for unit testing purposes only.
        /// </summary>
        protected DefaultServicesContainer()
        {
        }

        /// <summary>
        ///     Try to get a service of the given type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>The first instance of the service, or null if the service is not found.</returns>
        public override object GetService(Type serviceType)
        {
            // Cached read case is very performance-sensitive
            Argument.NotNull(serviceType);

            // Invalidate the cache if the dependency scope has switched
            if (this.lastKnownDependencyResolver != this.configuration.DependencyResolver)
            {
                this.ResetCache();
            }

            object result;

            if (this.cacheSingle.TryGetValue(serviceType, out result))
            {
                return result;
            }

            // Check input after initial read attempt for performance.
            if (!this.serviceTypesSingle.Contains(serviceType))
            {
                throw new Exception("DefaultServices_InvalidServiceType");
            }

            // Get the service from DI. If we're coming up hot, this might
            // mean we end up creating the service more than once.
            object dependencyService = this.lastKnownDependencyResolver.GetService(serviceType);

            if (!this.cacheSingle.TryGetValue(serviceType, out result))
            {
                result = dependencyService ?? this.defaultServicesSingle[serviceType];
                this.cacheSingle.TryAdd(serviceType, result);
            }

            return result;
        }

        /// <summary>
        ///     Try to get a list of services of the given type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>
        ///     The list of service instances of the given type. Returns an empty enumeration if the
        ///     service is not found.
        /// </returns>
        public override IEnumerable<object> GetServices(Type serviceType)
        {
            // Cached read case is very performance-sensitive
            Argument.NotNull(serviceType);

            // Invalidate the cache if the dependency scope has switched
            if (this.lastKnownDependencyResolver != this.configuration.DependencyResolver)
            {
                this.ResetCache();
            }

            object[] result;

            if (this.cacheMulti.TryGetValue(serviceType, out result))
            {
                return result;
            }

            // Check input after initial read attempt for performance.
            if (!this.serviceTypesMulti.Contains(serviceType))
            {
                throw new Exception("DefaultServices_InvalidServiceType");
            }

            // Get the service from DI. If we're coming up hot, this might
            // mean we end up creating the service more than once.
            IEnumerable<object> dependencyServices = this.lastKnownDependencyResolver.GetServices(serviceType);

            if (!this.cacheMulti.TryGetValue(serviceType, out result))
            {
                result = dependencyServices.Where(s => s != null)
                    .Concat(this.defaultServicesMulti[serviceType])
                    .ToArray();
                this.cacheMulti.TryAdd(serviceType, result);
            }

            return result;
        }

        public override bool IsSingleService(Type serviceType)
        {
            Argument.NotNull(serviceType);
            return this.serviceTypesSingle.Contains(serviceType);
        }

        protected override void ClearSingle(Type serviceType)
        {
            this.defaultServicesSingle[serviceType] = null;
        }

        // Returns the List<object> for the given service type. Also validates serviceType is in the known service type list.
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "inherits from base")]
        protected override List<object> GetServiceInstances(Type serviceType)
        {
            Contract.Assert(serviceType != null);

            List<object> result;
            if (!this.defaultServicesMulti.TryGetValue(serviceType, out result))
            {
                throw new Exception("DefaultServices_InvalidServiceType");
            }

            return result;
        }

        protected override void ReplaceSingle(Type serviceType, object service)
        {
            Argument.NotNull(serviceType);
            this.defaultServicesSingle[serviceType] = service;
        }

        // Removes the cached values for a single service type. Called whenever the user manipulates
        // the local service list for a given service type.
        protected override void ResetCache(Type serviceType)
        {
            object single;
            this.cacheSingle.TryRemove(serviceType, out single);
            object[] multiple;
            this.cacheMulti.TryRemove(serviceType, out multiple);
        }

        // Removes the cached values for all service types. Called when the dependency scope
        // has changed since the last time we made a request.
        private void ResetCache()
        {
            this.cacheSingle = new ConcurrentDictionary<Type, object>();
            this.cacheMulti = new ConcurrentDictionary<Type, object[]>();
            this.lastKnownDependencyResolver = this.configuration.DependencyResolver;
        }

        private void SetMultiple<T>(params T[] instances) where T : class
        {
            IEnumerable<object> x = instances;
            this.defaultServicesMulti[typeof(T)] = new List<object>(x);
        }

        private void SetSingle<T>(T instance) where T : class
        {
            this.defaultServicesSingle[typeof(T)] = instance;
        }
    }
}