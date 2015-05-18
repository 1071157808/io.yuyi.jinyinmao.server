// FileInformation: nyanya/nyanya.Meow/LoginInfoResponse.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:28 PM

using System;
using Cat.Domain.Users.Services.Interfaces;

namespace nyanya.Meow.Models
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