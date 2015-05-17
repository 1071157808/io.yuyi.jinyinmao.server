// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-15  7:43 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-17  8:20 PM
// ***********************************************************************
// <copyright file="SettleAccountTranscationInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Moe.Lib;
using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     SettleAccountTranscationEx.
    /// </summary>
    public static class SettleAccountTranscationEx
    {
        /// <summary>
        ///     To the information.
        /// </summary>
        /// <param name="transcation">The transcation.</param>
        /// <param name="bankCardInfo">The bank card information.</param>
        /// <returns>SettleAccountTranscationInfo.</returns>
        public static SettleAccountTranscationInfo ToInfo(this SettleAccountTranscation transcation, BankCardInfo bankCardInfo)
        {
            return new SettleAccountTranscationInfo
            {
                Amount = transcation.Amount,
                Args = transcation.Args,
                BankCardInfo = bankCardInfo,
                BankCardNo = transcation.BankCardNo,
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
    ///     SettleAccountTranscationInfoEx.
    /// </summary>
    public static class SettleAccountTranscationInfoEx
    {
        /// <summary>
        ///     Maps to database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="transcationModel">The transcation model.</param>
        public static void MapToDBModel(this SettleAccountTranscationInfo info, AccountTranscation transcationModel)
        {
            Dictionary<string, object> i = BuildAccountTranscationModelInfo();

            transcationModel.Amount = info.Amount;
            transcationModel.Args = info.Args.ToJson();
            transcationModel.BankCardInfo = info.BankCardInfo.ToJson();
            transcationModel.Cellphone = info.BankCardInfo.Cellphone;
            transcationModel.ChannelCode = info.ChannelCode;
            transcationModel.Info = i.ToJson();
            transcationModel.ResultCode = info.ResultCode;
            transcationModel.ResultTime = info.ResultTime;
            transcationModel.TradeCode = info.TradeCode;
            transcationModel.TransDesc = info.TransDesc;
            transcationModel.TranscationTime = info.TransactionTime;
            transcationModel.UserIdentifier = info.UserId.ToGuidString();
        }

        /// <summary>
        ///     To the database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Models.AccountTranscation.</returns>
        public static AccountTranscation ToDBModel(this SettleAccountTranscationInfo info)
        {
            Dictionary<string, object> i = BuildAccountTranscationModelInfo();

            return new AccountTranscation
            {
                Amount = info.Amount,
                Args = info.Args.ToJson(),
                BankCardInfo = info.BankCardInfo.ToJson(),
                Cellphone = info.BankCardInfo.Cellphone,
                ChannelCode = info.ChannelCode,
                Info = i.ToJson(),
                ResultCode = info.ResultCode,
                ResultTime = info.ResultTime,
                TradeCode = info.TradeCode,
                TransDesc = info.TransDesc,
                TranscationIdentifier = info.TransactionId.ToGuidString(),
                TranscationTime = info.TransactionTime,
                UserIdentifier = info.UserId.ToGuidString()
            };
        }

        private static Dictionary<string, object> BuildAccountTranscationModelInfo()
        {
            Dictionary<string, object> i = new Dictionary<string, object>();
            return i;
        }
    }

    /// <summary>
    ///     SettleAccountTranscationInfo.
    /// </summary>
    [Immutable]
    public class SettleAccountTranscationInfo
    {
        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public int Amount { get; set; }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     Gets or sets the bank card information.
        /// </summary>
        /// <value>The bank card information.</value>
        public BankCardInfo BankCardInfo { get; set; }

        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        /// <value>The bank card no.</value>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the channel code.
        /// </summary>
        /// <value>The channel code.</value>
        public int ChannelCode { get; set; }

        /// <summary>
        ///     Gets or sets the result code.
        /// </summary>
        /// <value>The result code.</value>
        public int ResultCode { get; set; }

        /// <summary>
        ///     Gets or sets the result time.
        /// </summary>
        /// <value>The result time.</value>
        public DateTime? ResultTime { get; set; }

        /// <summary>
        ///     Gets or sets the trade.
        /// </summary>
        /// <value>The trade.</value>
        public Trade Trade { get; set; }

        /// <summary>
        ///     Gets or sets the trade code.
        /// </summary>
        /// <value>The trade code.</value>
        public int TradeCode { get; set; }

        /// <summary>
        ///     Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        public Guid TransactionId { get; set; }

        /// <summary>
        ///     Gets or sets the transaction time.
        /// </summary>
        /// <value>The transaction time.</value>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        ///     Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        public string TransDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}