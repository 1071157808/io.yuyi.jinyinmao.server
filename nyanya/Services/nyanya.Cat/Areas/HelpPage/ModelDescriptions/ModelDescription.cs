// FileInformation: nyanya/nyanya.Cat/ModelDescription.cs
// CreatedTime: 2014/09/01   10:40 AM
// LastUpdatedTime: 2014/09/01   10:55 AM

using System;

namespace nyanya.Cat.Areas.HelpPage.ModelDescriptions
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