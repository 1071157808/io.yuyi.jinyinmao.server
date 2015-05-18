// FileInformation: nyanya/Cat.Domain.Orders/YilianEventsHandler.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   5:23 PM

using System.Threading.Tasks;
using Cat.Domain.Orders.Models;
using Cat.Events.Yilian;
using Domian.Config;
using Domian.Events;

namespace Cat.Domain.Orders.EventHandlers
{
    public class YilianEventsHandler : EventHandlerBase,
        IEventHandler<YilianPaymentRequestSended>,
        IEventHandler<YilianPaymentRequestCallbackProcessed>,
        IEventHandler<YilianQueryPaymentRequestProcessed>
    {
        public YilianEventsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        #region IEventHandler<YilianPaymentRequestCallbackProcessed> Members

        public async Task Handler(YilianPaymentRequestCallbackProcessed @event)
        {
            await this.DoAsync(async e =>
            {
                Order order = new Order(e.OrderIdentifier);
                await order.GotYilianPaymentResultAsync(e);
            }, @event);
        }

        #endregion IEventHandler<YilianPaymentRequestCallbackProcessed> Members

        #region IEventHandler<YilianPaymentRequestSended> Members

        public async Task Handler(YilianPaymentRequestSended @event)
        {
            if (!@event.Result)
            {
                await this.DoAsync(async e =>
                {
                    Order order = new Order(e.OrderIdentifier);
                    await order.GotYilianPaymentResultAsync(e);
                }, @event);
            }
        }

        #endregion IEventHandler<YilianPaymentRequestSended> Members

        #region IEventHandler<YilianQueryPaymentRequestProcessed> Members

        public async Task Handler(YilianQueryPaymentRequestProcessed @event)
        {
            await this.DoAsync(async e =>
            {
                Order order = new Order(e.OrderIdentifier);
                await order.GotYilianPaymentResultAsync(e);
            }, @event);
        }

        #endregion IEventHandler<YilianQueryPaymentRequestProcessed> Members
    }
}