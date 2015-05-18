// FileInformation: nyanya/nyanya.Xingye/SendVeriCodeResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   3:28 PM

using Xingye.Domain.Auth.Services.DTO;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     校验码发送结果
    /// </summary>
    public class SendVeriCodeResponse
    {
        /// <summary>
        ///     今天剩余发送次数，若为-1，则今天不能再次发送该类型校验码
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
