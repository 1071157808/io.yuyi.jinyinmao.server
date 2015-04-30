// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-28  12:45 PM
// ***********************************************************************
// <copyright file="SmsServiceFactory.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Api.Sms.Services
{
    /// <summary>
    ///     Class SmsServiceFactory.
    /// </summary>
    internal static class SmsServiceFactory
    {
        /// <summary>
        ///     Gets the SMS service.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>ISmsService.</returns>
        internal static ISmsService GetSmsService(SmsChannel channel)
        {
            switch (channel)
            {
                case SmsChannel.YanZhengMa:
                    return new ZTSmsService(0);

                case SmsChannel.TongZhi:
                    return new ZTSmsService(1);

                case SmsChannel.YingXiao:
                    return new ZTSmsService(2);

                default:
                    return new ZTSmsService(2);
            }
        }
    }
}
