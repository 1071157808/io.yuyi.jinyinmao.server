// FileInformation: nyanya/nyanya.AspDotNet.Common/CellphoneRequestModel.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:18 AM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.AspDotNet.Common.RequestModels
{
    public class CellphoneRequestModel : IRequestModel
    {
        [CellphoneFormat(ErrorMessage = "手机号格式不正确")]
        [Required(ErrorMessage = "请填写手机号")]
        public string Cellphone { get; set; }
    }
}