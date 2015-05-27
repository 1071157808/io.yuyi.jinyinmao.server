// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-28  12:45 PM
// ***********************************************************************
// <copyright file="SmsChannel.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Api.Sms.Services
{
    /// <summary>
    ///     Enum SmsChannel
    /// </summary>
    public enum SmsChannel
    {
        /// <summary>
        ///     助通验证码
        /// </summary>
        [Description("验证码")]
        YanZhengMa = 100001,

        /// <summary>
        ///     助通通知
        /// </summary>
        [Description("通知")]
        TongZhi = 100002,

        /// <summary>
        ///     助通营销
        /// </summary>
        [Description("营销")]
        YingXiao = 100003
    }

    internal static class SmsChannelEnumHelper
    {
        private static readonly Lazy<Dictionary<int, string>> Channels = new Lazy<Dictionary<int, string>>(
            () => Enum.GetValues(typeof(SmsChannel)).Cast<SmsChannel>().ToDictionary(value => Convert.ToInt32(value), value => value.Description()));

        internal static Dictionary<int, string> GetChannels() => Channels.Value;
    }
}