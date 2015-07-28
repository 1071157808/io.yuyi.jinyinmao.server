// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Utils.cs
// Created          : 2015-07-28  11:38 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-28  11:45 AM
// ***********************************************************************
// <copyright file="Utils.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using DataTransfer.Models;
using Yuyi.Jinyinmao.Domain;

namespace DataTransfer
{
    internal class Utils
    {
        private readonly static Dictionary<string, object> Args = new Dictionary<string, object>() { { "Comment", "由原银行卡数据迁移" } };
        public static Dictionary<string, BankCard> GetBankCards(string userId)
        { 
            Guid id = new Guid(userId);
            using (var context = new OldDBContext())
            {
                var bankCards = context.TransBankCard.Where(x => x.UserId == userId).ToList().Select(b => new BankCard
                {
                    AddingTime = b.AddingTime,
                    Args = Args,
                    BankCardNo = b.BankCardNo,
                    BankName = b.BankName,
                    Cellphone = b.Cellphone,
                    CityName = b.CityName,
                    Dispaly = true,
                    Verified = true,
                    VerifiedByYilian = true,
                    UserId = id,
                    VerifiedTime = b.VerifiedTime,
                    WithdrawAmount = b.WithdrawAmount
                }).ToDictionary<BankCard, string>(x => x.BankCardNo);
                return bankCards;
            }
        }

        public static Credential GetCredential(int? credential = -1)
        {
            switch (credential)
            {
                case 0:
                    return Credential.IdCard;

                case 1:
                    return Credential.Passport;

                case 2:
                    return Credential.Taiwan;

                case 3:
                    return Credential.Junguan;

                default:
                    return Credential.None;
            }
        }

        public static DateTime GetDate(DateTime date)
        {
            return date.Date;
        }

        public static string GetOutletCode(string outletCode)
        {
            List<string> list = new List<string>() { "901", "902", "902" };
            return list.IndexOf(outletCode) != -1 ? string.Empty : outletCode;
        }

        public static long GetProductCategory(int productCategory = 0, int? productType = 0)
        {
            if (productType != 0)
            {
                switch (productType)
                {
                    case 10:
                        return 100000010;

                    case 20:
                        return 100000020;
                }
                return -1;
            }
            switch (productCategory)
            {
                case 20:
                    return 210001010;

                case 30:
                    return 210002020;

                case 40:
                    return 210003010;

                default:
                    return -1;
            }
        }

        public static string GetProductName(string name)
        {
            string s = name.Replace("“", "\"");
            return s;
        }

        public static string GetRiskManagementMode(int? mode)
        {
            if (mode == null)
            {
                mode = -1;
            }
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

        public static TResult ReflectProperties<TResult, TSource>(TSource source) where TResult : new()
        {
            Type sourceType = source.GetType();
            TResult result = new TResult();
            Type resultType = result.GetType();
            var props = sourceType.GetProperties();

            foreach (var item in props)
            {
                var prop = resultType.GetProperty(item.Name);

                prop?.SetValue(result, item.GetValue(source));
            }

            return result;
        }
    }
}