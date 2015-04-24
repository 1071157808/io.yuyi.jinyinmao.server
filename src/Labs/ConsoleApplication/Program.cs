// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  7:41 AM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        private static async Task Method1()
        {
            await Task.Run(() => { throw new Exception("1"); });
        }

        private static async Task Method2()
        {
            await Task.Run(() => { throw new Exception("2"); });
        }

        private static async Task Method3()
        {
            await Task.Run(() => { throw new Exception("3"); });
        }

        private static async Task RunAsync()
        {
            try
            {
                Task[] tasks = { Method1(), Method2(), Method3() };
                await Task.WhenAll(tasks);
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.GetExceptionString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetExceptionString());
            }
        }
    }
}
