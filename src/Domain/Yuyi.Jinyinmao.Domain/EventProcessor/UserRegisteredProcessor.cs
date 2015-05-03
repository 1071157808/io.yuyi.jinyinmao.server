// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-03  6:05 PM
// ***********************************************************************
// <copyright file="UserRegisteredProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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
        public override Task ProcessEventAsync(UserRegistered @event)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await this.SmsService.SendMessageAsync(@event.Cellphone, Resources.Sms_SignUpSuccessful);
                }
                catch (Exception e)
                {
                    this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                }
            });

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    ICellphone cellphone = CellphoneFactory.GetGrain(GrainTypeHelper.GetGrainTypeLongKey(GrainType.Cellphone, @event.Cellphone));
                    await cellphone.Register();
                }
                catch (Exception e)
                {
                    this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                }
            });

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    Models.User user = new Models.User
                    {
                        Args = @event.Args,
                        Cellphone = @event.Cellphone,
                        ClientType = @event.ClientType,
                        Closed = false,
                        ContractId = @event.ContractId,
                        Credential = (int)Credential.None,
                        CredentialNo = string.Empty,
                        Info = "{}",
                        InviteBy = @event.InviteBy,
                        LoginNames = @event.LoginNames.Join(","),
                        OutletCode = @event.OutletCode,
                        RealName = string.Empty,
                        RegisterTime = @event.RegisterTime,
                        UserIdentifier = @event.UserId.ToGuidString(),
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
                }
                catch (Exception e)
                {
                    this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                }
            });

            return base.ProcessEventAsync(@event);
        }

        #endregion IUserRegisteredProcessor Members
    }
}