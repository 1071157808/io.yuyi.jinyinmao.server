// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-10  6:28 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  6:23 PM
// ***********************************************************************
// <copyright file="ProductCategoryCodeHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Packages.Helper
{
    /// <summary>
    ///     ProductCategoryCodeHelper.
    /// </summary>
    public static class ProductCategoryCodeHelper
    {
        /// <summary>
        ///     银行承兑
        /// </summary>
        public static long PC100000010 => 100000010;

        /// <summary>
        ///     商业承兑
        /// </summary>
        public static long PC100000020 => 100000020;

        /// <summary>
        ///     金包银
        /// </summary>
        public static long PC100000030 => 100000030;

        /// <summary>
        ///     富滇银票
        /// </summary>
        public static long PC210001010 => 210001010;

        /// <summary>
        ///     富滇商票
        /// </summary>
        public static long PC210001020 => 210001020;

        /// <summary>
        ///     施秉银票
        /// </summary>
        public static long PC210002010 => 210002010;

        /// <summary>
        ///     施秉商票
        /// </summary>
        public static long PC210002020 => 210002020;

        /// <summary>
        /// 阜新银票
        /// </summary>
        public static long PC210003010 => 210003010;

        /// <summary>
        /// 阜新商票
        /// </summary>
        public static long PC210003020 => 210003020;

        /// <summary>
        /// Determines whether [is bank regular product] [the specified product category code].
        /// </summary>
        /// <param name="productCategoryCode">The product category code.</param>
        /// <returns>System.Boolean.</returns>
        public static bool IsBankRegularProduct(long productCategoryCode)
        {
            return productCategoryCode >= 210000000 && productCategoryCode < 300000000;
        }

        /// <summary>
        ///     Determines whether [is jinyinmao jby product] [the specified product category code].
        /// </summary>
        /// <param name="productCategoryCode">The product category code.</param>
        /// <returns><c>true</c> if [is jinyinmao jby product] [the specified product category code]; otherwise, <c>false</c>.</returns>
        public static bool IsJinyinmaoJBYProduct(long productCategoryCode)
        {
            return productCategoryCode == 100000030;
        }

        /// <summary>
        ///     Determines whether [is jinyinmao product] [the specified product category code].
        /// </summary>
        /// <param name="productCategoryCode">The product category code.</param>
        /// <returns><c>true</c> if [is jinyinmao product] [the specified product category code]; otherwise, <c>false</c>.</returns>
        public static bool IsJinyinmaoProduct(long productCategoryCode)
        {
            return productCategoryCode >= 100000000 && productCategoryCode < 200000000;
        }

        /// <summary>
        ///     Determines whether [is jinyinmao regular product] [the specified product category code].
        /// </summary>
        /// <param name="productCategoryCode">The product category code.</param>
        /// <returns><c>true</c> if [is jinyinmao regular product] [the specified product category code]; otherwise, <c>false</c>.</returns>
        public static bool IsJinyinmaoRegularProduct(long productCategoryCode)
        {
            return productCategoryCode >= 100000000 && productCategoryCode < 100000030;
        }
    }
}