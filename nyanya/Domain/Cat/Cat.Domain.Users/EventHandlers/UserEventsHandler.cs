// FileInformation: nyanya/Cqrs.Domain.User/UserEventsHandler.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/07/27   3:51 AM

using System.Threading.Tasks;
using Cat.Events.Users;
using Cat.Domain.Users.Services;
using Cat.Domain.Users.Services.Interfaces;
using Domian.Config;
using Domian.Events;

namespace Cat.Domain.Users.EventHandlers
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
        public async Task Handler(UserSignInSucceeded @event)
        {
            await this.DoAsync(async e =>
            {
                IUserOldPlatformService userService = new UserOldPlatformService();
                await userService.SendSignInRequestAsync(@event.AmpAuthToken, @event.GoldCatAuthToken, @event.UserIdentifier);
            }, @event);
        }
    }
}