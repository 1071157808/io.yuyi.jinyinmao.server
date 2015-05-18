// FileInformation: nyanya/Domian/IHasMemento.cs
// CreatedTime: 2014/07/14   4:42 PM
// LastUpdatedTime: 2014/07/20   7:26 PM

namespace Domian.Events
{
    public interface IHasMemento
    {
        AggregateMemento GetMemento();
    }
}