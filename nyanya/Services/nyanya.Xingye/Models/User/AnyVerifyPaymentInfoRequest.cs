using ServiceStack.DataAnnotations;

namespace nyanya.Xingye.Models
{
    /// <summary>
    /// 验证支付信息请求
    /// </summary>
    public class AnyVerifyPaymentInfoRequest
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

        /// <summary>
        ///     手机号
        /// </summary>
        [Required]
        public string CellphoneNo { get; set; }


        #endregion Public Properties
    }
}