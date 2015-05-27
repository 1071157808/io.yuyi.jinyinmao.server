// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  7:20 PM
// ***********************************************************************
// <copyright file="PaymentRequestParameter.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     PaymentRequestParameter.
    /// </summary>
    public class PaymentRequestParameter
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PaymentRequestParameter" /> class.
        /// </summary>
        /// <param name="bn">The bn.</param>
        /// <param name="sn">The sn.</param>
        /// <param name="accNo">The acc no.</param>
        /// <param name="accName">Name of the acc.</param>
        /// <param name="accProvince">The acc province.</param>
        /// <param name="accCity">The acc city.</param>
        /// <param name="bankName">Name of the bank.</param>
        /// <param name="idType">Type of the identifier.</param>
        /// <param name="idNo">The identifier no.</param>
        /// <param name="mobileNo">The mobile no.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pn">The pn.</param>
        /// <param name="amount">The amount.</param>
        public PaymentRequestParameter(string bn, string sn, string accNo, string accName, string accProvince, string accCity, string bankName,
            int idType, string idNo, string mobileNo, string userId, string pn, decimal amount)
        {
            this.BatchNo = bn.ToUpper();

            TransDetail tran = new TransDetail
            {
                UserUuid = userId.ToUpper(),
                Sn = sn,
                AccNo = accNo,
                // 账号名
                AccName = accName,
                // 省份
                AccProvince = accProvince,
                // 城市名
                AccCity = accCity,
                Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture),
                // 银行名
                BankName = bankName,
                // 证件类型
                IDType = YilianRequestParameterHelper.TransformCredentialType(idType),
                // 证件号码
                IDNo = idNo,
                // 手机号
                MobileNo = mobileNo,
                // 回调URL
                MerchantUrl = "",
                // 产品编号
                MerOrderNo = pn
            };

            this.TransDetails = new List<TransDetail> { tran };
        }

        /// <summary>
        ///     “BATCH_NO 批次号”须保证唯一性
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global"), SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string BatchNo { get; set; }

        /// <summary>
        ///     Gets or sets the tran s_ details.
        /// </summary>
        /// <value>The tran s_ details.</value>
        [SuppressMessage("ReSharper", "MemberCanBeInternal"), SuppressMessage("ReSharper", "MemberCanBePrivate.Global"), SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
        public List<TransDetail> TransDetails { get; set; }
    }
}