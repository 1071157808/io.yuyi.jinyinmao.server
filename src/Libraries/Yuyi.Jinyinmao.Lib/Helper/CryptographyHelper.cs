// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  11:52 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-29  11:52 PM
// ***********************************************************************
// <copyright file="CryptographyHelper.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Yuyi.Jinyinmao.Helper
{
    /// <summary>
    ///     Class CryptographyHelper.
    /// </summary>
    public static class CryptographyHelper
    {
        private const string rgbIV = "JYM.YUYI";
        private const string rgbKey = "jym.yuyi";

        /// <summary>
        ///     Checks the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Check(string data, string salt, string encryptedData)
        {
            return string.CompareOrdinal(Encrypting(data, salt), encryptedData.ToUpper()) == 0;
        }

        /// <summary>
        ///     Decrypts the specified payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>System.String.</returns>
        public static string Decrypt(string payload)
        {
            payload = payload.Replace("%", "+");
            byte[] buffer = Convert.FromBase64String(payload);
            using (MemoryStream ms = new MemoryStream())
            {
                DESCryptoServiceProvider cryptoService = new DESCryptoServiceProvider();
                CryptoStream encStream = new CryptoStream(ms, cryptoService.CreateDecryptor(Encoding.UTF8.GetBytes(rgbKey), Encoding.UTF8.GetBytes(rgbIV)), CryptoStreamMode.Write);
                encStream.Write(buffer, 0, buffer.Length);
                encStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        ///     Encrypts the specified payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>System.String.</returns>
        public static string Encrypt(string payload)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(payload);
            using (MemoryStream ms = new MemoryStream())
            {
                DESCryptoServiceProvider cryptoService = new DESCryptoServiceProvider();
                CryptoStream encStream = new CryptoStream(ms, cryptoService.CreateEncryptor(Encoding.UTF8.GetBytes(payload), Encoding.UTF8.GetBytes(rgbIV)), CryptoStreamMode.Write);
                encStream.Write(buffer, 0, buffer.Length);
                encStream.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray()).Replace("+", "%");
            }
        }

        /// <summary>
        ///     Encryptings the specified payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>System.String.</returns>
        public static string Encrypting(string payload, string salt)
        {
            HashAlgorithm mySha256 = SHA256.Create();
            byte[] value = Encoding.UTF8.GetBytes(String.Format("--{0}--{1}--", salt, payload));
            byte[] hashValue = mySha256.ComputeHash(value);
            return BitConverter.ToString(hashValue).Replace("-", "").ToUpper();
        }
    }
}
