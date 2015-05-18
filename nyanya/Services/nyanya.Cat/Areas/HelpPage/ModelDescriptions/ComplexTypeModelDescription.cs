// FileInformation: nyanya/nyanya.Cat/ComplexTypeModelDescription.cs
// CreatedTime: 2014/09/01   10:40 AM
// LastUpdatedTime: 2014/09/01   10:55 AM

using System.Collections.ObjectModel;

namespace nyanya.Cat.Areas.HelpPage.ModelDescriptions
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