// FileInformation: nyanya/Cat.Domain.Yilian/UserEventsHandler.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:24 PM

using System.Threading.Tasks;
using Cat.Domain.Yilian.Models;
using Cat.Events.Users;
using Domian.Config;
using Domian.Events;
using Infrastructure.Lib.Utility;

namespace Cat.Domain.Yilian.EventHandlers
{
    public class UserEventsHandler : EventHandlerBase,
        IEventHandler<AppliedForSignUpPayment>,
        IEventHandler<AppliedForAddBankCard>
    {
        public UserEventsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        #region IEventHandler<AppliedForAddBankCard> Members

        public async Task Handler(AppliedForAddBankCard @event)
        {
            await this.DoAsync(this.ProcessYilianAuthRequest, @event);
        }

        #endregion IEventHandler<AppliedForAddBankCard> Members

        #region IEventHandler<AppliedForSignUpPayment> Members

        public async Task Handler(AppliedForSignUpPayment @event)
        {
            await this.DoAsync(this.ProcessYilianAuthRequest, @event);
        }

        #endregion IEventHandler<AppliedForSignUpPayment> Members

        private async Task ProcessYilianAuthRequest(IAppliedForYilianAuth @event)
        {
            YilianAuthRequest request = new YilianAuthRequest(GuidUtils.NewGuidString());
            await request.CreateFromEvent(@event);
            await request.SendRequestAsync();
        }
    }
}