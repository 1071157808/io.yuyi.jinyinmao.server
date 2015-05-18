// FileInformation: nyanya/Domain.Amp/MeowConfiguration.cs
// CreatedTime: 2014/03/30   8:08 PM
// LastUpdatedTime: 2014/04/21   11:46 PM

namespace Domain.Amp.Models
{
    public class MeowConfiguration
    {
        public virtual string Description { get; set; }

        public virtual long Id { get; set; }

        public virtual string Key { get; set; }

        public virtual string Value { get; set; }
    }
}