// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-10  6:28 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-10  10:19 PM
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
        public static long PC100000010
        {
            get { return 100000010; }
        }

        /// <summary>
        ///     商业承兑(非指定)
        /// </summary>
        public static long PC100000020
        {
            get { return 100000020; }
        }

        /// <summary>
        ///     商业承兑(国企担保)
        /// </summary>
        public static long PC100000021
        {
            get { return 100000021; }
        }

        /// <summary>
        ///     商业承兑(银行担保)
        /// </summary>
        public static long PC100000022
        {
            get { return 100000022; }
        }

        /// <summary>
        ///     商业承兑(保理业务)
        /// </summary>
        public static long PC100000023
        {
            get { return 100000023; }
        }

        /// <summary>
        ///     金包银
        /// </summary>
        public static long PC100000030
        {
            get { return 100000030; }
        }

        /// <summary>
        ///     富滇银票
        /// </summary>
        public static long PC210001010
        {
            get { return 210001010; }
        }

        /// <summary>
        ///     富滇商票
        /// </summary>
        public static long PC210001020
        {
            get { return 210001020; }
        }

        /// <summary>
        ///     施秉银票
        /// </summary>
        public static long PC210002010
        {
            get { return 210002010; }
        }

        /// <summary>
        ///     施秉商票
        /// </summary>
        public static long PC210002020
        {
            get { return 210002020; }
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