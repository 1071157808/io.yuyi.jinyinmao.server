// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  2:59 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-07  11:40 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Moe.Lib;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string s = "{0}={1}={2}";
            string s1 = "{1}={2}";
            string a = string.Format(s, 1, 2, 3);
            string b = string.Format(s1, 1, 2, 3);
            string c = string.Format(s, 1);
            string d = string.Format(s1, 1);
            string e = s.FormatWith(1, 2, 3);
            string f = s1.FormatWith(1, 2, 3);
            string g = s.FormatWith(1);
            string h = s1.FormatWith(1);
            Console.WriteLine(a + b + c + d + e + f + g + h);
        }
    }
}
