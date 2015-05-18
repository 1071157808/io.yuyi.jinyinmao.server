// FileInformation: nyanya/Cqrs.Domain.User/YilianAuthRequestEventsHandler.cs
// CreatedTime: 2014/07/19   10:02 PM
// LastUpdatedTime: 2014/07/24   9:53 AM

using System;
using System.Linq;
using System.Threading.Tasks;
using Cqrs.Domain.Users.Helper;
using Cqrs.Events.User;
using Infrastructure.Gateway.Payment.Yilian;

namespace Cqrs.Domain.Users.EventHandlers
{
    public partial class UserEventsHandler
    {
        #region IEventHandler<AppliedForAddBankCard> Members

        /// <summary>
        /// Handlers the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        public void Handler(AppliedForAddBankCard @event)
        {
            this.DoHandler(this.ProcessYilianAuthRequest, @event);
        }

        #endregion IEventHandler<AppliedForAddBankCard> Members

        #region IEventHandler<AppliedForSignUpPayment> Members

        /// <summary>
        /// Handlers the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        public void Handler(AppliedForSignUpPayment @event)
        {
            this.DoHandler(this.ProcessYilianAuthRequest, @event);
        }

        #endregion IEventHandler<AppliedForSignUpPayment> Members

        private void ProcessYilianAuthRequest(IAppliedForYilianAuth @event)
        {
            string sequenceNo = @event.SequenceNo;
            string accNo = @event.BankCardNo;
            string accName = @event.RealName;
            string accProvince = @event.CityName.Split(new[] { '|' }).First();
            string accCity = @event.CityName.Split(new[] { '|' }).Last();
            string bankName = @event.BankName;
            string idType = @event.CredentialCode;
            string idNo = @event.CredentialNo;
            string mobileNo = @event.Cellphone;
            string userUuid = @event.UserIdentifier;

            IYilianPaymentGatewayService service = new YilianPaymentGatewayService();

            Task<bool> task = service.UserAuthRequestAsync(new UserAuthRequestParameter("", sequenceNo, accNo, accName, accProvince,
                accCity, bankName, idType, idNo, mobileNo, userUuid));
            task.Wait();

            if (task.IsCompleted && task.Result)
            {
                return;
            }

            if (task.IsFaulted)
            {
                throw new NotImplementedException();
            }

            if (!task.Result)
            {
                throw new NotImplementedException();
            }
        }
    }
}