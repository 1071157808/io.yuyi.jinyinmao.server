// FileInformation: nyanya/Infrastructure.Lib.CQRS/RegisterCommandDispatchAttribute.cs
// CreatedTime: 2014/06/05   11:20 AM
// LastUpdatedTime: 2014/06/05   11:21 AM

using System;

namespace Infrastructure.Lib.CQRS.Messages
{
    /// <summary>
    ///     Represents that the instances of the decorated interfaces
    ///     can be registered in a command dispatcher.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class RegisterCommandDispatchAttribute : Attribute
    {
    }
}