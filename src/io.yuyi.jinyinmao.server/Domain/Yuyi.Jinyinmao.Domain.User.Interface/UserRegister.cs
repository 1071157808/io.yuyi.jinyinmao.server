// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-01  9:51 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-04  5:49 PM
// ***********************************************************************
// <copyright file="UserRegister.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using Moe.Lib;
using Moe.Orleans.Commands;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class UserRegister.
    /// </summary>
    [Immutable]
    public class UserRegister
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserRegister" /> class.
        /// </summary>
        public UserRegister()
        {
            this.HandlerId = GuidUtility.NewSequentialGuid();
            this.HandlerType = typeof(IUser);
            this.HanderResult = null;
        }

        /// <summary>
        ///     Gets the hander result.
        /// </summary>
        /// <value>The hander result.</value>
        public ICommandHanderResult HanderResult { get; set; }

        /// <summary>
        ///     Gets the handler identifier.
        /// </summary>
        /// <value>The handler identifier.</value>
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
        public Guid HandlerId { get; private set; }

        /// <summary>
        ///     Gets the type of the handler.
        /// </summary>
        /// <value>The type of the handler.</value>
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
        public Type HandlerType { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether this instance has result.
        /// </summary>
        /// <value><c>true</c> if this instance has result; otherwise, <c>false</c>.</value>
        public bool HasResult
        {
            get { return this.HanderResult != null; }
        }
    }
}
