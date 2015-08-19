// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Program.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-19  10:48
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
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
            string guid = "98C26A50-0D9B-4089-96F9-15D91F6E5C4C";
            Guid id = guid.AsGuid();
            Console.WriteLine(id);
        }
    }
}