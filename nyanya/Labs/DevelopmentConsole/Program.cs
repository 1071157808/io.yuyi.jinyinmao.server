// ***********************************************************************
// Project          : nyanya
// Author           : Siqi Lu
// Created          : 2015-05-18  2:54 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-01  3:28 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Configuration;
using System.Net.Http;
using Infrastructure.Lib.Extensions;
using Moe.Lib;

namespace DevelopmentConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string activityNotifyUrl = ConfigurationManager.AppSettings.Get("ActivityNotifyUrl");
            dynamic e = new { UserIdentifier = "11111111111111111111111111" };
            string timeStamp = DateTime.UtcNow.UnixTimeStamp().ToString();
            string toSign = timeStamp + "ydse@bjkw34sdjfb7w4s#df";
            string sign = MD5Hash.ComputeMD5Hash(toSign);
            string url = StringEx.FormatWith("{0}?dt={1}&sign={2}&userIdentifier={3}", activityNotifyUrl, timeStamp, sign, e.UserIdentifier);
            using (HttpClient client = new HttpClient())
            {
                var t = client.GetAsync(url);
                t.Wait();
                var r = t.Result;
                Console.WriteLine(r.RequestMessage);
            }
        }
    }
}