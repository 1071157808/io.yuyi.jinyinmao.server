// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-08  4:14 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    public class Apple
    {
        public int Price { get; set; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Apple> apples = new List<Apple>
            {
                new Apple
                {
                    Price = 10
                }
            };
            int amount = apples.Where(a => a.Price > 10).Sum(a => a.Price);
            Console.WriteLine(amount);
        }
    }
}