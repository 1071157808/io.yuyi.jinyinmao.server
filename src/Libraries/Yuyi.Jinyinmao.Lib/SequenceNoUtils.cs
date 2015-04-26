// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  1:41 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  2:01 AM
// ***********************************************************************
// <copyright file="SequenceNoUtils.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
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
        private static readonly char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private static readonly object lockObject = new object();

        /// <summary>
        ///     Generates the no.
        /// </summary>
        /// <param name="sequencePrefix">The sequence prefix.</param>
        /// <returns>System.String.</returns>
        public static string GenerateNo(char sequencePrefix)
        {
            // A => auth for yilian
            // B => add bank card for yilian
            // O => common order
            // S => common sequence number
            DateTime currentTime;
            lock (lockObject)
            {
                currentTime = DateTime.Now;
            }

            int year = currentTime.Year - 2013;
            if (year < 0)
            {
                year = 0;
            }
            int month = currentTime.Month;
            int day = currentTime.Day;
            int hour = currentTime.Hour;

            string yearChar = characters[year].ToString(CultureInfo.InvariantCulture);
            string monthChar = characters[month].ToString(CultureInfo.InvariantCulture);
            string dayChar = characters[day].ToString(CultureInfo.InvariantCulture);
            string hourChar = characters[hour].ToString(CultureInfo.InvariantCulture);

            string time = currentTime.ToString("mmssffff");

            StringBuilder sb = new StringBuilder();
            sb.Append(sequencePrefix).Append("x").Append(yearChar).Append(monthChar).Append(dayChar).Append(hourChar).Append(time);
            return sb.ToString();
        }
    }
}
