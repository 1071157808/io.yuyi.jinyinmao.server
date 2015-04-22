// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-21  3:58 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConsoleApplication
{
    internal class MyClass
    {
    }

    internal class Program
    {
        private static readonly Regex regex = new Regex(@"((?<=.)[A-Z][a-zA-Z]*)|((?<=[a-zA-Z])\d+)", RegexOptions.Multiline);

        /// <summary>
        ///     The string replace
        /// </summary>
        private static readonly string strReplace = @"_$1$2";

        private static void Main(string[] args)
        {
            MyClass c = new MyClass();
            string o = regex.Replace(c.GetType().Name, strReplace).ToLowerInvariant();
            Console.WriteLine(o);
        }
    }
}
