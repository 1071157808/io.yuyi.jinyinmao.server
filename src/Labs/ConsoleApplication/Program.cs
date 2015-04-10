// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  2:59 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-10  1:25 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans;
using Yuyi.Jinyinmao.Service;
using Yuyi.Jinyinmao.Service.Interface;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.ReadKey();
            GrainClient.Initialize("ClientConfiguration.xml");

            IUserService userService = new UserService();
            var task = userService.GetSignUpUserIdInfoAsync("15800780728");
            task.Wait();
        }
    }
}
