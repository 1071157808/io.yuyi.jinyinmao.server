// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : VariableHelper.cs
// Created          : 2015-05-25  1:25 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-04  7:16 PM
// ***********************************************************************
// <copyright file="VariableHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Packages.Helper
{
    /// <summary>
    ///     VariableHelper.
    /// </summary>
    public static class VariableHelper
    {
        /// <summary>
        ///     The daily jby withdrawal amount limit
        /// </summary>
        public static readonly int DailyJBYWithdrawalAmountLimit = 10000000;

        /// <summary>
        ///     The daily withdrawal limit count
        /// </summary>
        public static readonly int DailyWithdrawalLimitCount = 100;

        /// <summary>
        ///     The month free withrawal limit count
        /// </summary>
        public static readonly int MonthFreeWithrawalLimitCount = 4;

        /// <summary>
        /// The transfer destination identifier
        /// </summary>
        public static readonly Guid TransferDestinationId = new Guid("b1aa2356-9102-4259-9f28-46457dab7cc1");

        /// <summary>
        ///     The withdrawal charge fee
        /// </summary>
        public static readonly int WithdrawalChargeFee = 0;
    }
}