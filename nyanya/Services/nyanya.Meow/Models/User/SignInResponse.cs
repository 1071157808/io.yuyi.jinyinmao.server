// FileInformation: nyanya/nyanya.Meow/SignInResponse.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:28 PM

using Cat.Domain.Users.Services.DTO;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     登陆结果
    /// </summary>
    public class SignInResponse
    {
        /// <summary>
        ///     账户是否被锁定
        /// </summary>
        public bool Lock
        {
            get { return this.RemainCount < 1 && this.UserExist; }
        }

        /// <summary>
        ///     剩余登陆次数
        /// </summary>
        public int RemainCount { get; set; }

        /// <summary>
        ///     登陆结果
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        ///     用户是否存在
        /// </summary>
        public bool UserExist { get; set; }
    }

    internal static class SignInResponseModelExtensions
    {
        internal static SignInResponse ToSignInResponseModel(this SignInResult result)
        {
            return new SignInResponse
            {
                RemainCount = result.RemainCount,
                Successful = result.Successful,
                UserExist = result.UserExist
            };
        }
    }
}