// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  6:36 PM
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
            int jbyAccrualAmount = 20000309;
            int yield = 888;
            int interest = jbyAccrualAmount * yield / 3600000;
            //            DateTime now = new DateTime(2015, 06, 16, 16, 58, 12);
            //            int yield = DailyConfigHelper.GetDailyConfig(now.AddDays(-1)).JBYYield;
            //            DateTime confirmTime = GetLastInvestingConfirmTime(now);
            //            DateTime t1 = GetLastInvestingConfirmTime(now.AddDays(-1));
            //            DateTime t2 = GetLastInvestingConfirmTime(now.AddDays(-2));
            //            DateTime t3 = GetLastInvestingConfirmTime(now.AddDays(-3));
            //            DateTime t4 = GetLastInvestingConfirmTime(now.AddDays(-4));
            //            DateTime t5 = GetLastInvestingConfirmTime(now.AddDays(-5));
            //
            //
            //            Console.WriteLine(confirmTime);
            //            Console.WriteLine(interest);
            Console.WriteLine(interest);
        }
    }
}