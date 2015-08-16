// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  7:09 PM
// ***********************************************************************
// <copyright file="SettleAccountTransactionInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Moe.Lib;
using Orleans.Concurrency;
using PostSharp.Patterns.Model;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     SettleAccountTransactionEx.
    /// </summary>
    public static class SettleAccountTransactionEx
    {
        /// <summary>
        ///     To the information.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>SettleAccountTransactionInfo.</returns>
        public static SettleAccountTransactionInfo ToInfo(this SettleAccountTransaction transaction)
        {
            return new SettleAccountTransactionInfo
            {
                Amount = transaction.Amount,
                Args = transaction.Args,
                BankCardNo = transaction.BankCardNo,
                ChannelCode = transaction.ChannelCode,
                OrderId = transaction.OrderId,
                ResultCode = transaction.ResultCode,
                ResultTime = transaction.ResultTime,
                SequenceNo = transaction.SequenceNo,
                Trade = transaction.Trade,
                TradeCode = transaction.TradeCode,
                TransactionId = transaction.TransactionId,
                TransactionTime = transaction.TransactionTime,
                TransDesc = transaction.TransDesc,
                UserId = transaction.UserId,
                UserInfo = transaction.UserInfo
            };
        }
    }

    /// <summary>
    ///     SettleAccountTransactionInfoEx.
    /// </summary>
    public static class SettleAccountTransactionInfoEx
    {
        /// <summary>
        ///     Maps to database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="transactionModel">The transaction model.</param>
        public static void MapToDBModel(this SettleAccountTransactionInfo info, AccountTransaction transactionModel)
        {
            Dictionary<string, object> i = BuildAccountTransactionModelInfo();

            transactionModel.Amount = info.Amount;
            transactionModel.Args = info.Args.ToJson();
            transactionModel.BankCardNo = info.BankCardNo;
            transactionModel.ChannelCode = info.ChannelCode;
            transactionModel.Info = i.ToJson();
            transactionModel.OrderIdentifier = info.OrderId.ToGuidString();
            transactionModel.ResultCode = info.ResultCode;
            transactionModel.ResultTime = info.ResultTime;
            transactionModel.SequenceNo = info.SequenceNo;
            transactionModel.TradeCode = info.TradeCode;
            transactionModel.TransDesc = info.TransDesc;
            transactionModel.TransactionTime = info.TransactionTime;
            transactionModel.UserIdentifier = info.UserId.ToGuidString();
            transactionModel.UserInfo = info.UserInfo.ToJson();
        }

        /// <summary>
        ///     To the database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Models.AccountTransaction.</returns>
        public static AccountTransaction ToDBModel(this SettleAccountTransactionInfo info)
        {
            Dictionary<string, object> i = BuildAccountTransactionModelInfo();

            return new AccountTransaction
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
                TransactionIdentifier = info.TransactionId.ToGuidString(),
                TransactionTime = info.TransactionTime,
                UserIdentifier = info.UserId.ToGuidString(),
                UserInfo = info.UserInfo.ToJson()
            };
        }

        private static Dictionary<string, object> BuildAccountTransactionModelInfo()
        {
            Dictionary<string, object> i = new Dictionary<string, object>();
            return i;
        }
    }

    /// <summary>
    ///     SettleAccountTransactionInfo.
    /// </summary>
    [Immutable]
    public class SettleAccountTransactionInfo
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
        [Reference]
        public UserInfo UserInfo { get; set; }
    }
}