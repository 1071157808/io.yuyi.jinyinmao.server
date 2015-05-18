using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.DataAnnotations;

namespace nyanya.Xingye.Models
{
    /// <summary>
    /// 验证支付信息请求
    /// </summary>
    public class VerifyPaymentInfoRequest
    {
        #region Public Properties

        /// <summary>
        ///     默认银行卡号
        /// </summary>
        [Required]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     用户证件号
        /// </summary>
        [Required]
        public string CredentialNo { get; set; }
   

        #endregion Public Properties
    }
}