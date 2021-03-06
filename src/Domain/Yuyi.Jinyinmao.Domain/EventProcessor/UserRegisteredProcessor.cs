// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : UserRegisteredProcessor.cs
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-12  2:44 AM
// ***********************************************************************
// <copyright file="UserRegisteredProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain.Events
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
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = Resources.Sms_SignUpSuccessful;
                if (!await this.SmsService.SendMessageAsync(e.UserInfo.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.UserInfo.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                ICellphone cellphone = this.GrainFactory.GetGrain<ICellphone>(GrainTypeHelper.GetCellphoneGrainTypeLongKey(e.UserInfo.Cellphone));
                await cellphone.RegisterAsync(e.UserInfo.UserId);
            });

            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncUserAsync(e.UserInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion IUserRegisteredProcessor Members
    }
}