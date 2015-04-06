// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-03  6:32 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-05  7:09 PM
// ***********************************************************************
// <copyright file="IUserState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Moe.Actor.Model;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IUserState
    /// </summary>
    public interface IUserState : IEntityState
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the credential.
        /// </summary>
        /// <value>The credential.</value>
        Credential Credential { get; set; }

        /// <summary>
        ///     Gets or sets the credential no.
        /// </summary>
        /// <value>The credential no.</value>
        string CredentialNo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the real name.
        /// </summary>
        /// <value>The name of the real name.</value>
        string RealName { get; set; }

        /// <summary>
        ///     Gets or sets the register time.
        /// </summary>
        /// <value>The register time.</value>
        DateTime RegisterTime { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the user information is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        bool Verified { get; set; }

        /// <summary>
        ///     Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        DateTime? VerifiedTime { get; set; }

        /// <summary>
        ///     Gets or sets the jinyinmao account.
        /// </summary>
        /// <value>The jinyinmao account.</value>
        IJinyinmaoAccount JinyinmaoAccount { get; set; }

        /// <summary>
        ///     Gets or sets the source account.
        /// </summary>
        /// <value>The source account.</value>
        ISourceAccount SourceAccount { get; set; }
    }
}
