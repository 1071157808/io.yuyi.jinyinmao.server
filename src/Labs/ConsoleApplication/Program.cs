// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  2:59 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  7:27 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Yuyi.Jinyinmao.Service.Interface;
using Yuyi.Jinyinmao.Service.Misc.Interface;
using Yuyi.Jinyinmao.Services;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IVeriCodeService service = new VeriCodeService(new SmsService());
            var t1 = service.SendAsync("15800780728", VeriCodeType.SignUp);
            t1.Wait();
            Console.WriteLine(t1.Result);

            var t2 = service.VerifyAsync("15800780728", "225555", VeriCodeType.SignUp);
            t2.Wait();
            Console.WriteLine(t2.Result);

            var t3 = service.UseAsync(t2.Result.Token, VeriCodeType.SignUp);
            t3.Wait();
            Console.WriteLine(t3.Result);
        }
    }
}
