// FileInformation: nyanya/nyanya.Meow/SendVeriCodeResponse.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:26 PM

using Cat.Domain.Auth.Services.DTO;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     验证码发送结果
    /// </summary>
    public class SendVeriCodeResponse
    {
        /// <summary>
        ///     今天剩余发送次数，若为-1，则今天不能再次发送该类型验证码
        /// </summary>
        public int RemainCount { get; set; }

        /// <summary>
        ///     本次发送结果
        /// </summary>
        public bool Successful { get; set; }
    }

    internal static class SendVeriCodeResultExtensions
    {
        internal static SendVeriCodeResponse ToSendVeriCodeResponse(this SendVeriCodeResult result)
        {
            return new SendVeriCodeResponse
            {
                RemainCount = result.RemainCount,
                Successful = result.Successful
            };
        }
    }
}