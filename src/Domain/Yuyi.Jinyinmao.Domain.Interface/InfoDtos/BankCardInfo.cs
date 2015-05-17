// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-15  1:48 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-17  4:01 PM
// ***********************************************************************
// <copyright file="BankCardInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Moe.Lib;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     BankCardEx.
    /// </summary>
    public static class BankCardEx
    {
        /// <summary>
        /// To the information.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="withdrawAmount">The withdraw amount.</param>
        /// <returns>BankCardInfo.</returns>
        public static BankCardInfo ToInfo(this BankCard card, int withdrawAmount)
        {
            return new BankCardInfo
            {
                AddingTime = card.AddingTime,
                Args = card.Args,
                BankCardNo = card.BankCardNo,
                BankName = card.BankName,
                Cellphone = card.Cellphone,
                CityName = card.CityName,
                Dispaly = card.Dispaly,
                UserId = card.UserId,
                Verified = card.Verified,
                VerifiedByYilian = card.VerifiedByYilian,
                VerifiedTime = card.VerifiedTime.GetValueOrDefault(),
                WithdrawAmount = withdrawAmount
            };
        }

        /// <summary>
        /// To the information.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>BankCardInfo.</returns>
        public static BankCardInfo ToInfo(this BankCard card)
        {
            return new BankCardInfo
            {
                AddingTime = card.AddingTime,
                Args = card.Args,
                BankCardNo = card.BankCardNo,
                BankName = card.BankName,
                Cellphone = card.Cellphone,
                CityName = card.CityName,
                Dispaly = card.Dispaly,
                UserId = card.UserId,
                Verified = card.Verified,
                VerifiedByYilian = card.VerifiedByYilian,
                VerifiedTime = card.VerifiedTime.GetValueOrDefault(),
                WithdrawAmount = card.WithdrawAmount
            };
        }
    }

    /// <summary>
    ///     BankCardEx.
    /// </summary>
    public static class BankCardInfoEx
    {
        /// <summary>
        ///     Maps to database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="model">The model.</param>
        public static void MapToDBModel(this BankCardInfo info, Models.BankCard model)
        {
            Dictionary<string, object> i = BuildBankCardModelInfo(info);

            model.Args = info.Args.ToJson();
            model.BankCardNo = info.BankCardNo;
            model.BankName = info.BankName;
            model.CityName = info.CityName;
            model.Info = i.ToJson();
            model.UserIdentifier = info.UserId.ToGuidString();
            model.VerifiedTime = info.VerifiedTime;
        }

        /// <summary>
        ///     To the database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Models.BankCard.</returns>
        public static Models.BankCard ToDBModel(this BankCardInfo info)
        {
            Dictionary<string, object> i = BuildBankCardModelInfo(info);

            return new Models.BankCard
            {
                Args = info.Args.ToJson(),
                BankCardNo = info.BankCardNo,
                BankName = info.BankName,
                CityName = info.CityName,
                Info = i.ToJson(),
                UserIdentifier = info.UserId.ToGuidString(),
                VerifiedTime = info.VerifiedTime
            };
        }

        private static Dictionary<string, object> BuildBankCardModelInfo(BankCardInfo info)
        {
            Dictionary<string, object> i = new Dictionary<string, object>
            {
                { "Cellphone", info.Cellphone },
                { "Dispaly", info.Dispaly },
                { "VerifiedByYilian", info.VerifiedByYilian }
            };
            return i;
        }
    }

    /// <summary>
    ///     BankCardInfo.
    /// </summary>
    [Immutable]
    public class BankCardInfo
    {
        /// <summary>
        ///     Gets or sets the adding time.
        /// </summary>
        /// <value>The adding time.</value>
        public DateTime AddingTime { get; set; }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        /// <value>The bank card no.</value>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        /// <value>The name of the bank.</value>
        public string BankName { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the name of the city.
        /// </summary>
        /// <value>The name of the city.</value>
        public string CityName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BankCard" /> is dispaly.
        /// </summary>
        /// <value><c>true</c> if dispaly; otherwise, <c>false</c>.</value>
        public bool Dispaly { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BankCard" /> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [verified by yilian].
        /// </summary>
        /// <value><c>true</c> if [verified by yilian]; otherwise, <c>false</c>.</value>
        public bool VerifiedByYilian { get; set; }

        /// <summary>
        ///     Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        public DateTime VerifiedTime { get; set; }

        /// <summary>
        ///     Gets or sets the withdraw amount.
        /// </summary>
        /// <value>The withdraw amount.</value>
        public int WithdrawAmount { get; set; }
    }
}