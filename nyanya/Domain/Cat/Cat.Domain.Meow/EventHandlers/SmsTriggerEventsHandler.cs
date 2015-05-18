// FileInformation: nyanya/Cat.Domain.Meow/SmsTriggerEventsHandler.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:37 PM

using System.Threading;
using System.Threading.Tasks;
using Cat.Events.Orders;
using Cat.Events.Users;
using Domian.Config;
using Domian.Events;
using Infrastructure.Lib;
using Infrastructure.Lib.Extensions;
using Infrastructure.SMS;

namespace Cat.Domain.Meow.EventHandlers
{
    public class SmsTriggerEventsHandler : EventHandlerBase,
        IEventHandler<RegisteredANewUser>,
        IEventHandler<SignUpPaymentSucceeded>,
        IEventHandler<AddBankCardSucceeded>,
        IEventHandler<SignUpPaymentFailed>,
        IEventHandler<AddBankCardFailed>,
        IEventHandler<OrderPaymentSuccessed>,
        IEventHandler<OrderPaymentFailed>,
        IEventHandler<SetRedeemBillResult>
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
                string message = NyanyaResources.Sms_PaidForOrderFailed.FormatWith(e.ProductNo, e.OrderNo, e.Amount.ToIntFormat(), e.Message.RemoveToFirst(':').RemoveToFirst('：'));
                await this.smsService.SendAsync(cellphone, message);
            }, @event);
        }

        #endregion IEventHandler<OrderPaymentFailed> Members

        #region IEventHandler<OrderPaymentSuccessed> Members

        public async Task Handler(OrderPaymentSuccessed @event)
        {
            await this.DoAsync(async e =>
            {
                string cellphone = e.Cellphone;
                string message = NyanyaResources.Sms_PaidForOrder.FormatWith(e.ProductNo, e.OrderNo, e.Amount.ToIntFormat());
                await this.smsService.SendAsync(cellphone, message);
            }, @event);
        }

        #endregion IEventHandler<OrderPaymentSuccessed> Members

        #region IEventHandler<RegisteredANewUser> Members

        public async Task Handler(RegisteredANewUser @event)
        {
            await this.DoAsync(async e =>
            {
                string cellphone = e.Cellphone;
                string message = NyanyaResources.Sms_SignUpSuccessful;
                await this.smsService.SendAsync(cellphone, message);
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

        public Task Handler(SetRedeemBillResult @event)
        {
            //            await this.DoAsync(async e =>
            //            {
            //                string cellphone = e.Cellphone;
            //                string message = NyanyaResources.Sms_ZCBRedeemBillResult.FormatWith((e.Principal+e.RedeemInterest).ToFloor(2).ToString(), e.BankCardNo.GetLast(4), e.BankName);
            //                Thread.Sleep(1000);
            //                await this.smsService.SendAsync(cellphone, message);
            //            }, @event);
            return null;
        }

        #endregion IEventHandler<SignUpPaymentSucceeded> Members

        private async Task SendSmsForAddedABankCard(IAddedABankCard e)
        {
            string cellphone = e.Cellphone;
            string message = NyanyaResources.Sms_AddBankCardSuccessed.FormatWith(e.BankCardNo.GetLast(4));
            await this.smsService.SendAsync(cellphone, message);
        }

        private async Task SendSmsForAddingBankCardFailed(IAddingBankCardFailed e)
        {
            string cellphone = e.Cellphone;
            string message = NyanyaResources.Sms_AddBankCardFailed.FormatWith(e.BankCardNo.GetLast(4), e.Message.RemoveToFirst(':').RemoveToFirst('：'));
            await this.smsService.SendAsync(cellphone, message);
        }
    }
}