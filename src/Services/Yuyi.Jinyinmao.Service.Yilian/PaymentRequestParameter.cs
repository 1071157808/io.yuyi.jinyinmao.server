// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  6:27 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  6:58 PM
// ***********************************************************************
// <copyright file="PaymentRequestParameter.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

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
            this.BATCH_NO = bn.ToUpper();

            TransDetail tran = new TransDetail
            {
                USER_UUID = userId.ToUpper(),
                SN = sn,
                ACC_NO = accNo,
                // 账号名
                ACC_NAME = accName,
                // 省份
                ACC_PROVINCE = accProvince,
                // 城市名
                ACC_CITY = accCity,
                AMOUNT = decimal.Round(amount, 2, MidpointRounding.AwayFromZero).ToString(),
                // 银行名
                BANK_NAME = bankName,
                // 证件类型
                ID_TYPE = YilianRequestParameterHelper.TransformCredentialType(idType),
                // 证件号码
                ID_NO = idNo,
                // 手机号
                MOBILE_NO = mobileNo,
                // 回调URL
                MERCHANT_URL = "",
                // 产品编号
                MER_ORDER_NO = pn
            };

            this.TRANS_DETAILS = new List<TransDetail>();
            this.TRANS_DETAILS.Add(tran);
        }

        /// <summary>
        ///     “BATCH_NO 批次号”须保证唯一性
        /// </summary>
        public string BATCH_NO { get; set; }

        /// <summary>
        ///     Gets or sets the tran s_ details.
        /// </summary>
        /// <value>The tran s_ details.</value>
        public List<TransDetail> TRANS_DETAILS { get; set; }
    }
}
