// FileInformation: nyanya/nyanya.Cat/SignInResponse.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   10:52 AM

using Cat.Domain.Users.Services.DTO;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     登陆结果
    /// </summary>
    public class SignInResponse
    {
        #region Public Properties

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

        /// <summary>
        ///     兴业加密Code(用于联合登录)
        /// </summary>
        public string RCode { get; set; }

        #endregion Public Properties
    }

    internal static class SignInResponseModelExtensions
    {
        #region Internal Methods

        internal static SignInResponse ToSignInResponseModel(this SignInResult result)
        {
            return new SignInResponse
            {
                RemainCount = result.RemainCount,
                Successful = result.Successful,
                UserExist = result.UserExist,
                RCode = result.RCode
            };
        }

        #endregion Internal Methods
    }
}