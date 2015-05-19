// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-17  7:18 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  2:15 AM
// ***********************************************************************
// <copyright file="JBYAccountTranscationInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     JBYAccountTranscationEx.
    /// </summary>
    public static class JBYAccountTranscationEx
    {
        /// <summary>
        ///     To the information.
        /// </summary>
        /// <param name="transcation">The transcation.</param>
        /// <returns>JBYAccountTranscationInfo.</returns>
        public static JBYAccountTranscationInfo ToInfo(this JBYAccountTranscation transcation)
        {
            return new JBYAccountTranscationInfo
            {
                Amount = transcation.Amount,
                Args = transcation.Args,
                PredeterminedResultDate = transcation.PredeterminedResultDate,
                ProductId = transcation.ProductId,
                ResultCode = transcation.ResultCode,
                ResultTime = transcation.ResultTime,
                SettleAccountTranscationId = transcation.SettleAccountTranscationId,
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
    /// </summary>
    public static class JBYAccountTranscationInfoEx
    {
        /// <summary>
        ///     Maps to database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="transcationModel">The transcation model.</param>
        public static void MapToDBModel(this JBYAccountTranscationInfo info, JBYTranscation transcationModel)
        {
            Dictionary<string, object> i = BuidJBYTranscationModelInfo(info);

            transcationModel.AccountTranscationIdentifier = info.SettleAccountTranscationId.ToGuidString();
            transcationModel.Amount = info.Amount;
            transcationModel.Args = info.Args.ToJson();
            transcationModel.Info = i.ToJson();
            transcationModel.ResultCode = info.ResultCode;
            transcationModel.ResultTime = info.ResultTime;
            transcationModel.TradeCode = info.TradeCode;
            transcationModel.TranscationTime = info.TransactionTime;
            transcationModel.TransDesc = info.TransDesc;
            transcationModel.UserIdentifier = info.UserId.ToGuidString();
        }

        /// <summary>
        ///     To the database model.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>JBYTranscation.</returns>
        public static JBYTranscation ToDBModel(this JBYAccountTranscationInfo info)
        {
            Dictionary<string, object> i = BuidJBYTranscationModelInfo(info);

            return new JBYTranscation
            {
                AccountTranscationIdentifier = info.SettleAccountTranscationId.ToGuidString(),
                Amount = info.Amount,
                Args = info.Args.ToJson(),
                Info = i.ToJson(),
                ResultCode = info.ResultCode,
                ResultTime = info.ResultTime,
                TradeCode = info.TradeCode,
                TranscationIdentifier = info.TransactionId.ToGuidString(),
                TranscationTime = info.TransactionTime,
                TransDesc = info.TransDesc,
                UserIdentifier = info.UserId.ToGuidString()
            };
        }

        private static Dictionary<string, object> BuidJBYTranscationModelInfo(JBYAccountTranscationInfo info)
        {
            Dictionary<string, object> i = new Dictionary<string, object>
            {
                { "PredeterminedResultDate", info.PredeterminedResultDate }
            };
            return i;
        }
    }

    /// <summary>
    ///     JBYAccountTranscationInfo.
    /// </summary>
    public class JBYAccountTranscationInfo
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
        /// Gets or sets the predetermined result date.
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
        ///     Gets or sets the settle account transcation identifier.
        /// </summary>
        /// <value>The settle account transcation identifier.</value>
        public Guid SettleAccountTranscationId { get; set; }

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