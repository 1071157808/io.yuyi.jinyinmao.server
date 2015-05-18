// FileInformation: nyanya/Cat.Domain.Yilian/OrderEventsHandler.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:24 PM

using System.Threading.Tasks;
using Cat.Domain.Yilian.Models;
using Cat.Events.Orders;
using Domian.Config;
using Domian.Events;
using Infrastructure.Lib.Utility;

namespace Cat.Domain.Yilian.EventHandlers
{
    public class OrderEventsHandler : EventHandlerBase, IEventHandler<OrderBuilded>
    {
        public OrderEventsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        #region IEventHandler<OrderBuilded> Members

        /// <summary>
        ///     Handlers the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task Handler(OrderBuilded @event)
        {
            await this.DoAsync(async e =>
            {
                YilianPaymentRequest request = new YilianPaymentRequest(GuidUtils.NewGuidString());
                await request.CreateFromEvent(e);
                await request.SendRequestAsync();
            }, @event);
        }

        #endregion IEventHandler<OrderBuilded> Members
    }
}