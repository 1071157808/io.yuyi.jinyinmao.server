// FileInformation: nyanya/Domian/AggregateMemento.cs
// CreatedTime: 2014/07/07   5:39 PM
// LastUpdatedTime: 2014/07/07   5:39 PM

namespace Domian.Events
{
    public class AggregateMemento
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return this.Value;
        }
    }
}