// FileInformation: nyanya/Infrastructure.Lib.CQRS/AdditionalInterfaceToProxyAttribute.cs
// CreatedTime: 2014/06/04   4:47 PM
// LastUpdatedTime: 2014/06/04   4:47 PM

using System;

namespace Infrastructure.Lib.CQRS.Interception
{
    /// <summary>
    ///     Represents that the decorated classes are requiring additional interfaces
    ///     to be intercepted when Castle Dynamic Proxy is creating the proxy objects
    ///     for these classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class AdditionalInterfaceToProxyAttribute : Attribute
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the type of the interface that needs to be intercepted
        ///     when the proxy object is created.
        /// </summary>
        public Type InterfaceType { get; set; }

        #endregion Public Properties

        #region Ctor

        /// <summary>
        ///     Initializes a new instance of <c>AdditionalInterfaceToProxyAttribute</c>.
        /// </summary>
        /// <param name="intfType">
        ///     The type of the interface that needs to be intercepted
        ///     when the proxy object is create.
        /// </param>
        public AdditionalInterfaceToProxyAttribute(Type intfType)
        {
            this.InterfaceType = intfType;
        }

        #endregion Ctor
    }
}