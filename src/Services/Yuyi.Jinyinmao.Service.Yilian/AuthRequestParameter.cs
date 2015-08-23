// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : AuthRequestParameter.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-21  13:22
// ***********************************************************************
// <copyright file="AuthRequestParameter.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     AuthRequestParameter.
    /// </summary>
    public sealed class AuthRequestParameter
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
            this.BatchNo = bn.ToUpperInvariant();

            TransDetail tran = new TransDetail
            {
                UserUuid = userId.ToUpperInvariant(),
                Sn = sn.ToUpperInvariant(),
                AccNo = accNo,
                // 账号名
                AccName = accName,
                // 省份
                AccProvince = accProvince,
                // 城市名
                AccCity = accCity,
                Amount = "1.08",
                // 银行名
                BankName = bankName,
                // 证件类型
                IDType = YilianRequestParameterHelper.TransformCredentialType(idType),
                // 证件号码
                IDNo = idNo,
                // 手机号
                MobileNo = mobileNo,
                // 回调URL
                // MerchantUrl = "",
                // 模拟产品编号
                MerOrderNo = "A" + userId.ToUpperInvariant()
            };

            this.TransDetails = new List<TransDetail> { tran };
        }

        /// <summary>
        ///     “BATCH_NO 批次号”须保证唯一性
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        [JsonProperty("BATCH_NO")]
        public string BatchNo { get; set; }

        /// <summary>
        ///     交易信息
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
        [SuppressMessage("ReSharper", "MemberCanBeInternal")]
        [JsonProperty("TRANS_DETAILS")]
        public List<TransDetail> TransDetails { get; set; }
    }
}