// FileInformation: nyanya/nyanya.Xingye/KeyValuePairModelDescription.cs
// CreatedTime: 2014/09/01   10:42 AM
// LastUpdatedTime: 2014/09/01   10:46 AM

namespace nyanya.Xingye.Areas.HelpPage.ModelDescriptions
{
    public class KeyValuePairModelDescription : ModelDescription
    {
        public ModelDescription KeyModelDescription { get; set; }

        public ModelDescription ValueModelDescription { get; set; }
    }
}