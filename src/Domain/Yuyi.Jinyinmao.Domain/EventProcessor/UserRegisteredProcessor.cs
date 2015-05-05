// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  2:42 AM
// ***********************************************************************
// <copyright file="UserRegisteredProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     UserRegisteredProcessor.
    /// </summary>
    public class UserRegisteredProcessor : EventProcessor<UserRegistered>, IUserRegisteredProcessor
    {
        #region IUserRegisteredProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(UserRegistered @event)
        {
            await this.ProcessingEventAsync(@event, async e => { await this.SmsService.SendMessageAsync(e.Cellphone, Resources.Sms_SignUpSuccessful); });

            await this.ProcessingEventAsync(@event, async e =>
            {
                ICellphone cellphone = CellphoneFactory.GetGrain(GrainTypeHelper.GetGrainTypeLongKey(GrainType.Cellphone, e.Cellphone));
                await cellphone.Register();
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                Models.User user = new Models.User
                {
                    Args = e.Args,
                    Cellphone = e.Cellphone,
                    ClientType = e.ClientType,
                    Closed = false,
                    ContractId = e.ContractId,
                    Credential = (int)Credential.None,
                    CredentialNo = string.Empty,
                    Info = new Dictionary<string, object>().ToJson(),
                    InviteBy = e.InviteBy,
                    LoginNames = e.LoginNames.Join(","),
                    OutletCode = e.OutletCode,
                    RealName = string.Empty,
                    RegisterTime = e.RegisterTime,
                    UserIdentifier = e.UserId.ToGuidString(),
                    Verified = false,
                    VerifiedTime = null
                };

                using (JYMDBContext db = new JYMDBContext())
                {
                    if (await db.Users.AnyAsync(u => u.UserIdentifier == user.UserIdentifier))
                    {
                        return;
                    }

                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IUserRegisteredProcessor Members
    }
}