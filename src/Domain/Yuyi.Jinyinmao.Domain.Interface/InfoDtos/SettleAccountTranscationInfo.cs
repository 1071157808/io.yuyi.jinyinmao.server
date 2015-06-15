// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  7:09 PM
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
        /// <returns>SettleAccountTranscationInfo.</returns>
        public static SettleAccountTranscationInfo ToInfo(this SettleAccountTranscation transcation)
        {
            return new SettleAccountTranscationInfo
            {
                Amount = transcation.Amount,
                Args = transcation.Args,
                BankCardNo = transcation.BankCardNo,
                ChannelCode = transcation.ChannelCode,
                OrderId = transcation.OrderId,
                ResultCode = transcation.ResultCode,
                ResultTime = transcation.ResultTime,
                SequenceNo = transcation.SequenceNo,
                Trade = transcation.Trade,
                TradeCode = transcation.TradeCode,
                TransactionId = transcation.TransactionId,
                TransactionTime = transcation.TransactionTime,
                TransDesc = transcation.TransDesc,
                UserId = transcation.UserId,
                UserInfo = transcation.UserInfo
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
            transcationModel.BankCardNo = info.BankCardNo;
            transcationModel.ChannelCode = info.ChannelCode;
            transcationModel.Info = i.ToJson();
            transcationModel.OrderIdentifier = info.OrderId.ToGuidString();
            transcationModel.ResultCode = info.ResultCode;
            transcationModel.ResultTime = info.ResultTime;
            transcationModel.SequenceNo = info.SequenceNo;
            transcationModel.TradeCode = info.TradeCode;
            transcationModel.TransDesc = info.TransDesc;
            transcationModel.TranscationTime = info.TransactionTime;
            transcationModel.UserIdentifier = info.UserId.ToGuidString();
            transcationModel.UserInfo = info.UserInfo.ToJson();
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
                BankCardNo = info.BankCardNo,
                ChannelCode = info.ChannelCode,
                Info = i.ToJson(),
                OrderIdentifier = info.OrderId.ToGuidString(),
                ResultCode = info.ResultCode,
                ResultTime = info.ResultTime,
                SequenceNo = info.SequenceNo,
                TradeCode = info.TradeCode,
                TransDesc = info.TransDesc,
                TranscationIdentifier = info.TransactionId.ToGuidString(),
                TranscationTime = info.TransactionTime,
                UserIdentifier = info.UserId.ToGuidString(),
                UserInfo = info.UserInfo.ToJson()
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
        public long Amount { get; set; }

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
        ///     Gets or sets the channel code.
        /// </summary>
        /// <value>The channel code.</value>
        public int ChannelCode { get; set; }

        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        public Guid OrderId { get; set; }

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
        ///     Gets or sets the sequence no.
        /// </summary>
        /// <value>The sequence no.</value>
        public string SequenceNo { get; set; }

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

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}