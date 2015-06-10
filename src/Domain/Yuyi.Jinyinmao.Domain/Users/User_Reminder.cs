// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-10  11:35 AM
// ***********************************************************************
// <copyright file="User_Reminder.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans.Runtime;
using Yuyi.Jinyinmao.Domain.Dtos;
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

        #region IUser Members

        /// <summary>
        ///     do daily work as an asynchronous operation.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns>Task.</returns>
        public async Task DoDailyWorkAsync(bool force = false)
        {
            try
            {
                if (force || (DateTime.UtcNow.AddHours(8).Hour <= 4 && DateTime.UtcNow.AddHours(8).Hour >= 1))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("UserDailyWork: {0}\n".FormatWith(this.State.Id));

                    DateTime now = DateTime.UtcNow.AddHours(8);
                    List<JBYAccountTranscation> jbyWithdrawalTranscations = this.State.JBYAccount.Values
                        .Where(t => t.TradeCode == TradeCodeHelper.TC2001012002 && t.ResultCode == 0 && t.PredeterminedResultDate.HasValue
                                    && t.PredeterminedResultDate.GetValueOrDefault(DateTime.MaxValue).Date < now)
                        .ToList();

                    builder.Append("JBYWithdrawalResulted: ");

                    foreach (JBYAccountTranscation transcation in jbyWithdrawalTranscations)
                    {
                        await this.JBYWithdrawalResultedAsync(transcation.TransactionId);
                        builder.Append(transcation.TransactionId + " ");
                    }

                    builder.Append("\n");

                    JBYAccountTranscationInfo transcationInfo = await this.JBYReinvestingAsync();
                    builder.Append(transcationInfo == null ? "JBYReinvesting: SKIPED." : "JBYReinvesting: {0}".FormatWith(transcationInfo.ToJson()));

                    Trace.TraceWarning(builder.ToString());
                }
            }
            catch (Exception e)
            {
                SiloClusterErrorLogger.Log(new ErrorLog
                {
                    Exception = e.GetExceptionString(),
                    Message = e.Message,
                    PartitionKey = this.State.Id.ToGuidString(),
                    RowKey = "UserDailyWorkError-{0}".FormatWith(DateTime.UtcNow)
                });
            }
        }

        #endregion IUser Members
    }
}