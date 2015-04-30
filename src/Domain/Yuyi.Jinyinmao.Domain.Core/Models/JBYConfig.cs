// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  5:29 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-29  5:40 PM
// ***********************************************************************
// <copyright file="JBYConfig.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Domain.Models
{
    /// <summary>
    ///     JBYConfig.
    /// </summary>
    public class JBYConfig
    {
        /// <summary>
        ///     Gets or sets the date number.
        /// </summary>
        /// <value>The date number.</value>
        public int DateNumber { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the jby withdrawal limit.
        /// </summary>
        /// <value>The jby withdrawal limit.</value>
        public int JBYWithdrawalLimit { get; set; }

        /// <summary>
        ///     Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }

        /// <summary>
        ///     Gets or sets the yield.
        /// </summary>
        /// <value>The yield.</value>
        public int Yield { get; set; }
    }
}
