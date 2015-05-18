// FileInformation: nyanya/Infrastructure.Lib.CQRS/ConfigSection.cs
// CreatedTime: 2014/07/01   1:28 PM
// LastUpdatedTime: 2014/07/01   2:02 PM

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dependencies;

namespace Infrastructure.Lib.CQRS.Config
{
    public class ConfigSection
    {
        //全局DependencyResolver
        public IDependencyResolver DependencyResolver { get; set; }

        public ServicesContainer ServicesContainer { get; set; }

        /// <summary>
        ///     Returns a list of <see cref="System.String" /> values which represents the types
        ///     of interceptors references by the given method.
        /// </summary>
        /// <param name="contractType">The type for the method.</param>
        /// <param name="method">The method.</param>
        /// <returns>A list of <see cref="System.String" /> values which contains the interceptor types.</returns>
        public IEnumerable<string> GetInterceptorTypes(Type contractType, MethodInfo method)
        {
            //if (this.Interception == null ||
            //    this.Interception.Interceptors == null ||
            //    this.Interception.Contracts == null)
            //    return null;

            //InterceptContractElement interceptContractElement = this.FindInterceptContractElement(contractType);
            //if (interceptContractElement == null)
            //    return null;

            //InterceptMethodElement interceptMethodElement = this.FindInterceptMethodElement(interceptContractElement, method);
            //if (interceptMethodElement == null)
            //    return null;

            //var interceptorRefNames = this.FindInterceptorRefNames(interceptMethodElement);
            //if (interceptorRefNames == null || interceptorRefNames.Count() == 0)
            //    return null;

            //List<string> ret = new List<string>();
            //foreach (var interceptorRefName in interceptorRefNames)
            //{
            //    var interceptorTypeName = this.FindInterceptorTypeNameByRefName(interceptorRefName);
            //    if (!string.IsNullOrEmpty(interceptorTypeName))
            //        ret.Add(interceptorTypeName);
            //}
            //return ret;
            throw new NotImplementedException();
        }
    }
}