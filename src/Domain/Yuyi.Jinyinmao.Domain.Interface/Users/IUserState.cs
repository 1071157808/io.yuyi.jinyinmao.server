// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:25 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-23  9:50 PM
// ***********************************************************************
// <copyright file="IUserState.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IUserState
    /// </summary>
    public interface IUserState : IEntityState
    {
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     Gets or sets the bank cards.
        /// </summary>
        /// <value>The bank cards.</value>
        Dictionary<string, BankCard> BankCards { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        string Cellphone { get; set; }

        /// <summary>
        ///     900 => PC, 901 => iPhone, 902 => Android, 903 => M
        /// </summary>
        /// <value>The type of the client.</value>
        long ClientType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="IUserState" /> is closed.
        /// </summary>
        /// <value><c>true</c> if closed; otherwise, <c>false</c>.</value>
        bool Closed { get; set; }

        /// <summary>
        ///     Gets or sets the contract identifier.
        /// </summary>
        /// <value>The contract identifier.</value>
        long ContractId { get; set; }

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
        ///     Gets or sets the encrypted password.
        /// </summary>
        /// <value>The encrypted password.</value>
        string EncryptedPassword { get; set; }

        /// <summary>
        ///     Gets or sets the encrypted payment password.
        /// </summary>
        /// <value>The encrypted payment password.</value>
        string EncryptedPaymentPassword { get; set; }

        /// <summary>
        ///     Gets or sets the invite by.
        /// </summary>
        /// <value>The invite by.</value>
        string InviteBy { get; set; }

        /// <summary>
        ///     Gets or sets the jby account.
        /// </summary>
        /// <value>The jby account.</value>
        Dictionary<Guid, JBYAccountTranscation> JBYAccount { get; set; }

        /// <summary>
        ///     Gets or sets the login names.
        /// </summary>
        /// <value>The login names.</value>
        List<string> LoginNames { get; set; }

        /// <summary>
        ///     Gets or sets the orders.
        /// </summary>
        /// <value>The orders.</value>
        Dictionary<Guid, Order> Orders { get; set; }

        /// <summary>
        ///     Gets or sets the outlet code.
        /// </summary>
        /// <value>The outlet code.</value>
        string OutletCode { get; set; }

        /// <summary>
        ///     Gets or sets the payment salt.
        /// </summary>
        /// <value>The payment salt.</value>
        string PaymentSalt { get; set; }

        /// <summary>
        ///     Gets or sets the name of the real.
        /// </summary>
        /// <value>The name of the real.</value>
        string RealName { get; set; }

        /// <summary>
        ///     Gets or sets the register time.
        /// </summary>
        /// <value>The register time.</value>
        DateTime RegisterTime { get; set; }

        /// <summary>
        ///     Gets or sets the salt.
        /// </summary>
        /// <value>The salt.</value>
        string Salt { get; set; }

        /// <summary>
        ///     Gets or sets the settle account.
        /// </summary>
        /// <value>The settle account.</value>
        Dictionary<Guid, SettleAccountTranscation> SettleAccount { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="IUserState" /> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        bool Verified { get; set; }

        /// <summary>
        ///     Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        DateTime? VerifiedTime { get; set; }
    }
}