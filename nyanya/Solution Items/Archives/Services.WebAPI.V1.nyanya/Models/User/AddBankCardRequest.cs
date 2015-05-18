// FileInformation: nyanya/Services.WebAPI.V1.nyanya/AddBankCardRequest.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/26   6:13 PM

using System.ComponentModel.DataAnnotations;
using Services.WebAPI.Common.RequestModels;
using Services.WebAPI.Common.Validation;

namespace Services.WebAPI.V1.nyanya.Models
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
        [AvailableValues("浦发银行", "深发银行", "平安银行", "民生银行", "工商银行", "农业银行", "建设银行", "招商银行", "广发银行", "广州银行", "邮储银行", "兴业银行", "光大银行", "华夏银行", "中信银行", "广州农商行", "海南农信社")]
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