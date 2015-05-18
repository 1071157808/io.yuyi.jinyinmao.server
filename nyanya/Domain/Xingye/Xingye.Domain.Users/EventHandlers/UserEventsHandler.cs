// FileInformation: nyanya/Xingye.Domain.Users/UserEventsHandler.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:29 PM

using System.Threading.Tasks;
using Domian.Config;
using Domian.Events;
using Xingye.Domain.Users.Services;
using Xingye.Domain.Users.Services.Interfaces;
using Xingye.Events.Users;

namespace Xingye.Domain.Users.EventHandlers
{
    public class UserEventsHandler : EventHandlerBase, IEventHandler<UserSignInSucceeded> //, IEventHandler<RegisteredANewUser>,
        //IEventHandler<PasswordChanged>,
        //IEventHandler<PaymentPasswordSet>
    {
        public UserEventsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        //#region IEventHandler<PasswordChanged> Members

        //public Task Handler(PasswordChanged @event)
        //{
        //    return null;
        //}

        //#endregion IEventHandler<PasswordChanged> Members

        //#region IEventHandler<PaymentPasswordSet> Members

        //public Task Handler(PaymentPasswordSet @event)
        //{
        //    return null;
        //}

        //#endregion IEventHandler<PaymentPasswordSet> Members

        //#region IEventHandler<RegisteredANewUser> Members

        //public Task Handler(RegisteredANewUser @event)
        //{
        //    return null;
        //}

        //#endregion IEventHandler<RegisteredANewUser> Members

        #region IEventHandler<UserSignInSucceeded> Members

        public async Task Handler(UserSignInSucceeded @event)
        {
            await this.DoAsync(async e =>
            {
                IUserOldPlatformService userService = new UserOldPlatformService();
                await userService.SendSignInRequestAsync(@event.AmpAuthToken, @event.GoldCatAuthToken, @event.UserIdentifier);
            }, @event);
        }

        #endregion IEventHandler<UserSignInSucceeded> Members
    }
}