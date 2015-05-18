// FileInformation: nyanya/Domian/IEventHandler.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/07/26   7:29 PM

using System.Threading.Tasks;

namespace Domian.Events
{
    public interface IEventHandler<in T>
    {
        Task Handler(T @event);
    }
}