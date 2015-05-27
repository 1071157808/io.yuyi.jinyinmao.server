// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-07  11:57 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  6:09 PM
// ***********************************************************************
// <copyright file="JsonHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Packages.Helper
{
    /// <summary>
    ///     JsonHelper.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        ///     Gets the new dictionary.
        /// </summary>
        /// <value>The new dictionary.</value>
        public static string NewDictionary { get; } = (new Dictionary<string, object>()).ToJson();

        /// <summary>
        ///     Gets the new object.
        /// </summary>
        /// <value>The new object.</value>
        public static string NewObject { get; } = (new object()).ToJson();
    }
}