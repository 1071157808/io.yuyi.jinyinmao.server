// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  4:01 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-15  6:05 PM
// ***********************************************************************
// <copyright file="Authenticate.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     Authenticate.
    /// </summary>
    [Immutable]
    public class Authenticate : Command
    {
        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        /// <value>The bank card no.</value>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        /// <value>The name of the bank.</value>
        public string BankName { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the name of the city.
        /// </summary>
        /// <value>The name of the city.</value>
        public string CityName { get; set; }

        /// <summary>
        ///     Gets or sets the credential.
        /// </summary>
        /// <value>The credential.</value>
        public Credential Credential { get; set; }

        /// <summary>
        ///     Gets or sets the credential no.
        /// </summary>
        /// <value>The credential no.</value>
        public string CredentialNo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the real.
        /// </summary>
        /// <value>The name of the real.</value>
        public string RealName { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}