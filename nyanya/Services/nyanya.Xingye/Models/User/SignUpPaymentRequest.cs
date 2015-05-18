// FileInformation: nyanya/nyanya.Xingye/SignUpPaymentRequest.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   3:29 PM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.RequestModels;
using nyanya.AspDotNet.Common.Validation;
using Xingye.Commands.Users;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     开通移动支付使用的请求
    /// </summary>
    public class SignUpPaymentRequest : IRequestModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        [Required]
        [MinLength(15)]
        [MaxLength(19)]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        [AvailableValues("浦发银行", "深发银行", "平安银行", "民生银行", "工商银行", "农业银行", "建设银行", "招商银行", "广发银行", "广州银行", "邮储银行", "兴业银行", "光大银行", "华夏银行", "中信银行", "广州农商行", "海南农信社", "中国银行")]
        public string BankName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the city.
        /// </summary>
        [Required]
        [RegularExpression(@"^.+\|.+$")]
        [AvailableValues("上海|上海", "广东|广州", "广东|深圳")]
        public string CityName { get; set; }

        /// <summary>
        ///     证件类型。0 => 身份证， 1 => 护照，2 => 台湾， 3=> 军官证
        /// </summary>
        [Required]
        [AvailableValues(Credential.IdCard, Credential.Junguan, Credential.Passport, Credential.Taiwan)]
        public Credential Credential { get; set; }

        /// <summary>
        ///     Gets or sets the credential no.
        /// </summary>
        [Required]
        public string CredentialNo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the real.
        /// </summary>
        [Required]
        public string RealName { get; set; }

        #endregion Public Properties
    }
}
