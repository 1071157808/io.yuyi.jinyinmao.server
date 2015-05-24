// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-23  10:41 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-23  10:41 PM
// ***********************************************************************
// <copyright file="SpecialIdHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Packages.Helper
{
    /// <summary>
    ///     SpecialIdHelper.
    /// </summary>
    public static class SpecialIdHelper
    {
        private static readonly Guid reinvestingJBYTranscationProductId = Guid.Parse("92CFADC4-91A5-4A09-8D0E-AC122C837F5B");

        private static readonly Guid withdrawalJBYTranscationProductId = Guid.Parse("92CFADC4-91A5-4A09-8D0E-AC122C837F5B");

        /// <summary>
        /// Gets the j reinvesting by transcation product identifier.
        /// </summary>
        /// <value>The j reinvesting by transcation product identifier.</value>
        public static Guid ReinvestingJBYTranscationProductId
        {
            get { return reinvestingJBYTranscationProductId; }
        }

        /// <summary>
        /// Gets the withdrawal jby transcation product identifier.
        /// </summary>
        /// <value>The withdrawal jby transcation product identifier.</value>
        public static Guid WithdrawalJBYTranscationProductId
        {
            get { return withdrawalJBYTranscationProductId; }
        }
    }
}