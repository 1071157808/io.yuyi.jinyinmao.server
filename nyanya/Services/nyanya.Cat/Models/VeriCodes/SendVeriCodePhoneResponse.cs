using Cat.Domain.Auth.Services.DTO;
using System.Drawing;

namespace nyanya.Cat.Models
{
    public class SendVeriCodePhoneResponse
    {
        /// <summary>
        ///     验证码图片
        /// </summary>
        public byte[] VerifyImage { get; set; }
    }
}