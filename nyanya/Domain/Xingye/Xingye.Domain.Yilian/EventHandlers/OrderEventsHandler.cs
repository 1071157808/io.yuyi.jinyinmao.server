// FileInformation: nyanya/Xingye.Domain.Yilian/OrderEventsHandler.cs
// CreatedTime: 2014/09/02   1:13 PM
// LastUpdatedTime: 2014/09/03   10:24 AM

using System.Threading.Tasks;
using Domian.Config;
using Domian.Events;
using Infrastructure.Lib.Utility;
using Xingye.Domain.Yilian.Models;
using Xingye.Events.Orders;

namespace Xingye.Domain.Yilian.EventHandlers
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