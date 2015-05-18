// FileInformation: nyanya/nyanya.Meow/EnumTypeModelDescription.cs
// CreatedTime: 2014/09/01   10:42 AM
// LastUpdatedTime: 2014/09/01   10:50 AM

using System.Collections.ObjectModel;

namespace nyanya.Meow.Areas.HelpPage.ModelDescriptions
{
    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            this.Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}