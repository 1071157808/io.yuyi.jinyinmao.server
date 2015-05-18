// FileInformation: nyanya/Infrastructure.Lib.CQRS/InterceptorSelector.cs
// CreatedTime: 2014/06/04   4:41 PM
// LastUpdatedTime: 2014/06/04   5:04 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Infrastructure.Lib.CQRS.Application;
using Infrastructure.Lib.CQRS.Config;

namespace Infrastructure.Lib.CQRS.Interception
{
    /// <summary>
    ///     Represents the interceptor selector.
    /// </summary>
    public sealed class InterceptorSelector : IInterceptorSelector
    {
        #region Private Methods

        private MethodInfo GetMethodInBase(Type baseType, MethodInfo thisMethod)
        {
            MethodInfo[] methods = baseType.GetMethods();
            IEnumerable<MethodInfo> methodQuery = methods.Where(p =>
            {
                bool retval = p.Name == thisMethod.Name &&
                              p.IsGenericMethod == thisMethod.IsGenericMethod &&
                              ((p.GetParameters() == null && thisMethod.GetParameters() == null) || (p.GetParameters().Length == thisMethod.GetParameters().Length));
                if (!retval)
                    return false;
                ParameterInfo[] thisMethodParameters = thisMethod.GetParameters();
                ParameterInfo[] pMethodParameters = p.GetParameters();
                for (int i = 0; i < thisMethodParameters.Length; i++)
                {
                    retval &= pMethodParameters[i].ParameterType == thisMethodParameters[i].ParameterType;
                }
                return retval;
            });
            if (methodQuery != null && methodQuery.Count() > 0)
                return methodQuery.Single();
            return null;
        }

        #endregion Private Methods

        #region IInterceptorSelector Members

        /// <summary>
        ///     Selects the interceptors for the given type and method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="interceptors">The origin interceptor collection.</param>
        /// <returns>An array of interceptors specific for the given type and method.</returns>
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            IConfigSource configSource = AppRuntime.Instance.CurrentApplication.ConfigSource;
            List<IInterceptor> selectedInterceptors = new List<IInterceptor>();

            IEnumerable<string> interceptorTypes = configSource.Config.GetInterceptorTypes(type, method);
            if (interceptorTypes == null)
            {
                if (type.BaseType != null && type.BaseType != typeof(Object))
                {
                    Type baseType = type.BaseType;
                    MethodInfo methodInfoBase = null;
                    while (baseType != null && type.BaseType != typeof(Object))
                    {
                        methodInfoBase = this.GetMethodInBase(baseType, method);
                        if (methodInfoBase != null)
                            break;
                        baseType = baseType.BaseType;
                    }
                    if (baseType != null && methodInfoBase != null)
                    {
                        interceptorTypes = configSource.Config.GetInterceptorTypes(baseType, methodInfoBase);
                    }
                }
                if (interceptorTypes == null)
                {
                    Type[] intfTypes = type.GetInterfaces();
                    if (intfTypes != null && intfTypes.Count() > 0)
                    {
                        foreach (Type intfType in intfTypes)
                        {
                            MethodInfo methodInfoBase = this.GetMethodInBase(intfType, method);
                            if (methodInfoBase != null)
                                interceptorTypes = configSource.Config.GetInterceptorTypes(intfType, methodInfoBase);
                            if (interceptorTypes != null)
                                break;
                        }
                    }
                }
            }

            if (interceptorTypes != null && interceptorTypes.Count() > 0)
            {
                foreach (IInterceptor interceptor in interceptors)
                {
                    if (interceptorTypes.Any(p => interceptor.GetType().AssemblyQualifiedName.Equals(p)))
                        selectedInterceptors.Add(interceptor);
                }
            }

            return selectedInterceptors.ToArray();
        }

        #endregion IInterceptorSelector Members
    }
}