// FileInformation: nyanya/Infrastructure.Lib/DependencyResolver.cs
// CreatedTime: 2014/07/03   4:40 PM
// LastUpdatedTime: 2014/07/04   9:43 AM

using System;
using System.Collections.Generic;
using Ninject;
using Ninject.Parameters;
using Ninject.Planning.Bindings;

namespace Infrastructure.Lib.Dependencies
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public DependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        #region IDependencyResolver Members

        /// <summary>
        ///     Evaluates if an instance of the specified service can be resolved.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        public bool CanResolve<T>(params IParameter[] parameters)
        {
            return this.kernel.CanResolve<T>(parameters);
        }

        /// <summary>
        ///     Evaluates if  an instance of the specified service by using the first binding with the specified name can be resolved.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        public bool CanResolve<T>(string name, params IParameter[] parameters)
        {
            return this.kernel.CanResolve<T>(name, parameters);
        }

        /// <summary>
        ///     Evaluates if  an instance of the specified service by using the first binding that matches the specified constraint can be resolved.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        public bool CanResolve<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return this.kernel.CanResolve<T>(constraint, parameters);
        }

        /// <summary>
        ///     Gets an instance of the specified service.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        public object CanResolve(Type service, params IParameter[] parameters)
        {
            return this.kernel.CanResolve(service, parameters);
        }

        /// <summary>
        ///     Gets an instance of the specified service by using the first binding with the specified name.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        public object CanResolve(Type service, string name, params IParameter[] parameters)
        {
            return this.kernel.CanResolve(service, name, parameters);
        }

        /// <summary>
        ///     Gets an instance of the specified service.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        public T Get<T>(params IParameter[] parameters)
        {
            return this.kernel.Get<T>(parameters);
        }

        /// <summary>
        ///     Gets an instance of the specified service by using the first binding with the specified name.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        public T Get<T>(string name, params IParameter[] parameters)
        {
            return this.kernel.Get<T>(name, parameters);
        }

        /// <summary>
        ///     Gets an instance of the specified service by using the first binding that matches the specified constraint.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        public T Get<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return this.kernel.Get<T>(constraint, parameters);
        }

        /// <summary>
        ///     Gets an instance of the specified service.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        public object Get(Type service, params IParameter[] parameters)
        {
            return this.kernel.Get(service, parameters);
        }

        /// <summary>
        ///     Gets an instance of the specified service by using the first binding with the specified name.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        public object Get(Type service, string name, params IParameter[] parameters)
        {
            return this.kernel.Get(service, name, parameters);
        }

        /// <summary>
        ///     Gets an instance of the specified service by using the first binding that matches the specified constraint.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        public object Get(Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return this.kernel.Get(service, constraint, parameters);
        }

        /// <summary>
        ///     Gets all available instances of the specified service.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        public IEnumerable<T> GetAll<T>(params IParameter[] parameters)
        {
            return this.kernel.GetAll<T>(parameters);
        }

        /// <summary>
        ///     Gets all instances of the specified service using bindings registered with the specified name.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        public IEnumerable<T> GetAll<T>(string name, params IParameter[] parameters)
        {
            return this.kernel.GetAll<T>(name, parameters);
        }

        /// <summary>
        ///     Gets all instances of the specified service by using the bindings that match the specified constraint.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="constraint">The constraint to apply to the bindings.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        public IEnumerable<T> GetAll<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return this.kernel.GetAll<T>(constraint, parameters);
        }

        /// <summary>
        ///     Gets all available instances of the specified service.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        public IEnumerable<object> GetAll(Type service, params IParameter[] parameters)
        {
            return this.kernel.GetAll(service, parameters);
        }

        /// <summary>
        ///     Gets all instances of the specified service using bindings registered with the specified name.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        public IEnumerable<object> GetAll(Type service, string name, params IParameter[] parameters)
        {
            return this.kernel.GetAll(service, name, parameters);
        }

        /// <summary>
        ///     Gets all instances of the specified service by using the bindings that match the specified constraint.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="constraint">The constraint to apply to the bindings.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        public IEnumerable<object> GetAll(Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return this.kernel.GetAll(service, constraint, parameters);
        }

        /// <summary>
        ///     Gets the service object of the specified type.
        /// </summary>
        /// <returns>
        ///     A service object of type <paramref name="serviceType" />.-or- null if there is no service object of type <paramref name="serviceType" />.
        /// </returns>
        /// <param name="serviceType">An object that specifies the type of service object to get. </param>
        /// <filterpriority>2</filterpriority>
        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        /// <summary>
        ///     Tries to get an instance of the specified service.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        public T GetService<T>()
        {
            return this.kernel.Get<T>();
        }

        /// <summary>
        ///     Gets the services of the specifies type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns>
        ///     All service instances or an empty enumerable if none is configured.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }

        /// <summary>
        ///     Tries to get all instances of the specified service.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <returns>All service instances or an empty enumerable if none is configured.</returns>
        public IEnumerable<T> GetServices<T>()
        {
            return this.kernel.GetAll<T>();
        }

        /// <summary>
        ///     Tries to get an instance of the specified service.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        public T TryGet<T>(params IParameter[] parameters)
        {
            return this.kernel.TryGet<T>(parameters);
        }

        /// <summary>
        ///     Tries to get an instance of the specified service by using the first binding with the specified name.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        public T TryGet<T>(string name, params IParameter[] parameters)
        {
            return this.kernel.TryGet<T>(name, parameters);
        }

        /// <summary>
        ///     Tries to get an instance of the specified service by using the first binding that matches the specified constraint.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        public T TryGet<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return this.kernel.TryGet<T>(constraint, parameters);
        }

        /// <summary>
        ///     Tries to get an instance of the specified service.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        public object TryGet(Type service, params IParameter[] parameters)
        {
            return this.kernel.TryGet(service, parameters);
        }

        /// <summary>
        ///     Tries to get an instance of the specified service by using the first binding with the specified name.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        public object TryGet(Type service, string name, params IParameter[] parameters)
        {
            return this.kernel.TryGet(service, name, parameters);
        }

        /// <summary>
        ///     Tries to get an instance of the specified service by using the first binding that matches the specified constraint.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        public object TryGet(Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return this.kernel.TryGet(service, constraint, parameters);
        }

        #endregion IDependencyResolver Members
    }
}