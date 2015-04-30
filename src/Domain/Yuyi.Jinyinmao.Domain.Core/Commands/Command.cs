// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  12:08 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  4:06 PM
// ***********************************************************************
// <copyright file="Command.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

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
        public string Args { get; set; }

        #region ICommand Members

        /// <summary>
        ///     Gets or sets the command identifier.
        /// </summary>
        /// <value>The command identifier.</value>
        public Guid CommandId { get; set; }

        #endregion ICommand Members
    }
}
