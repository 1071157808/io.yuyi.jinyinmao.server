// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-07-09  10:01 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-09  3:50 PM
// ***********************************************************************
// <copyright file="FileHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.IO;

namespace SagasTransfer
{
    public static class FileHelper
    {
        public static void WriteTo(string baseDir, string msg)
        {
            string dir = Path.Combine(baseDir, "log");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string fileName = $"{DateTime.UtcNow.AddHours(8).ToString("yyyyMMdd")}.txt";
            using (StreamWriter sw = new StreamWriter(File.Open(Path.Combine(dir, fileName), FileMode.Append)))
            {
                sw.WriteLine("--------------------------------Error--------------------------------");
                sw.WriteLine($"Time:{DateTime.UtcNow.AddHours(8).ToLongDateString()} ");
                sw.WriteLine(msg);
                sw.WriteLine("--------------------------------Error--------------------------------");
            }
        }
    }
}