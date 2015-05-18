// FileInformation: nyanya/Cat.Domain.Users/BankCard.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:24 PM

using System;
using Domian.Models;

namespace Cat.Domain.Users.Models
{
    public class BankCard : IValueObject
    {
        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string CityName { get; set; }

        public int Id { get; set; }

        public bool IsDefault { get; set; }

        public string UserIdentifier { get; set; }

        public DateTime VerifiedTime { get; set; }
    }
}

/*

工商银行	    全国	单笔、单日累计限额5万	卡（账）号 + 姓名
农业银行	    全国	100万	            卡（账）号 + 姓（户）名
建设银行	    全国	100万	            卡(账)号+姓(户)名
招商银行	    全国	100万	            卡(账)号+姓(户)名
广发银行	    全国	100万	            卡（账）号 + 姓（户）名
广州银行	    全国	100万	            卡（账）号 + 姓（户）名+广东+广州
邮储银行	    全国	100万	            卡(账)号+姓(户)名
兴业银行	    全国	100万	            卡(账)号+姓(户)名
光大银行	    全国	100万	            卡（账）号 + 姓名
华夏银行	　	全国	单笔2万，单日5万	    卡（账）号 + 姓名
中信银行	　	全国	单笔2万，单日5万	    卡（账）号 + 姓名
广州农商行	全国	单笔2万，单日5万	    卡（账）号 + 姓名
海南农信社	海南省	100万	        卡（账）号 + 姓名

 */