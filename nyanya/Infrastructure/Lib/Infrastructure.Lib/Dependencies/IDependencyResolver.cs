// FileInformation: nyanya/Infrastructure.Lib/IDependencyResolver.cs
// CreatedTime: 2014/07/02   9:32 AM
// LastUpdatedTime: 2014/07/03   11:22 PM

using System;
using System.Collections.Generic;
using Ninject.Parameters;
using Ninject.Planning.Bindings;

namespace Infrastructure.Lib.Dependencies
{
    /// <summary>
    ///     Represents a dependency injection container.
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        ///     Evaluates if an instance of the specified service can be resolved.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        bool CanResolve<T>(params IParameter[] parameters);

        /// <summary>
        ///     Evaluates if  an instance of the specified service by using the first binding with the specified name can be resolved.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        bool CanResolve<T>(string name, params IParameter[] parameters);

        /// <summary>
        ///     Evaluates if  an instance of the specified service by using the first binding that matches the specified constraint can be resolved.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        bool CanResolve<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters);

        /// <summary>
        ///     Gets an instance of the specified service.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        object CanResolve(Type service, params IParameter[] parameters);

        /// <summary>
        ///     Gets an instance of the specified service by using the first binding with the specified name.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        object CanResolve(Type service, string name, params IParameter[] parameters);

        /// <summary>
        ///     Gets an instance of the specified service.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        T Get<T>(params IParameter[] parameters);

        /// <summary>
        ///     Gets an instance of the specified service by using the first binding with the specified name.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        T Get<T>(string name, params IParameter[] parameters);

        /// <summary>
        ///     Gets an instance of the specified service by using the first binding that matches the specified constraint.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        T Get<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters);

        /// <summary>
        ///     Gets an instance of the specified service.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        object Get(Type service, params IParameter[] parameters);

        /// <summary>
        ///     Gets an instance of the specified service by using the first binding with the specified name.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        object Get(Type service, string name, params IParameter[] parameters);

        /// <summary>
        ///     Gets an instance of the specified service by using the first binding that matches the specified constraint.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service.</returns>
        object Get(Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters);

        /// <summary>
        ///     Gets all available instances of the specified service.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        IEnumerable<T> GetAll<T>(params IParameter[] parameters);

        /// <summary>
        ///     Gets all instances of the specified service using bindings registered with the specified name.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        IEnumerable<T> GetAll<T>(string name, params IParameter[] parameters);

        /// <summary>
        ///     Gets all instances of the specified service by using the bindings that match the specified constraint.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="constraint">The constraint to apply to the bindings.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        IEnumerable<T> GetAll<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters);

        /// <summary>
        ///     Gets all available instances of the specified service.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        IEnumerable<object> GetAll(Type service, params IParameter[] parameters);

        /// <summary>
        ///     Gets all instances of the specified service using bindings registered with the specified name.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        IEnumerable<object> GetAll(Type service, string name, params IParameter[] parameters);

        /// <summary>
        ///     Gets all instances of the specified service by using the bindings that match the specified constraint.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="constraint">The constraint to apply to the bindings.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>A series of instances of the service.</returns>
        IEnumerable<object> GetAll(Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters);

        /// <summary>
        ///     Gets the service object of the specified type.
        /// </summary>
        /// <returns>
        ///     A service object of type <paramref name="serviceType" />.-or- null if there is no service object of type <paramref name="serviceType" />.
        /// </returns>
        /// <param name="serviceType">An object that specifies the type of service object to get. </param>
        /// <filterpriority>2</filterpriority>
        object GetService(Type serviceType);

        /// <summary>
        ///     Tries to get an instance of the specified service.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        T GetService<T>();

        /// <summary>
        ///     Gets the services of the specifies type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns>
        ///     All service instances or an empty enumerable if none is configured.
        /// </returns>
        IEnumerable<object> GetServices(Type serviceType);

        /// <summary>
        ///     Tries to get all instances of the specified service.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <returns>All service instances or an empty enumerable if none is configured.</returns>
        IEnumerable<T> GetServices<T>();

        /// <summary>
        ///     Tries to get an instance of the specified service.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        T TryGet<T>(params IParameter[] parameters);

        /// <summary>
        ///     Tries to get an instance of the specified service by using the first binding with the specified name.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        T TryGet<T>(string name, params IParameter[] parameters);

        /// <summary>
        ///     Tries to get an instance of the specified service by using the first binding that matches the specified constraint.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        T TryGet<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters);

        /// <summary>
        ///     Tries to get an instance of the specified service.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        object TryGet(Type service, params IParameter[] parameters);

        /// <summary>
        ///     Tries to get an instance of the specified service by using the first binding with the specified name.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        object TryGet(Type service, string name, params IParameter[] parameters);

        /// <summary>
        ///     Tries to get an instance of the specified service by using the first binding that matches the specified constraint.
        /// </summary>
        /// <param name="service">The service to resolve.</param>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>An instance of the service, or <see langword="null" /> if no implementation was available.</returns>
        object TryGet(Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters);
    }
}