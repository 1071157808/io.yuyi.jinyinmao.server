// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  6:08 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-29  5:48 PM
// ***********************************************************************
// <copyright file="AuthenticateResultedProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     AddBankCardResultedProcessor.
    /// </summary>
    public class AuthenticateResultedProcessor : EventProcessor<AuthenticateResulted>, IAuthenticateResultedProcessor
    {
        #region IAuthenticateResultedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override Task ProcessEventAsync(AuthenticateResulted @event)
        {
            Task.Factory.StartNew(async () =>
            {
                if (@event.Result)
                {
                    try
                    {
                        using (JYMDBContext db = new JYMDBContext())
                        {
                            Models.User user = await db.Query<Models.User>().FirstAsync(u => u.UserIdentifier == @event.UserId.ToGuidString());

                            user.RealName = @event.RealName;
                            user.Credential = (int)@event.Credential;
                            user.CredentialNo = @event.CredentialNo;
                            user.Verified = true;
                            user.VerifiedTime = @event.VerifiedTime;

                            await db.SaveChangesAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                    }
                }
            });

            return base.ProcessEventAsync(@event);
        }

        #endregion IAuthenticateResultedProcessor Members
    }
}
