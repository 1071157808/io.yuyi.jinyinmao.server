// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  1:41 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-24  10:06 PM
// ***********************************************************************
// <copyright file="SequenceNoUtils.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Globalization;
using System.Text;

namespace Yuyi.Jinyinmao.Packages
{
    /// <summary>
    ///     SequenceNoUtils.
    /// </summary>
    public static class SequenceNoUtils
    {
        private static readonly char[] Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private static readonly object LockObject = new object();

        /// <summary>
        ///     Generates the no.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GenerateNo()
        {
            DateTime currentTime;
            lock (LockObject)
            {
                currentTime = DateTime.UtcNow.AddHours(8);
            }

            int year = currentTime.Year - 2013;
            if (year < 0)
            {
                year = 0;
            }
            int month = currentTime.Month;
            int day = currentTime.Day;
            int hour = currentTime.Hour;

            string yearChar = Characters[year].ToString(CultureInfo.InvariantCulture);
            string monthChar = Characters[month].ToString(CultureInfo.InvariantCulture);
            string dayChar = Characters[day].ToString(CultureInfo.InvariantCulture);
            string hourChar = Characters[hour].ToString(CultureInfo.InvariantCulture);

            string time = currentTime.ToString("mmssffffff");

            StringBuilder sb = new StringBuilder();
            sb.Append(yearChar).Append(monthChar).Append(dayChar).Append(hourChar).Append(time);
            return sb.ToString().ToUpperInvariant();
        }
    }
}