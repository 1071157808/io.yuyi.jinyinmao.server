// FileInformation: nyanya/Xingye.Domain.Orders/YilianEventsHandler.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   4:11 PM

using System.Threading.Tasks;
using Domian.Config;
using Domian.Events;
using Xingye.Domain.Orders.Models;
using Xingye.Events.Yilian;

namespace Xingye.Domain.Orders.EventHandlers
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