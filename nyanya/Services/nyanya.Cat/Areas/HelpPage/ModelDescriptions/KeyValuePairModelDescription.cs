// FileInformation: nyanya/nyanya.Cat/KeyValuePairModelDescription.cs
// CreatedTime: 2014/09/01   10:40 AM
// LastUpdatedTime: 2014/09/01   10:55 AM

namespace nyanya.Cat.Areas.HelpPage.ModelDescriptions
{
    public class KeyValuePairModelDescription : ModelDescription
    {
        public ModelDescription KeyModelDescription { get; set; }

        public ModelDescription ValueModelDescription { get; set; }
    }
}