// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-17  10:07 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  1:37 AM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
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
            string APIKey;

            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[256]; //2048 bit
                cryptoProvider.GetBytes(secretKeyByteArray);
                APIKey = Convert.ToBase64String(secretKeyByteArray);
            }

            Console.WriteLine(APIKey);
            Console.WriteLine("Copied to the clibboard.");
            Clipboard.SetText(APIKey);
        }
    }
}
