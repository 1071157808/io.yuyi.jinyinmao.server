﻿// FileInformation: nyanya/nyanya.Xingye/SignInResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   3:29 PM

using Xingye.Domain.Users.Services.DTO;

namespace nyanya.Xingye.Models
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
                UserExist = result.UserExist
            };
        }

        #endregion Internal Methods
    }
}