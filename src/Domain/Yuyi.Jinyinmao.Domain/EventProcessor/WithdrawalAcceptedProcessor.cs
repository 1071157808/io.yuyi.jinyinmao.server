// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  11:01 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  12:27 AM
// ***********************************************************************
// <copyright file="WithdrawalAcceptedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     WithdrawalAcceptedProcessor.
    /// </summary>
    public class WithdrawalAcceptedProcessor : EventProcessor<WithdrawalAccepted>, IWithdrawalAcceptedProcessor
    {
        #region IWithdrawalAcceptedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(WithdrawalAccepted @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                using (JYMDBContext db = new JYMDBContext())
                {
                    AccountTranscation accountTranscation = e.WithdrawalTranscation.ToDBAccountTranscationModel(e.Args);

                    AccountTranscation accountChargeTranscation = e.ChargeTranscation.ToDBAccountTranscationModel(e.Args);

                    if (!await db.ReadonlyQuery<AccountTranscation>().AnyAsync(t => t.TranscationIdentifier == accountTranscation.TranscationIdentifier))
                    {
                        db.Add(accountTranscation);
                    }

                    if (!await db.ReadonlyQuery<AccountTranscation>().AnyAsync(t => t.TranscationIdentifier == accountChargeTranscation.TranscationIdentifier))
                    {
                        db.Add(accountChargeTranscation);
                    }

                    await db.ExecuteSaveChangesAsync();
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IWithdrawalAcceptedProcessor Members
    }
}