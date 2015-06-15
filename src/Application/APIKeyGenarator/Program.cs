// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-14  6:24 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace APIKeyGenarator
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            string apiKey;

            using (RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[256]; //2048 bit
                cryptoProvider.GetBytes(secretKeyByteArray);
                apiKey = Convert.ToBase64String(secretKeyByteArray);
            }

            Console.WriteLine(apiKey);
            Console.WriteLine("Copied to the clibboard.");
            Clipboard.SetText(apiKey);
        }
    }
}