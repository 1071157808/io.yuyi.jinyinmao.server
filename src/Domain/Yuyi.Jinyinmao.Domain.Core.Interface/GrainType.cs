// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-07  10:53 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-10  12:57 PM
// ***********************************************************************
// <copyright file="GrainType.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

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
        Cellphone = 100001
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
            return Convert.ToInt64(grainType) * Trillion + Convert.ToInt64(key);
        }
    }
}
