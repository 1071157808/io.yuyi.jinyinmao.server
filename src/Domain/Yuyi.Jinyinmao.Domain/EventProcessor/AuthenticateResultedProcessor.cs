// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  6:08 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  3:13 AM
// ***********************************************************************
// <copyright file="AuthenticateResultedProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

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
        public override async Task ProcessEventAsync(AuthenticateResulted @event)
        {
            string userIdentifier = @event.UserId.ToGuidString();
            await this.ProcessingEventAsync(@event, async e =>
            {
                using (JYMDBContext db = new JYMDBContext())
                {
                    Models.User user = await db.Query<Models.User>().FirstAsync(u => u.UserIdentifier == userIdentifier);

                    user.RealName = e.RealName;
                    user.Credential = (int)e.Credential;
                    user.CredentialNo = e.CredentialNo;
                    user.Verified = true;
                    user.VerifiedTime = e.VerifiedTime;

                    await db.SaveChangesAsync();
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IAuthenticateResultedProcessor Members
    }
}