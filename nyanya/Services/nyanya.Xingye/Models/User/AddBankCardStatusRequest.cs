// FileInformation: nyanya/nyanya.Xingye/AddBankCardStatusRequest.cs
// CreatedTime: 2014/09/05   14:51 AM
// LastUpdatedTime: 2014/09/05   14:51 AM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.RequestModels;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     添加银行卡状态请求
    /// </summary>
    public class AddBankCardStatusRequest : IRequestModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        [Required]
        [MinLength(15)]
        [MaxLength(19)]
        public string BankCardNo { get; set; }

        #endregion Public Properties
    }
}