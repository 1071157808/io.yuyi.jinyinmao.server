// FileInformation: nyanya/nyanya.Cat/VerifyVeriCodeResponse.cs
// CreatedTime: 2014/07/09   9:21 AM
// LastUpdatedTime: 2014/07/09   9:23 AM

using Cat.Domain.Auth.Services.DTO;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     验证码验证结果
    /// </summary>
    public class VerifyVeriCodeResponse
    {
        /// <summary>
        ///     剩余验证次数，若为-1，则该验证码已失效
        /// </summary>
        public int RemainCount { get; set; }

        /// <summary>
        ///     本次验证结果
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        ///     验证码口令，如果验证码验证失败，则为空
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