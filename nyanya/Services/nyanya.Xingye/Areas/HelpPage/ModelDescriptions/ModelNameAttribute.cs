// FileInformation: nyanya/nyanya.Xingye/ModelNameAttribute.cs
// CreatedTime: 2014/09/01   10:42 AM
// LastUpdatedTime: 2014/09/01   10:46 AM

using System;

namespace nyanya.Xingye.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    ///     Use this attribute to change the name of the <see cref="ModelDescription" /> generated for a type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public sealed class ModelNameAttribute : Attribute
    {
        public ModelNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}