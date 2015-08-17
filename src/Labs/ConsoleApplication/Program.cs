// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Program.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  13:56
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace ConsoleApplication
{
    internal class Program
    {
        /// <summary>
        ///     The floor limit
        /// </summary>
        private static readonly int FloorLimit = 1;

        /// <summary>
        ///     The maximum bonus amount
        /// </summary>
        private static readonly long MaxBonusAmount = 1000L;

        /// <summary>
        ///     The minimum bonus amount
        /// </summary>
        private static readonly long MinBonusAmount = 1L;

        /// <summary>
        ///     The random
        /// </summary>
        private static readonly Random Random = new Random(DateTime.Now.DayOfYear);

        /// <summary>
        ///     The upper limit
        /// </summary>
        private static readonly int UpperLimit = 1000;

        private static void Main(string[] args)
        {
            long baseAmount = 100000000;
            int r = Random.Next(FloorLimit, UpperLimit + 1);

            long bonus = Convert.ToInt64(Math.Pow(Convert.ToDouble(baseAmount) / 100000000d, 0.25d) * Math.Pow(Convert.ToDouble(r), 2d) / 1800);

            if (bonus <= MinBonusAmount)
            {
                bonus = MinBonusAmount;
            }
            else if (bonus > MaxBonusAmount)
            {
                bonus = MaxBonusAmount;
            }

            Console.WriteLine(bonus);
        }
    }
}