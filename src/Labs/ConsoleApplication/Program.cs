// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-03  10:45 PM
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
            Regex r = new Regex(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$", RegexOptions.IgnoreCase);
            Console.WriteLine(r.IsMatch("www.baidu.com")); // 20
            Console.WriteLine(r.IsMatch("www.baidu.com ")); // 21
            Console.WriteLine(r.IsMatch("www.baidu.com/#/uuuu#")); // 19
            Console.WriteLine(r.IsMatch("www.baidu.com/#/uuuu#/?dddddd="));
            Console.WriteLine(r.IsMatch("http://www.baidu.com/#/uuuu#/?dddddd=")); // 15
            Console.WriteLine(r.IsMatch("https://www.baidu.com/#/uuuu#/?dddddd=")); // 15
            Console.WriteLine(r.IsMatch("HTTps://www.baidu.com/#/uuuu#/?dddddd=")); // 15
            Console.WriteLine(r.IsMatch("httpss://www.baidu.com/#/uuuu#/?dddddd=")); // 15
        }
    }
}