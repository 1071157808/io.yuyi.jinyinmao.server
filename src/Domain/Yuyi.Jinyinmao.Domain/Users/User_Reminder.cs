// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-18  11:37 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-24  4:33 PM
// ***********************************************************************
// <copyright file="User_Reminder.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans.Runtime;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     User.
    /// </summary>
    public partial class User
    {
        #region IRemindable Members

        /// <summary>
        ///     Receieve a new Reminder.
        /// </summary>
        /// <param name="reminderName">Name of this Reminder</param>
        /// <param name="status">Status of this Reminder tick</param>
        /// <returns>
        ///     Completion promise which the grain will resolve when it has finished processing this Reminder tick.
        /// </returns>
        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            if (reminderName == "DailyWork")
            {
                await this.DoDailyWorkAsync();
            }
        }

        #endregion IRemindable Members

        /// <summary>
        /// do daily work as an asynchronous operation.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns>Task.</returns>
        public async Task DoDailyWorkAsync(bool force = false)
        {
            if (force || (DateTime.UtcNow.AddHours(8).Hour <= 4 && DateTime.UtcNow.AddHours(8).Hour >= 1))
            {
                DateTime now = DateTime.UtcNow.AddHours(8);
                List<JBYAccountTranscation> jbyWithdrawalTranscations = this.State.JBYAccount.Values
                    .Where(t => t.TradeCode == TradeCodeHelper.TC2001012002 && t.ResultCode == 0 && t.PredeterminedResultDate.HasValue
                                && t.PredeterminedResultDate.GetValueOrDefault(DateTime.MaxValue).Date < now)
                    .ToList();

                foreach (JBYAccountTranscation transcation in jbyWithdrawalTranscations)
                {
                    await this.JBYWithdrawalResultedAsync(transcation.TransactionId);
                }

                await this.JBYReinvestingAsync();
            }
        }
    }
}