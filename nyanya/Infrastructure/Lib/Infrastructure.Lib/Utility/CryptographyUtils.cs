// FileInformation: nyanya/Infrastructure.Lib/CryptographyUtils.cs
// CreatedTime: 2014/06/24   11:13 AM
// LastUpdatedTime: 2014/07/16   1:30 PM

using System;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Lib.Utility
{
    // 单向加密方法
    public static class CryptographyUtils
    {
        public static bool Check(string data, string salt, string encryptedData)
        {
            return String.CompareOrdinal(Encrypting(data, salt), encryptedData.ToUpper()) == 0;
        }

        public static string Encrypting(string payload, string salt)
        {
            HashAlgorithm mySha256 = SHA256.Create();
            byte[] value = Encoding.UTF8.GetBytes(String.Format("--{0}--{1}--", salt, payload));
            byte[] hashValue = mySha256.ComputeHash(value);
            return BitConverter.ToString(hashValue).Replace("-", "").ToUpper();
        }
    }
}