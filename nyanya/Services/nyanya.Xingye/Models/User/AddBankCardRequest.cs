// FileInformation: nyanya/nyanya.Xingye/AddBankCardRequest.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/01   11:33 AM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.RequestModels;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     添加银行卡请求
    /// </summary>
    public class AddBankCardRequest : IRequestModel
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

        #endregion Public Properties
    }
}
