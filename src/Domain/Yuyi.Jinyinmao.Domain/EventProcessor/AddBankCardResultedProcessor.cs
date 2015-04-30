// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:51 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-29  6:42 PM
// ***********************************************************************
// <copyright file="AddBankCardResultedProcessor.cs" company="Shanghai Yuyi">
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
    public class AddBankCardResultedProcessor : EventProcessor<AddBankCardResulted>, IAddBankCardResultedProcessor
    {
        #region IAddBankCardResultedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override Task ProcessEventAsync(AddBankCardResulted @event)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    string message = @event.Result ? Resources.Sms_AddBankCardSuccessed : Resources.Sms_AddBankCardFailed;
                    await this.SmsService.SendMessageAsync(@event.Cellphone, message);
                }
                catch (Exception e)
                {
                    this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                }
            });

            Task.Factory.StartNew(async () =>
            {
                if (@event.Result)
                {
                    try
                    {
                        Models.BankCard bankCard = new Models.BankCard
                        {
                            Args = @event.Args,
                            BankCardNo = @event.BankCardNo,
                            BankName = @event.BankName,
                            CityName = @event.CityName,
                            Info = (new { @event.CanBeUsedByYilian }).ToJson(),
                            IsDefault = false,
                            UserIdentifier = @event.UserId.ToGuidString(),
                            VerifiedTime = @event.VerifiedTime
                        };

                        using (JYMDBContext db = new JYMDBContext())
                        {
                            if (await db.BankCards.AnyAsync(c => c.BankCardNo == bankCard.BankCardNo))
                            {
                                return;
                            }

                            await db.SaveAsync(bankCard);
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

        #endregion IAddBankCardResultedProcessor Members
    }
}
