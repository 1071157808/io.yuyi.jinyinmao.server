﻿// FileInformation: nyanya/nyanya.Meow/YSBUserInfoResponse.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:28 PM

using Cat.Domain.Users.Models;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     银生宝用户数据
    /// </summary>
    public class YSBUserInfoResponse
    {
        /// <summary>
        ///     Gets or sets the identifier card.
        /// </summary>
        /// <value>
        ///     The identifier card.
        /// </value>
        public string IdCard { get; set; }

        /// <summary>
        ///     Gets or sets the name of the real.
        /// </summary>
        /// <value>
        ///     The name of the real.
        /// </value>
        public string RealName { get; set; }

        /// <summary>
        ///     是否有银生宝数据
        /// </summary>
        public bool Verified { get; set; }
    }

    internal static class YSBUserInfoExtensions
    {
        internal static YSBUserInfoResponse ToYSBUserInfoResponse(this YSBUserInfo info)
        {
            if (info == null)
            {
                return new YSBUserInfoResponse
                {
                    Verified = false
                };
            }

            return new YSBUserInfoResponse
            {
                IdCard = info.IdCard,
                RealName = info.RealName,
                Verified = true
            };
        }
    }
}