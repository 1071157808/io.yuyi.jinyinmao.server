// FileInformation: nyanya/nyanya.Meow/ComplexTypeModelDescription.cs
// CreatedTime: 2014/09/01   10:42 AM
// LastUpdatedTime: 2014/09/01   10:50 AM

using System.Collections.ObjectModel;

namespace nyanya.Meow.Areas.HelpPage.ModelDescriptions
{
    public class ComplexTypeModelDescription : ModelDescription
    {
        public ComplexTypeModelDescription()
        {
            this.Properties = new Collection<ParameterDescription>();
        }

        public Collection<ParameterDescription> Properties { get; private set; }
    }
}