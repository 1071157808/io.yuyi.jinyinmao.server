// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-21  12:06 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  12:24 AM
// ***********************************************************************
// <copyright file="GrainType.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Enum GrainType
    /// </summary>
    public enum GrainType
    {
        /// <summary>
        ///     The cellphone, 100001
        /// </summary>
        Cellphone = 100001,

        /// <summary>
        ///     The jby
        /// </summary>
        JBY = 100002
    }

    /// <summary>
    ///     Class GrainTypeHelper.
    /// </summary>
    public static class GrainTypeHelper
    {
        /// <summary>
        ///     The trillion
        /// </summary>
        public const long Trillion = 1000000000000;

        /// <summary>
        ///     Gets the grain type long key.
        /// </summary>
        /// <param name="grainType">Type of the grain.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.Int64.</returns>
        public static long GetGrainTypeLongKey(GrainType grainType, int key)
        {
            return GetGrainTypeLongKey(grainType, Convert.ToInt64(key));
        }

        /// <summary>
        ///     Gets the grain type long key.
        /// </summary>
        /// <param name="grainType">Type of the grain.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.Int64.</returns>
        public static long GetGrainTypeLongKey(GrainType grainType, string key)
        {
            return GetGrainTypeLongKey(grainType, Convert.ToInt64(key));
        }

        /// <summary>
        ///     Gets the grain type long key.
        /// </summary>
        /// <param name="grainType">Type of the grain.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.Int64.</returns>
        public static long GetGrainTypeLongKey(GrainType grainType, long key)
        {
            return Convert.ToInt64(grainType) * Trillion + key;
        }

        /// <summary>
        /// Gets the jby grain type long key.
        /// </summary>
        /// <returns>System.Int64.</returns>
        public static long GetJBYGrainTypeLongKey()
        {
            return GrainTypeHelper.GetGrainTypeLongKey(GrainType.JBY, ProductCategoryCodeHelper.PC100000030);
        }
    }
}