// FileInformation: nyanya/Infrastructure.Lib/ServicesContainer.cs
// CreatedTime: 2014/06/30   2:16 PM
// LastUpdatedTime: 2014/07/01   1:59 PM

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;

namespace Infrastructure.Lib
{
    // This is global config.
    public abstract class ServicesContainer : IDisposable
    {
        #region IDisposable Members

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Although this class is not sealed, end users cannot set instances of it so in practice it is sealed.")]
        [SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "Although this class is not sealed, end users cannot set instances of it so in practice it is sealed.")]
        public virtual void Dispose()
        {
        }

        #endregion IDisposable Members

        /// <summary>
        ///     Adds a service to the end of services list for the given service type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="service">The service instance.</param>
        public void Add(Type serviceType, object service)
        {
            this.Insert(serviceType, Int32.MaxValue, service);
        }

        /// <summary>
        ///     Adds the services of the specified collection to the end of the services list for
        ///     the given service type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="services">The services to add.</param>
        public void AddRange(Type serviceType, IEnumerable<object> services)
        {
            this.InsertRange(serviceType, Int32.MaxValue, services);
        }

        /// <summary>
        ///     Removes all the service instances of the given service type.
        /// </summary>
        /// <param name="serviceType">The service type to clear from the services list.</param>
        public virtual void Clear(Type serviceType)
        {
            Argument.NotNull(serviceType, "serviceType");

            if (this.IsSingleService(serviceType))
            {
                this.ClearSingle(serviceType);
            }
            else
            {
                this.ClearMultiple(serviceType);
            }
            this.ResetCache(serviceType);
        }

        /// <summary>
        ///     Searches for a service that matches the conditions defined by the specified
        ///     predicate, and returns the zero-based index of the first occurrence.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="match">
        ///     The delegate that defines the conditions of the element
        ///     to search for.
        /// </param>
        /// <returns>The zero-based index of the first occurrence, if found; otherwise, -1.</returns>
        public int FindIndex(Type serviceType, Predicate<object> match)
        {
            Argument.NotNull(serviceType, "serviceType");

            Argument.NotNull(match, "match");

            List<object> instances = this.GetServiceInstances(serviceType);
            return instances.FindIndex(match);
        }

        public abstract object GetService(Type serviceType);

        public abstract IEnumerable<object> GetServices(Type serviceType);

        /// <summary>
        ///     Inserts a service into the collection at the specified index.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="index">
        ///     The zero-based index at which the service should be inserted.
        ///     If <see cref="Int32.MaxValue" /> is passed, ensures the element is added to the end.
        /// </param>
        /// <param name="service">The service to insert.</param>
        public void Insert(Type serviceType, int index, object service)
        {
            Argument.NotNull(serviceType, "serviceType");

            Argument.NotNull(service, "service");

            Argument.IsNot(serviceType.IsInstanceOfType(service), "{0} must drive from {1}.".FormatWith(service.GetType().Name, serviceType.Name));

            List<object> instances = this.GetServiceInstances(serviceType);
            if (index == Int32.MaxValue)
            {
                index = instances.Count;
            }

            instances.Insert(index, service);

            this.ResetCache(serviceType);
        }

        /// <summary>
        ///     Inserts the elements of the collection into the service list at the specified index.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="index">
        ///     The zero-based index at which the new elements should be inserted.
        ///     If <see cref="Int32.MaxValue" /> is passed, ensures the elements are added to the end.
        /// </param>
        /// <param name="services">The collection of services to insert.</param>
        public void InsertRange(Type serviceType, int index, IEnumerable<object> services)
        {
            Argument.NotNull(serviceType, "serviceType");

            Argument.NotNull(services, "services");

            object[] filteredServices = services.Where(svc => svc != null).ToArray();
            object incorrectlyTypedService = filteredServices.FirstOrDefault(svc => !serviceType.IsInstanceOfType(svc));

            // ReSharper disable once PossibleNullReferenceException
            Argument.Is(incorrectlyTypedService.IsNotNull(), "{0} must drive from {1}.".FormatWith(incorrectlyTypedService.GetType().Name, serviceType.Name));

            List<object> instances = this.GetServiceInstances(serviceType);
            if (index == Int32.MaxValue)
            {
                index = instances.Count;
            }

            instances.InsertRange(index, filteredServices);

            this.ResetCache(serviceType);
        }

        /// <summary>
        ///     Determine whether the service type should be fetched with GetService or GetServices.
        /// </summary>
        /// <param name="serviceType">type of service to query</param>
        /// <returns>true iff the service is singular. </returns>
        public abstract bool IsSingleService(Type serviceType);

        /// <summary>
        ///     Removes the first occurrence of the given service from the service list for the given service type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="service">The service instance to remove.</param>
        /// <returns> <c>true</c> if the item is successfully removed; otherwise, <c>false</c>.</returns>
        public bool Remove(Type serviceType, object service)
        {
            Argument.NotNull(serviceType, "serviceType");

            Argument.NotNull(service, "service");

            List<object> instances = this.GetServiceInstances(serviceType);
            bool result = instances.Remove(service);

            this.ResetCache(serviceType);

            return result;
        }

        /// <summary>
        ///     Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="match">The delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements removed from the list.</returns>
        public int RemoveAll(Type serviceType, Predicate<object> match)
        {
            Argument.NotNull(serviceType, "serviceType");

            Argument.NotNull(match, "match");

            List<object> instances = this.GetServiceInstances(serviceType);
            int result = instances.RemoveAll(match);

            this.ResetCache(serviceType);

            return result;
        }

        /// <summary>
        ///     Removes the service at the specified index.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="index">The zero-based index of the service to remove.</param>
        public void RemoveAt(Type serviceType, int index)
        {
            Argument.NotNull(serviceType, "serviceType");

            List<object> instances = this.GetServiceInstances(serviceType);
            instances.RemoveAt(index);

            this.ResetCache(serviceType);
        }

        /// <summary>
        ///     Replaces all existing services for the given service type with the given
        ///     service instance. This works for both singular and plural services.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="service">The service instance.</param>
        /// <inheritdoc />
        public void Replace(Type serviceType, object service)
        {
            // Check this early, so we don't call RemoveAll before Insert would catch the null service.
            Argument.NotNull(serviceType, "serviceType");

            Argument.NotNull(service, "service");

            Argument.IsNot(serviceType.IsInstanceOfType(service), "{0} must drive from {1}.".FormatWith(service.GetType().Name, serviceType.Name));

            if (this.IsSingleService(serviceType))
            {
                this.ReplaceSingle(serviceType, service);
            }
            else
            {
                this.ReplaceMultiple(serviceType, service);
            }
            this.ResetCache(serviceType);
        }

        /// <summary>
        ///     Replaces all existing services for the given service type with the given
        ///     service instances.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="services">The service instances.</param>
        public void ReplaceRange(Type serviceType, IEnumerable<object> services)
        {
            // Check this early, so we don't call RemoveAll before InsertRange would catch the null services.
            Argument.NotNull(serviceType, "serviceType");

            Argument.NotNull(services, "services");

            this.RemoveAll(serviceType, _ => true);
            this.InsertRange(serviceType, 0, services);
        }

        protected virtual void ClearMultiple(Type serviceType)
        {
            List<object> instances = this.GetServiceInstances(serviceType);
            instances.Clear();
        }

        protected abstract void ClearSingle(Type serviceType);

        // critical method for mutation operations (Add,Insert,Clear,Replace, etc)
        // This is used for multi-services.
        // There are other abstract methods to mutate the single services.
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "expose for mutation")]
        protected abstract List<object> GetServiceInstances(Type serviceType);

        protected virtual void ReplaceMultiple(Type serviceType, object service)
        {
            this.RemoveAll(serviceType, _ => true);
            this.Insert(serviceType, 0, service);
        }

        protected abstract void ReplaceSingle(Type serviceType, object service);

        protected virtual void ResetCache(Type serviceType)
        {
        }
    }
}