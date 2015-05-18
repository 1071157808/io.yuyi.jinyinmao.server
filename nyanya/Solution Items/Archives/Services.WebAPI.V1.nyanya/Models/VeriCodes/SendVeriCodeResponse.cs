// FileInformation: nyanya/Services.WebAPI.V1.nyanya/SendVeriCodeResponse.cs
// CreatedTime: 2014/07/09   1:25 AM
// LastUpdatedTime: 2014/07/09   9:30 AM

using Cqrs.Domain.Auth.Services.DTO;

namespace Services.WebAPI.V1.nyanya.Models.VeriCodes
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