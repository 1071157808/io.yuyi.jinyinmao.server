// FileInformation: nyanya/Cat.Domain.Users/YilianEventsHandler.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   5:23 PM

using System.Threading.Tasks;
using Cat.Domain.Users.Models;
using Cat.Events.Yilian;
using Domian.Config;
using Domian.Events;

namespace Cat.Domain.Users.EventHandlers
{
    public class YilianEventsHandler : EventHandlerBase,
        IEventHandler<YilianAuthRequestSended>,
        IEventHandler<YilianAuthRequestCallbackProcessed>,
        IEventHandler<YilianQueryAuthRequestProcessed>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianEventsHandler" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public YilianEventsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        #region IEventHandler<YilianAuthRequestCallbackProcessed> Members

        public async Task Handler(YilianAuthRequestCallbackProcessed @event)
        {
            await this.DoAsync(async e =>
            {
                User user = new User(@event.UserIdentifier);
                await user.GotYilianVerifyResultAsync(e);
            }, @event);
        }

        #endregion IEventHandler<YilianAuthRequestCallbackProcessed> Members

        #region IEventHandler<YilianAuthRequestSended> Members

        public async Task Handler(YilianAuthRequestSended @event)
        {
            if (!@event.Result)
            {
                await this.DoAsync(async e =>
                {
                    User user = new User(e.UserIdentifier);
                    await user.GotYilianVerifyResultAsync(e);
                }, @event);
            }
        }

        #endregion IEventHandler<YilianAuthRequestSended> Members

        #region IEventHandler<YilianQueryAuthRequestProcessed> Members

        public async Task Handler(YilianQueryAuthRequestProcessed @event)
        {
            await this.DoAsync(async e =>
            {
                User user = new User(@event.UserIdentifier);
                await user.GotYilianVerifyResultAsync(e);
            }, @event);
        }

        #endregion IEventHandler<YilianQueryAuthRequestProcessed> Members
    }
}