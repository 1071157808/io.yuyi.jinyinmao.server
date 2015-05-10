// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  9:19 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string t = "2015-01-01 12:02:03";
            DateTime time = DateTime.Parse(t);
            Console.WriteLine(time);
        }
    }
}