// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  11:42 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Moe.Lib;
using Newtonsoft.Json;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(DateTime.UtcNow.AddHours(8).Hour);
            Console.WriteLine(DateTime.UtcNow.AddHours(9).Hour);
            Console.WriteLine(DateTime.UtcNow.AddHours(10).Hour);

            List<string> r = new List<string>();
            r.Add("1");
            r.Add("2");
            r.Add("3");

            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("a", r);

            var info = new
            {
                Args = d,
                PaidAmount = 10000L,
                UpdateTime = DateTime.UtcNow,
                Yield = "aaaaaa"
            };

            string json = info.ToJson();

            Dictionary<string, object> i = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            var o1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(i["Args"].ToString());
            Console.WriteLine(o1);
            var o5 = JsonConvert.DeserializeObject<string[]>(o1["a"].ToString());
            Console.WriteLine(o5);

            var o2 = Convert.ToInt64(i["PaidAmount"].ToString());
            Console.WriteLine(o2);

            var o3 = Convert.ToDateTime(i["UpdateTime"].ToString());
            Console.WriteLine(o3);

            var o4 = Convert.ToInt32(i["Yield"].ToString());
            Console.WriteLine(o4);
            //            Dictionary<string, object> i = new Dictionary<string, object>
            //            {
            //                { "Args", info.Args },
            //                { "PaidAmount", info.PaidAmount },
            //                { "UpdateTime", info.UpdateTime },
            //                { "Yield", info.Yield }
            //            };
        }
    }
}