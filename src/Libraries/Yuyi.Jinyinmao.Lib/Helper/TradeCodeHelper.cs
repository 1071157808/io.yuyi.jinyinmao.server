// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  6:22 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  6:23 PM
// ***********************************************************************
// <copyright file="TradeCodeHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Packages.Helper
{
    /// <summary>
    ///     TradeCodeHelper.
    /// </summary>
    public static class TradeCodeHelper
    {
        /// <summary>
        ///     钱包收到金包银转入金额
        /// </summary>
        public static int TC1005011103 => 1005011103;

        /// <summary>
        ///     钱包收到银票或者商票产品返还本金
        /// </summary>
        public static int TC1005011104 => 1005011104;

        /// <summary>
        ///     钱包收到银票或者商票产品结算利息
        /// </summary>
        public static int TC1005011105 => 1005011105;

        /// <summary>
        /// 钱包账户收到签到活动奖励
        /// </summary>
        public static int TC1005011107 => 1005011107;

        /// <summary>
        ///     钱包金额转为金包银金额
        /// </summary>
        public static int TC1005012003 => 1005012003;

        /// <summary>
        ///     购买银票或者商票产品
        /// </summary>
        public static int TC1005012004 => 1005012004;

        /// <summary>
        ///     钱包取现收取手续费
        /// </summary>
        public static int TC1005012102 => 1005012102;

        /// <summary>
        ///     钱包收到银票或者商票产品返还本金(银行专区)
        /// </summary>
        public static int TC1005021104 => 1005021104;

        /// <summary>
        ///     钱包收到银票或者商票产品结算利息(银行专区)
        /// </summary>
        public static int TC1005021105 => 1005021105;

        /// <summary>
        ///     购买银票或者商票产品(银行专区)
        /// </summary>
        public static int TC1005022004 => 1005022004;

        /// <summary>
        ///     个人钱包账户充值
        /// </summary>
        public static int TC1005051001 => 1005051001;

        /// <summary>
        ///     个人钱包账户取现
        /// </summary>
        public static int TC1005052001 => 1005052001;

        /// <summary>
        ///     金包银收到金包银的复利投资
        /// </summary>
        public static int TC2001011106 => 2001011106;

        /// <summary>
        ///     金包银金额转为钱包金额
        /// </summary>
        public static int TC2001012002 => 2001012002;

        /// <summary>
        ///     金包银金额收到钱包转入金额
        /// </summary>
        public static int TC2001051102 => 2001051102;

        /// <summary>
        ///     Determines whether the specified trade code is crebit.
        /// </summary>
        /// <param name="tradeCode">The trade code.</param>
        /// <returns><c>true</c> if the specified trade code is crebit; otherwise, <c>false</c>.</returns>
        public static bool IsCrebit(int tradeCode) => tradeCode.ToString()[6] == '2';

        /// <summary>
        ///     Determines whether the specified trade code is debit.
        /// </summary>
        /// <param name="tradeCode">The trade code.</param>
        /// <returns><c>true</c> if the specified trade code is debit; otherwise, <c>false</c>.</returns>
        public static bool IsDebit(int tradeCode) => tradeCode.ToString()[6] == '1';
    }
}