// FileInformation: nyanya/Services.WebAPI.Common/CellphoneRequestModel.cs
// CreatedTime: 2014/03/31   7:25 PM
// LastUpdatedTime: 2014/03/31   7:30 PM

using Services.WebAPI.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Services.WebAPI.Common.RequestModels
{
    public class CellphoneRequestModel : IRequestModel
    {
        [CellphoneFormat(ErrorMessage = "手机号格式不正确")]
        [Required(ErrorMessage = "请填写手机号")]
        public string Cellphone { get; set; }
    }
}