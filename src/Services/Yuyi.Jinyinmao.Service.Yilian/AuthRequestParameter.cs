// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  11:11 PM
// ***********************************************************************
// <copyright file="AuthRequestParameter.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     AuthRequestParameter.
    /// </summary>
    public class AuthRequestParameter
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthRequestParameter" /> class.
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
        public AuthRequestParameter(string bn, string sn, string accNo, string accName, string accProvince, string accCity, string bankName,
            int idType, string idNo, string mobileNo, string userId)
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
                AMOUNT = "1.08",
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
                // 模拟产品编号
                MER_ORDER_NO = "A" + userId.ToUpper()
            };

            this.TRANS_DETAILS = new List<TransDetail>();
            this.TRANS_DETAILS.Add(tran);
        }

        /// <summary>
        ///     “BATCH_NO 批次号”须保证唯一性
        /// </summary>
        public string BATCH_NO { get; set; }

        /// <summary>
        ///     交易信息
        /// </summary>
        public List<TransDetail> TRANS_DETAILS { get; set; }
    }
}
