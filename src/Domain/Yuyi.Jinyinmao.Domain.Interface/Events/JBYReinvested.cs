// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-19  12:53 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  12:58 AM
// ***********************************************************************
// <copyright file="JBYReinvested.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYReinvested.
    /// </summary>
    public class JBYReinvested : Event
    {
        /// <summary>
        ///     Gets or sets the transcation information.
        /// </summary>
        /// <value>The transcation information.</value>
        public JBYAccountTranscationInfo TranscationInfo { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}