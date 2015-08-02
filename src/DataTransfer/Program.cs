// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Program.cs
// Created          : 2015-07-31  2:27 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  2:35 PM
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
                string arg = args[0].IsNullOrEmpty() ? "0" : args[0];

                switch (arg)
                {

                    case "0":
                        Console.WriteLine("Start Full Mode");
                        Work.Run().Wait();
                        MemoryWork.RunProduct().Wait();
                        MemoryWork.RunUser().Wait();
                        Console.WriteLine("Finish");
                        break;

                    case "1":
                        Console.WriteLine("Start DB Mode");
                        Work.Run().Wait();
                        //MemoryWork.Run().Wait();
                        Console.WriteLine("Finish");
                        break;

                    case "2":
                        Console.WriteLine("Start Product Memory Mode");
                        //Work.Run().Wait();
                        MemoryWork.RunProduct().Wait();
                        Console.WriteLine("Finish");
                        break;
                    case "3":
                        Console.WriteLine("Start User Memory Mode");
                        //Work.Run().Wait();
                        MemoryWork.RunUser().Wait();
                        Console.WriteLine("Finish");
                        break;
                }
                Console.WriteLine("Finish");
            }
            catch (AggregateException exception)
            {
                Console.WriteLine(exception.Message + exception.StackTrace);
                foreach (var item in exception.InnerExceptions)
                {
                    File.AppendAllText(DateTime.Now.ToString("yyyyMMdd") + "_log.txt", item.GetExceptionString());
                }
                Console.ReadKey();
            }
            catch (Exception e)
            {
                File.AppendAllText(DateTime.Now.ToString("yyyyMMdd") + "_log.txt", e.GetExceptionString());
                Console.ReadKey();
            }
            Console.ReadKey();
        }
    }
}