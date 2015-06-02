// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-03  2:56 AM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Text.RegularExpressions;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Regex r = new Regex(@"^\d{15,19}$");
            Console.WriteLine(r.IsMatch("11111111111111111111")); // 20
            Console.WriteLine(r.IsMatch("11111111111111111111 ")); // 21
            Console.WriteLine(r.IsMatch("1111111111111111111")); // 19
            Console.WriteLine(r.IsMatch("11111111111111"));
            Console.WriteLine(r.IsMatch("111111111111111")); // 15
        }
    }
}