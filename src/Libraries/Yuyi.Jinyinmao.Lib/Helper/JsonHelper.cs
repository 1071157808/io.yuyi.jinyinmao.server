// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-07  11:57 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  12:01 PM
// ***********************************************************************
// <copyright file="JsonHelper.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Packages.Helper
{
    /// <summary>
    ///     JsonHelper.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        ///     The new dictionary
        /// </summary>
        private static readonly string newDictionary = (new Dictionary<string, object>()).ToJson();

        /// <summary>
        ///     The new object
        /// </summary>
        private static readonly string newObject = (new object()).ToJson();

        /// <summary>
        ///     Gets the new dictionary.
        /// </summary>
        /// <value>The new dictionary.</value>
        [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
        public static string NewDictionary
        {
            get { return newDictionary; }
        }

        /// <summary>
        ///     Gets the new object.
        /// </summary>
        /// <value>The new object.</value>
        [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
        public static string NewObject
        {
            get { return newObject; }
        }
    }
}