// FileInformation: nyanya/nyanya.Meow/ModelDescription.cs
// CreatedTime: 2014/09/01   10:42 AM
// LastUpdatedTime: 2014/09/01   10:50 AM

using System;

namespace nyanya.Meow.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    ///     Describes a type model.
    /// </summary>
    public abstract class ModelDescription
    {
        public string Documentation { get; set; }

        public Type ModelType { get; set; }

        public string Name { get; set; }
    }
}