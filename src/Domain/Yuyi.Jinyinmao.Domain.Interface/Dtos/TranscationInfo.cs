// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  7:02 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-03  7:04 PM
// ***********************************************************************
// <copyright file="TranscationInfo.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Moe.Lib;
using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    /// TranscationEx.
    /// </summary>
    public static class TranscationEx
    {
        /// <summary>
        /// To the information.
        /// </summary>
        /// <param name="transcation">The transcation.</param>
        /// <returns>TranscationInfo.</returns>
        public static TranscationInfo ToInfo(this Transcation transcation)
        {
            return new TranscationInfo
            {
                AgreementsInfo = transcation.AgreementsInfo,
                Amount = transcation.Amount,
                BankCardNo = transcation.BankCardNo,
                Cellphone = transcation.Cellphone,
                ChannelCode = transcation.ChannelCode,
                ResultCode = transcation.ResultCode,
                ResultTime = transcation.ResultTime,
                Trade = transcation.Trade,
                TradeCode = transcation.TradeCode,
                TransactionId = transcation.TransactionId,
                TransactionTime = transcation.TransactionTime,
                TransDesc = transcation.TransDesc,
                UserId = transcation.UserId
            };
        }
    }

    /// <summary>
    /// TranscationInfoEx.
    /// </summary>
    public static class TranscationInfoEx
    {
        /// <summary>
        /// To the database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>AccountTranscation.</returns>
        public static AccountTranscation ToDBModel(this TranscationInfo info, Dictionary<string, object> args = null)
        {
            string argsString = JsonHelper.NewDictionary;
            if (args != null)
            {
                argsString = args.ToJson();
            }

            return new AccountTranscation
            {
                AgreementsInfo = info.AgreementsInfo.ToJson(),
                Amount = info.Amount,
                Args = argsString,
                BankCardInfo = JsonHelper.NewDictionary,
                Cellphone = info.Cellphone,
                ChannelCode = info.ChannelCode,
                Info = JsonHelper.NewDictionary,
                ResultCode = info.ResultCode,
                ResultTime = info.ResultTime,
                TradeCode = info.TradeCode,
                TransDesc = info.TransDesc,
                TranscationIdentifier = info.TransactionId.ToGuidString(),
                TranscationTime = info.TransactionTime,
                UserIdentifier = info.UserId.ToGuidString()
            };
        }
    }

    /// <summary>
    /// TranscationInfo.
    /// </summary>
    [Immutable]
    public class TranscationInfo
    {
        /// <summary>
        /// Gets or sets the agreements information.
        /// </summary>
        /// <value>The agreements information.</value>
        public Dictionary<string, object> AgreementsInfo { get; set; }

        /// <summary>
        /// 金额，以分为单位
        /// </summary>
        /// <value>The amount.</value>
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets the bank card no.
        /// </summary>
        /// <value>The bank card no.</value>
        public string BankCardNo { get; set; }

        /// <summary>
        /// Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        /// Gets or sets the channel code.
        /// </summary>
        /// <value>The channel code.</value>
        public int ChannelCode { get; set; }

        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        /// <value>The result code.</value>
        public int ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the result time.
        /// </summary>
        /// <value>The result time.</value>
        public DateTime? ResultTime { get; set; }

        /// <summary>
        /// Gets or sets the trade.
        /// </summary>
        /// <value>The trade.</value>
        public Trade Trade { get; set; }

        /// <summary>
        /// Gets or sets the trade code.
        /// </summary>
        /// <value>The trade code.</value>
        public int TradeCode { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the transaction time.
        /// </summary>
        /// <value>The transaction time.</value>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        public string TransDesc { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}