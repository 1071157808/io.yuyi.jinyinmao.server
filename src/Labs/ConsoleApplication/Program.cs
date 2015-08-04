// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Program.cs
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-04  1:44 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Yuyi.Jinyinmao.Packages.Helper;

namespace ConsoleApplication
{
    internal class Program
    {
        private static DateTime GetLastInvestingConfirmTime(DateTime date)
        {
            DailyConfig confirmConfig = DailyConfigHelper.GetLastWorkDayConfig(date, 1);
            return confirmConfig.Date.Date.AddDays(1).AddMilliseconds(-1);
        }

        private static void Main(string[] args)
        {
            var t = GetLastInvestingConfirmTime(DateTime.UtcNow.AddDays(1).AddHours(8));
            Console.WriteLine(t);
        }
    }
}