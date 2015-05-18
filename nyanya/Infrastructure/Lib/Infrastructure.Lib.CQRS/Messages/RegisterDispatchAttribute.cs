// FileInformation: nyanya/Infrastructure.Lib.CQRS/RegisterDispatchAttribute.cs
// CreatedTime: 2014/06/05   10:18 AM
// LastUpdatedTime: 2014/06/05   10:18 AM

using System;

namespace Infrastructure.Lib.CQRS.Messages
{
    /// <summary>
    ///     Represents that the instances of the decorated interfaces
    ///     can be registered in a message dispatcher.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class RegisterDispatchAttribute : Attribute
    {
    }
}