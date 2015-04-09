// ***********************************************************************
// Assembly         : Yuyi.Jinyinmao.Domain.User.Interface
// Author           : Siqi Lu
// Created          : 2015-04-04  9:48 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-05  6:30 PM
// ***********************************************************************
// <copyright file="UserRegister.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Moe.Actor.Commands;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    /// Class UserRegister.
    /// </summary>
    [Immutable]
    public class UserRegister : Command
    {
        /// <summary>
        /// Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        /// Gets or sets the credential.
        /// </summary>
        /// <value>The credential.</value>
        public Credential Credential { get; set; }

        /// <summary>
        /// Gets or sets the credential no.
        /// </summary>
        /// <value>The credential no.</value>
        public string CredentialNo { get; set; }

        /// <summary>
        ///     Gets the handler identifier.
        /// </summary>
        /// <value>The handler identifier.</value>
        public override Guid HandlerId
        {
            get { return this.UserId; }
        }

        /// <summary>
        /// Gets or sets the name of the real name.
        /// </summary>
        /// <value>The name of the real name.</value>
        public string RealName { get; set; }

        /// <summary>
        /// Gets or sets the register time.
        /// </summary>
        /// <value>The register time.</value>
        public DateTime? RegisterTime { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user information is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified { get; set; }

        /// <summary>
        /// Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        public DateTime? VerifiedTime { get; set; }
    }
}
