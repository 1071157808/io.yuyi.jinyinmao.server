using DataTransfer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace DataTransfer
{
    class Utils
    {
        public static DateTime getDate(DateTime date)
        {
            return date.Date;
        }

        public static long getProductCategory(int productCategory = 0, int? productType = 0)
        {
            if (productType != 0)
            {
                if (productType == 10)
                {
                    return 100000010;
                }
                else if (productType == 20)
                {
                    return 100000020;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                switch (productCategory)
                {
                    case 20:
                        return 210001010;
                        break;
                    case 30:
                        return 210002020;
                        break;
                    case 40:
                        return 210003010;
                        break;
                    default:
                        return -1;
                        break;
                }
            }
        }

        public static string getProductName(string name)
        {
            string s = name.Replace("“", "\"");
            return s;
        }

        public static string getRiskManagementMode(int? mode)
        {
            if (mode == null) { mode = -1; }
            switch (mode)
            {
                case 20:
                    return "央企担保";
                case 30:
                    return "国企担保";
                case 40:
                    return "国有担保公司担保";
                case 50:
                    return "担保公司担保";
                case 60:
                    return "上市集团担保";
                case 70:
                    return "集团担保";
                case 80:
                    return "国资参股担保公司担保";
                case 90:
                    return "银行担保";
                case 100:
                    return "金银猫全程监管资金流向";
                case 110:
                    return "上市公司无条件承兑";
                case -1:
                    return "银行无条件承兑";
                default:
                    return string.Empty;
            }
        }

        public static Credential getCredential(int? credential = -1)
        {
            switch (credential)
            {
                case 0:
                    return Credential.IdCard;
                    break;
                case 1:
                    return Credential.Passport;
                    break;
                case 2:
                    return Credential.Taiwan;
                    break;
                case 3:
                    return Credential.Junguan;
                    break;
                default:
                    return Credential.None;
                    break;
            }
        }

        public static string getOutletCode(string outletCode)
        {
            if (outletCode.Equals("901") || outletCode.Equals("902") || outletCode.Equals("903"))
            {
                return "";
            }
            else
                return outletCode;
        }

        public static TResult ReflectProperties<TResult, TSource>(TSource source) where TResult : new()
        {
            Type sourceType = source.GetType();
            TResult result = new TResult();
            Type resultType = result.GetType();
            var props = sourceType.GetProperties();

            foreach (var item in props)
            {
                var prop = resultType.GetProperty(item.Name);

                if (prop == null) continue;
                prop.SetValue(result, item.GetValue(source));
            }

            return result;
        }


    }
}
