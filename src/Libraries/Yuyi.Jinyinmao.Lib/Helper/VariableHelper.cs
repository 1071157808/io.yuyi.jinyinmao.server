// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  1:25 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-25  1:25 AM
// ***********************************************************************
// <copyright file="VariableHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

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
        ///     The withdrawal charge fee
        /// </summary>
        public static readonly int WithdrawalChargeFee = 200;
    }
}