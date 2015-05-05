// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:51 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  3:12 AM
// ***********************************************************************
// <copyright file="AddBankCardResultedProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     AddBankCardResultedProcessor.
    /// </summary>
    public class AddBankCardResultedProcessor : EventProcessor<AddBankCardResulted>, IAddBankCardResultedProcessor
    {
        #region IAddBankCardResultedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(AddBankCardResulted @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = e.Result ? Resources.Sms_AddBankCardSuccessed : Resources.Sms_AddBankCardFailed;
                await this.SmsService.SendMessageAsync(e.Cellphone, message.FormatWith(e.BankCardNo.GetLast(4)));
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                if (e.Result)
                {
                    Models.BankCard bankCard = new Models.BankCard
                    {
                        Args = e.Args,
                        BankCardNo = e.BankCardNo,
                        BankName = e.BankName,
                        CityName = e.CityName,
                        Info = (new { e.CanBeUsedByYilian }).ToJson(),
                        IsDefault = e.IsDefault,
                        UserIdentifier = e.UserId.ToGuidString(),
                        VerifiedTime = e.VerifiedTime
                    };

                    using (JYMDBContext db = new JYMDBContext())
                    {
                        if (await db.BankCards.AnyAsync(c => c.BankCardNo == bankCard.BankCardNo))
                        {
                            return;
                        }

                        if (e.IsDefault)
                        {
                            var defaultBankCards = await db.Query<BankCard>().Where(c => c.IsDefault).ToListAsync();
                            defaultBankCards.ForEach(c => c.IsDefault = false);
                        }

                        db.BankCards.Add(bankCard);

                        await db.ExecuteSaveChangesAsync();
                    }
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IAddBankCardResultedProcessor Members
    }
}