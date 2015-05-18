// FileInformation: nyanya/Infrastructure.Lib.CQRS/IEventHandler.cs
// CreatedTime: 2014/06/13   4:58 PM
// LastUpdatedTime: 2014/06/13   5:00 PM

using System.Threading.Tasks;
using Infrastructure.Lib.CQRS.Bus;

namespace Infrastructure.Lib.CQRS.Messages
{
    public interface IEventHandler
    {
        string Name { get; }
    }

    public interface IEventHandler<in T> : IEventHandler where T : IEvent
    {
        /// <summary>
        ///     Handles the specified event.
        /// </summary>
        /// <param name="event">The event to be handled.</param>
        Task Handle(T @event);
    }
}