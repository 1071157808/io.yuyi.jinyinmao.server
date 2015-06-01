// ***********************************************************************
// Project          : nyanya
// Author           : Siqi Lu
// Created          : 2015-05-18  2:53 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-01  3:18 PM
// ***********************************************************************
// <copyright file="UserEventsHandler.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Cat.Domain.Users.Services;
using Cat.Domain.Users.Services.Interfaces;
using Cat.Events.Users;
using Domian.Config;
using Domian.Events;
using Moe.Lib;

namespace Cat.Domain.Users.EventHandlers
{
    public class UserEventsHandler : EventHandlerBase, IEventHandler<UserSignInSucceeded>, IEventHandler<RegisteredANewUser>
        //IEventHandler<PasswordChanged>,
        //IEventHandler<PaymentPasswordSet>
    {
        private static string activityNotifyUrl;

        public UserEventsHandler(CqrsConfiguration config)
            : base(config)
        {
            activityNotifyUrl = ConfigurationManager.AppSettings.Get("ActivityNotifyUrl");
        }

        #region IEventHandler<RegisteredANewUser> Members

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

        public async Task Handler(RegisteredANewUser @event)
        {
            await this.DoAsync(async e =>
            {
                string timeStamp = DateTime.UtcNow.UnixTimeStamp().ToString();
                string toSign = timeStamp + "ydse@bjkw34sdjfb7w4s#df";
                string sign = MD5Hash.ComputeMD5Hash(toSign);
                string url = "{0}?dt={1}&sign={2}&userIdentifier={3}".FormatWith(activityNotifyUrl, timeStamp, sign, e.UserIdentifier);
                using (HttpClient client = new HttpClient())
                {
                    await client.GetAsync(url);
                }
            }, @event);
        }

        #endregion IEventHandler<RegisteredANewUser> Members

        #region IEventHandler<UserSignInSucceeded> Members

        //#endregion IEventHandler<RegisteredANewUser> Members
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