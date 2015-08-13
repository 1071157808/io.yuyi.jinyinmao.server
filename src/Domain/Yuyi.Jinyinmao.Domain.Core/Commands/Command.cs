// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Command.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-13  23:16
// ***********************************************************************
// <copyright file="Command.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     Command.
    /// </summary>
    public abstract class Command : ICommand
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Command" /> class.
        /// </summary>
        protected Command()
        {
            this.CommandId = Guid.NewGuid();
        }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        #region ICommand Members

        /// <summary>
        ///     Gets or sets the command identifier.
        /// </summary>
        /// <value>The command identifier.</value>
        public Guid CommandId { get; }

        #endregion ICommand Members
    }
}