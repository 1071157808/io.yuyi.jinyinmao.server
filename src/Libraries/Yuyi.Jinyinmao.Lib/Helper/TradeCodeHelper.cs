// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  6:22 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  11:51 AM
// ***********************************************************************
// <copyright file="TradeCodeHelper.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
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
        /// 钱包收到金包银转入金额
        /// </summary>
        public static int TC1005011103
        {
            get { return 1005011103; }
        }

        /// <summary>
        ///     钱包收到银票或者商票产品返还本金
        /// </summary>
        public static int TC1005011104
        {
            get { return 1005011104; }
        }

        /// <summary>
        ///     钱包收到银票或者商票产品结算利息
        /// </summary>
        public static int TC1005011105
        {
            get { return 1005011105; }
        }

        /// <summary>
        /// 钱包金额转为金包银金额
        /// </summary>
        public static int TC1005012003
        {
            get { return 1005012003; }
        }

        /// <summary>
        ///     购买银票或者商票产品
        /// </summary>
        public static int TC1005012004
        {
            get { return 1005012004; }
        }

        /// <summary>
        ///     钱包取现收取手续费
        /// </summary>
        public static int TC1005012102
        {
            get { return 1005012102; }
        }

        /// <summary>
        ///     钱包收到银票或者商票产品返还本金(银行专区)
        /// </summary>
        public static int TC1005021104
        {
            get { return 1005021104; }
        }

        /// <summary>
        ///     钱包收到银票或者商票产品结算利息(银行专区)
        /// </summary>
        public static int TC1005021105
        {
            get { return 1005021105; }
        }

        /// <summary>
        ///     购买银票或者商票产品(银行专区)
        /// </summary>
        public static int TC1005022004
        {
            get { return 1005022004; }
        }

        /// <summary>
        ///     个人钱包账户充值
        /// </summary>
        public static int TC1005051001
        {
            get { return 1005051001; }
        }

        /// <summary>
        ///     个人钱包账户取现
        /// </summary>
        public static int TC1005052001
        {
            get { return 1005052001; }
        }

        /// <summary>
        /// 金包银金额转为钱包金额
        /// </summary>
        public static int TC2001012002
        {
            get { return 2001012002; }
        }

        /// <summary>
        /// 金包银金额收到钱包转入金额
        /// </summary>
        public static int TC2001051102
        {
            get { return 2001051102; }
        }

        /// <summary>
        ///     Determines whether the specified trade code is crebit.
        /// </summary>
        /// <param name="tradeCode">The trade code.</param>
        /// <returns><c>true</c> if the specified trade code is crebit; otherwise, <c>false</c>.</returns>
        public static bool IsCrebit(int tradeCode)
        {
            return tradeCode.ToString()[6] == '2';
        }

        /// <summary>
        ///     Determines whether the specified trade code is debit.
        /// </summary>
        /// <param name="tradeCode">The trade code.</param>
        /// <returns><c>true</c> if the specified trade code is debit; otherwise, <c>false</c>.</returns>
        public static bool IsDebit(int tradeCode)
        {
            return tradeCode.ToString()[6] == '1';
        }
    }
}