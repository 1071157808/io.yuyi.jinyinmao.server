// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : CacheHelper.cs
// Created          : 2015-08-18  18:36
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-18  18:38
// ***********************************************************************
// <copyright file="CacheHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Web;
using System.Web.Caching;

namespace Yuyi.Jinyinmao.Api.Link.Utils
{
    /// <summary>
    ///     Class CacheHelper.
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        ///     Gets the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>System.Object.</returns>
        public static object GetCache(string cacheKey)
        {
            Cache objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }

        /// <summary>
        ///     Sets the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="objObject">The object object.</param>
        /// <param name="minutes">The timeout.</param>
        public static void SetCache(string cacheKey, object objObject, int minutes)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }
    }
}