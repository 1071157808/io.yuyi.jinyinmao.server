using Infrastructure.Lib.Secutiry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace nyanya.Infrastructure.Lib.Tests.Secutiry
{
     [TestClass]
    public class DESEncryptorTests
     {
         /// <summary>
         /// 加密键值
         /// </summary>
         private const string Key = "YoNyanya";

         [TestMethod]
         public void DESEncryptor_加密_Test()
         {
             const string inputString = "13916203671|2014-10-25 16:21";

             var des = new DESEncryptor { EncryptKey = Key, InputString = inputString };
             //加密
             des.DesEncrypt();

             var outString = des.OutString;

             Assert.IsNotNull(outString);
         }

         [TestMethod]
         public void DESEncryptor_解密_Test()
         {
             const string inputString = "13916203671|2014-10-25 16:21";
             const string inputString2 = "13916203671|2014-10-25 16:22";

             //加密
             var encryptString = DesEncrypt(Key, inputString);
             //加密
             var encryptString2 = DesEncrypt(Key, inputString2);
             //解密
             var decryptString = DesDecrypt(Key, encryptString);
             //解密
             var decryptString2 = DesDecrypt(Key, encryptString2);
             

             Assert.AreEqual(decryptString, inputString);
             Assert.AreEqual(decryptString2, inputString2);
         }


         #region 私有方法
         /// <summary>
         /// 加密
         /// </summary>
         /// <param name="key">加密的Key值</param>
         /// <param name="inputString">需要加密的字符</param>
         /// <returns>加密后的字符</returns>
         public string DesEncrypt(string key, string inputString)
         {
             var des = new DESEncryptor { EncryptKey = Key, InputString = inputString };
             //加密
             des.DesEncrypt();
             //返回加密后的字符串
             return des.OutString;
         }

         /// <summary>
         /// 解密
         /// </summary>
         /// <param name="key">解密的Key值</param>
         /// <param name="inputString">需要解密的字符</param>
         /// <returns>解密后的字符</returns>
         public string DesDecrypt(string key, string inputString)
         {
             var des = new DESEncryptor { DecryptKey = Key, InputString = inputString };
             //解密
             des.DesDecrypt();
             //返回解密后的字符串
             return des.OutString;
         } 
         #endregion
     }
}