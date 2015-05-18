// FileInformation: nyanya/Services.WebAPI.V1.nyanya/LoginInfoResponse.cs
// CreatedTime: 2014/07/20   1:36 PM
// LastUpdatedTime: 2014/07/20   11:17 PM

using System;
using Cqrs.Domain.Users.Services.Interfaces;

namespace Services.WebAPI.V1.nyanya.Models
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public class LoginInfoResponse
    {
        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>
        /// The name of the login.
        /// </value>
        public string LoginName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LoginInfoResponse"/> is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if valid; otherwise, <c>false</c>.
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