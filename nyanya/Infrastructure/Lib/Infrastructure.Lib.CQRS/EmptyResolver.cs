// FileInformation: nyanya/Infrastructure.Lib.CQRS/EmptyResolver.cs
// CreatedTime: 2014/07/01   1:32 PM
// LastUpdatedTime: 2014/07/04   9:41 AM

using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Lib.Dependencies;
using Ninject.Parameters;
using Ninject.Planning.Bindings;

namespace Infrastructure.Lib.CQRS
{
    public class EmptyResolver : IDependencyResolver
    {
        private static readonly IDependencyResolver instance = new EmptyResolver();

        private EmptyResolver()
        {
        }

        public static IDependencyResolver Instance
        {
            get { return instance; }
        }

        #region IDependencyResolver Members

        public bool CanResolve<T>(params IParameter[] parameters)
        {
            return false;
        }

        public bool CanResolve<T>(string name, params IParameter[] parameters)
        {
            return false;
        }

        public bool CanResolve<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return false;
        }

        public object CanResolve(Type service, params IParameter[] parameters)
        {
            return false;
        }

        public object CanResolve(Type service, string name, params IParameter[] parameters)
        {
            return false;
        }

        public T Get<T>(params IParameter[] parameters)
        {
            return default(T);
        }

        public T Get<T>(string name, params IParameter[] parameters)
        {
            return default(T);
        }

        public T Get<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return default(T);
        }

        public object Get(Type service, params IParameter[] parameters)
        {
            return null;
        }

        public object Get(Type service, string name, params IParameter[] parameters)
        {
            return null;
        }

        public object Get(Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return null;
        }

        public IEnumerable<T> GetAll<T>(params IParameter[] parameters)
        {
            return Enumerable.Empty<T>();
        }

        public IEnumerable<T> GetAll<T>(string name, params IParameter[] parameters)
        {
            return Enumerable.Empty<T>();
        }

        public IEnumerable<T> GetAll<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return Enumerable.Empty<T>();
        }

        public IEnumerable<object> GetAll(Type service, params IParameter[] parameters)
        {
            return Enumerable.Empty<object>();
        }

        public IEnumerable<object> GetAll(Type service, string name, params IParameter[] parameters)
        {
            return Enumerable.Empty<object>();
        }

        public IEnumerable<object> GetAll(Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return Enumerable.Empty<object>();
        }

        public object GetService(Type serviceType)
        {
            return null;
        }

        public T GetService<T>()
        {
            return default(T);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }

        public IEnumerable<T> GetServices<T>()
        {
            return Enumerable.Empty<T>();
        }

        public T TryGet<T>(params IParameter[] parameters)
        {
            return default(T);
        }

        public T TryGet<T>(string name, params IParameter[] parameters)
        {
            return default(T);
        }

        public T TryGet<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return default(T);
        }

        public object TryGet(Type service, params IParameter[] parameters)
        {
            return null;
        }

        public object TryGet(Type service, string name, params IParameter[] parameters)
        {
            return null;
        }

        public object TryGet(Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return null;
        }

        #endregion IDependencyResolver Members

        public void Dispose()
        {
        }
    }
}