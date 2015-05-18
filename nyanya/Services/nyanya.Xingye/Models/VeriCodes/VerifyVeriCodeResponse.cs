// FileInformation: nyanya/nyanya.Xingye/VerifyVeriCodeResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   3:28 PM

using Xingye.Domain.Auth.Services.DTO;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     校验码验证结果
    /// </summary>
    public class VerifyVeriCodeResponse
    {
        /// <summary>
        ///     剩余验证次数，若为-1，则该校验码已失效
        /// </summary>
        public int RemainCount { get; set; }

        /// <summary>
        ///     本次验证结果
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        ///     校验码口令，如果校验码验证失败，则为空
        /// </summary>
        public string Token { get; set; }
    }

    internal static class VerifyVeriCodeResultExtensions
    {
        internal static VerifyVeriCodeResponse ToVerifyVeriCodeResponse(this VerifyVeriCodeResult result)
        {
            return new VerifyVeriCodeResponse
            {
                RemainCount = result.RemainCount,
                Successful = result.Successful,
                Token = result.Token
            };
        }
    }
}
