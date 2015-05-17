// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:53 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  3:21 AM
// ***********************************************************************
// <copyright file="LoginPasswordResetProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     LoginPasswordResetProcessor.
    /// </summary>
    public class LoginPasswordResetProcessor : EventProcessor<LoginPasswordReset>, ILoginPasswordResetProcessor
    {
        #region ILoginPasswordResetProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(LoginPasswordReset @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = Resources.Sms_ResetLoginPawword;
                if (!await this.SmsService.SendMessageAsync(e.UserInfo.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.UserInfo.Cellphone, message));
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion ILoginPasswordResetProcessor Members
    }
}