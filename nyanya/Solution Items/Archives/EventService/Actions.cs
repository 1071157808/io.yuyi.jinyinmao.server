// FileInformation: nyanya/EventService/Actions.cs
// CreatedTime: 2014/08/13   1:18 AM
// LastUpdatedTime: 2014/08/14   9:53 AM

using System.Threading.Tasks;
using Cqrs.Events.Orders;
using Cqrs.Events.Products;
using Cqrs.Events.User;
using Cqrs.Events.Yilian;

namespace EventService
{
    public partial class EventHandlerService
    {
        #region Public Methods

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

        #endregion Public Methods
    }
}