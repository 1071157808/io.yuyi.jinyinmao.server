// FileInformation: nyanya/Xingye.Domain.Meow/SmsTriggerEventsHandler.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/03   10:31 AM

using System.Threading.Tasks;
using Xingye.Events.Orders;
using Xingye.Events.Users;
using Domian.Config;
using Domian.Events;
using Infrastructure.Lib;
using Infrastructure.Lib.Extensions;
using Infrastructure.SMS;
using Xingye.Events.Orders;
using Xingye.Events.Users;

namespace Xingye.Domain.Meow.EventHandlers
{
    public class SmsTriggerEventsHandler : EventHandlerBase,
        IEventHandler<RegisteredANewUser>,
        IEventHandler<SignUpPaymentSucceeded>,
        IEventHandler<AddBankCardSucceeded>,
        IEventHandler<SignUpPaymentFailed>,
        IEventHandler<AddBankCardFailed>,
        IEventHandler<OrderPaymentSuccessed>,
        IEventHandler<OrderPaymentFailed>
    {
        private readonly ISmsService smsService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SmsTriggerEventsHandler" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="smsService">The SMS service.</param>
        public SmsTriggerEventsHandler(CqrsConfiguration config, ISmsService smsService)
            : base(config)
        {
            this.smsService = smsService;
        }

        #region IEventHandler<AddBankCardFailed> Members

        public async Task Handler(AddBankCardFailed @event)
        {
            await this.DoAsync(async e => { await this.SendSmsForAddingBankCardFailed(e); }, @event);
        }

        #endregion IEventHandler<AddBankCardFailed> Members

        #region IEventHandler<AddBankCardSucceeded> Members

        public async Task Handler(AddBankCardSucceeded @event)
        {
            await this.DoAsync(async e => { await this.SendSmsForAddedABankCard(e); }, @event);
        }

        #endregion IEventHandler<AddBankCardSucceeded> Members

        #region IEventHandler<OrderPaymentFailed> Members

        public async Task Handler(OrderPaymentFailed @event)
        {
            await this.DoAsync(async e =>
            {
                string cellphone = e.Cellphone;
                string message = NyanyaResources.Sms_xy_PaidForOrderFailed.FormatWith(e.OrderNo, e.Amount.ToIntFormat(), e.Message.RemoveToFirst(':').RemoveToFirst('：'));
                await this.smsService.SendAsync(cellphone, message, 1);
            }, @event);
        }

        #endregion IEventHandler<OrderPaymentFailed> Members

        #region IEventHandler<OrderPaymentSuccessed> Members

        public async Task Handler(OrderPaymentSuccessed @event)
        {
            await this.DoAsync(async e =>
            {
                string cellphone = e.Cellphone;
                string message = NyanyaResources.Sms_xy_PaidForOrder.FormatWith(e.OrderNo, e.Amount.ToIntFormat());
                await this.smsService.SendAsync(cellphone, message, 1);
            }, @event);
        }

        #endregion IEventHandler<OrderPaymentSuccessed> Members

        #region IEventHandler<RegisteredANewUser> Members

        public async Task Handler(RegisteredANewUser @event)
        {
            await this.DoAsync(async e =>
            {
                string cellphone = e.Cellphone;
                string message = NyanyaResources.Sms_xy_SignUpSuccessful;
                await this.smsService.SendAsync(cellphone, message, 1);
            }, @event);
        }

        #endregion IEventHandler<RegisteredANewUser> Members

        #region IEventHandler<SignUpPaymentFailed> Members

        public async Task Handler(SignUpPaymentFailed @event)
        {
            await this.DoAsync(async e => { await this.SendSmsForAddingBankCardFailed(e); }, @event);
        }

        #endregion IEventHandler<SignUpPaymentFailed> Members

        #region IEventHandler<SignUpPaymentSucceeded> Members

        public async Task Handler(SignUpPaymentSucceeded @event)
        {
            await this.DoAsync(async e => { await this.SendSmsForAddedABankCard(e); }, @event);
        }

        #endregion IEventHandler<SignUpPaymentSucceeded> Members

        private async Task SendSmsForAddedABankCard(IAddedABankCard e)
        {
            string cellphone = e.Cellphone;
            string message = NyanyaResources.Sms_xy_AddBankCardSuccessed.FormatWith(e.BankCardNo.GetLast(4));
            await this.smsService.SendAsync(cellphone, message, 1);
        }

        private async Task SendSmsForAddingBankCardFailed(IAddingBankCardFailed e)
        {
            string cellphone = e.Cellphone;
            string message = NyanyaResources.Sms_xy_AddBankCardFailed.FormatWith(e.BankCardNo.GetLast(4), e.Message.RemoveToFirst(':').RemoveToFirst('：'));
            await this.smsService.SendAsync(cellphone, message, 1);
        }
    }
}