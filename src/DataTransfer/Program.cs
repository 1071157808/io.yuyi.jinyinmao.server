// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Program.cs
// Created          : 2015-07-30  7:51 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-30  7:54 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.IO;
using Moe.Lib;

namespace DataTransfer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Start");
                Work.Run().Wait();
                Console.WriteLine("Finish");
                //MemoryWork.Run().Wait();
            }
            catch (AggregateException exception)
            {
                Console.WriteLine(exception.Message + exception.StackTrace);
                foreach (var item in exception.InnerExceptions)
                {
                    File.AppendAllText($"{DateTime.Now.ToString("yyyyMMdd")}_log.txt", item.GetExceptionString());
                }
                Console.ReadKey();
            }
            catch (Exception e)
            {
                File.AppendAllText($"{DateTime.Now.ToString("yyyyMMdd")}_log.txt", e.GetExceptionString());
                Console.ReadKey();
            }
            Console.ReadKey();
        }
    }
}