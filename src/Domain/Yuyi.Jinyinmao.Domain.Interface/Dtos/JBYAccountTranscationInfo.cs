// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  6:46 PM
// ***********************************************************************
// <copyright file="JBYAccountTransactionInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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
    ///     JBYAccountTransactionEx.
    /// </summary>
    public static class JBYAccountTransactionEx
    {
        /// <summary>
        ///     To the information.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>JBYAccountTransactionInfo.</returns>
        public static JBYAccountTransactionInfo ToInfo(this JBYAccountTransaction transaction)
        {
            return new JBYAccountTransactionInfo
            {
                Amount = transaction.Amount,
                Args = transaction.Args,
                PredeterminedResultDate = transaction.PredeterminedResultDate,
                ProductId = transaction.ProductId,
                ResultCode = transaction.ResultCode,
                ResultTime = transaction.ResultTime,
                SettleAccountTransactionId = transaction.SettleAccountTransactionId,
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
    /// </summary>
    public static class JBYAccountTransactionInfoEx
    {
        /// <summary>
        ///     Maps to database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="transactionModel">The transaction model.</param>
        public static void MapToDBModel(this JBYAccountTransactionInfo info, JBYTransaction transactionModel)
        {
            Dictionary<string, object> i = BuidJBYTransactionModelInfo(info);

            transactionModel.AccountTransactionIdentifier = info.SettleAccountTransactionId.ToGuidString();
            transactionModel.Amount = info.Amount;
            transactionModel.Args = info.Args.ToJson();
            transactionModel.Info = i.ToJson();
            transactionModel.JBYProductIdentifier = info.ProductId.ToGuidString();
            transactionModel.ResultCode = info.ResultCode;
            transactionModel.ResultTime = info.ResultTime;
            transactionModel.TradeCode = info.TradeCode;
            transactionModel.TransactionTime = info.TransactionTime;
            transactionModel.TransDesc = info.TransDesc;
            transactionModel.UserIdentifier = info.UserId.ToGuidString();
            transactionModel.UserInfo = info.UserInfo.ToJson();
        }

        /// <summary>
        ///     To the database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>JBYTransaction.</returns>
        public static JBYTransaction ToDBModel(this JBYAccountTransactionInfo info)
        {
            Dictionary<string, object> i = BuidJBYTransactionModelInfo(info);

            return new JBYTransaction
            {
                AccountTransactionIdentifier = info.SettleAccountTransactionId.ToGuidString(),
                Amount = info.Amount,
                Args = info.Args.ToJson(),
                Info = i.ToJson(),
                JBYProductIdentifier = info.ProductId.ToGuidString(),
                ResultCode = info.ResultCode,
                ResultTime = info.ResultTime,
                TradeCode = info.TradeCode,
                TransactionIdentifier = info.TransactionId.ToGuidString(),
                TransactionTime = info.TransactionTime,
                TransDesc = info.TransDesc,
                UserIdentifier = info.UserId.ToGuidString(),
                UserInfo = info.UserInfo.ToJson()
            };
        }

        private static Dictionary<string, object> BuidJBYTransactionModelInfo(JBYAccountTransactionInfo info)
        {
            Dictionary<string, object> i = new Dictionary<string, object>
            {
                { "PredeterminedResultDate", info.PredeterminedResultDate }
            };
            return i;
        }
    }

    /// <summary>
    ///     JBYAccountTransactionInfo.
    /// </summary>
    [Immutable]
    public class JBYAccountTransactionInfo
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
        ///     Gets or sets the predetermined result date.
        /// </summary>
        /// <value>The predetermined result date.</value>
        public DateTime? PredeterminedResultDate { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public Guid ProductId { get; set; }

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
        ///     Gets or sets the settle account transaction identifier.
        /// </summary>
        /// <value>The settle account transaction identifier.</value>
        public Guid SettleAccountTransactionId { get; set; }

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