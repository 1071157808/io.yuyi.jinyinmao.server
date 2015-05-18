// FileInformation: nyanya/Infrastructure.Lib.CQRS/RegisterEventDispatchAttribute.cs
// CreatedTime: 2014/06/13   5:59 PM
// LastUpdatedTime: 2014/06/13   6:00 PM

using System;

namespace Infrastructure.Lib.CQRS.Messages
{
    /// <summary>
    ///     Represents that the instances of the decorated interfaces
    ///     can be registered in a command dispatcher.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class RegisterEventDispatchAttribute : Attribute
    {
    }
}