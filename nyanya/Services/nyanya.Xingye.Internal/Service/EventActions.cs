// FileInformation: nyanya/nyanya.Xingye.Internal/Actions.cs
// CreatedTime: 2014/08/28   10:18 AM
// LastUpdatedTime: 2014/08/28   10:40 AM

using System.Threading.Tasks;
using Xingye.Events.Orders;
using Xingye.Events.Products;
using Xingye.Events.Users;
using Xingye.Events.Yilian;

namespace nyanya.Xingye.Internal.Service
{
    public partial class EventHandlerService
    {
        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(OrderBuilded @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(OrderPaymentFailed @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(OrderPaymentSuccessed @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(ProductRepaid @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(AppliedForAddBankCard @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(AppliedForSignUpPayment @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(YilianAuthRequestSended @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(YilianAuthRequestCallbackReceived @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(YilianAuthRequestCallbackProcessed @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(YilianQueryAuthRequestProcessed @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(YilianPaymentRequestSended @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(YilianPaymentRequestCallbackReceived @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(YilianPaymentRequestCallbackProcessed @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(YilianQueryPaymentRequestProcessed @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(UserSignInSucceeded @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(RegisteredANewUser @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(SignUpPaymentSucceeded @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(AddBankCardSucceeded @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(SignUpPaymentFailed @event)
        {
            return await this.Handler(@event);
        }

        /// <summary>
        ///     Anies the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public async Task<object> Any(AddBankCardFailed @event)
        {
            return await this.Handler(@event);
        }
    }
}