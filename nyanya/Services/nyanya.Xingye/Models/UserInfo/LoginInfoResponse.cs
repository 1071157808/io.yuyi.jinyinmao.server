// FileInformation: nyanya/nyanya.Xingye/LoginInfoResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   3:29 PM

using System;
using Xingye.Domain.Users.Services.Interfaces;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     用户登录信息
    /// </summary>
    public class LoginInfoResponse
    {
        /// <summary>
        ///     Gets or sets the name of the login.
        /// </summary>
        /// <value>
        ///     The name of the login.
        /// </value>
        public string LoginName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="LoginInfoResponse" /> is valid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if valid; otherwise, <c>false</c>.
        /// </value>
        public bool Valid { get; set; }
    }

    internal static class CurrentUserExtensions
    {
        internal static LoginInfoResponse ToLoginInfoResponse(this CurrentUser user)
        {
            return new LoginInfoResponse
            {
                LoginName = user.Name,
                Valid = user.ExpiryTime > DateTime.Now.AddSeconds(2)
            };
        }
    }
}