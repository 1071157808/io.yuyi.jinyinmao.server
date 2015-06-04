// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-04  3:31 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Yuyi.Jinyinmao.Service;
using Yuyi.Jinyinmao.Service.Interface;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IProductService service = new ProductService();
            var t = service.GetJBYMaxIssueNoAsync();
            t.Wait();
            int i = t.Result;
            Console.WriteLine(i);
        }
    }
}