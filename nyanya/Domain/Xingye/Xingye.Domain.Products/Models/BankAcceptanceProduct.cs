// FileInformation: nyanya/Cqrs.Domain.Product/BankAcceptanceProduct.cs
// CreatedTime: 2014/07/16   5:00 PM
// LastUpdatedTime: 2014/07/21   1:36 AM

using System;

namespace Xingye.Domain.Products.Models
{
    public partial class BankAcceptanceProduct : Product
    {
        /// <summary>
        /// 承兑行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 银承票号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 融资企业营业执照编号
        /// </summary>
        public string BusinessLicense { get; set; }

        /// <summary>
        /// 融资企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 融资用途
        /// </summary>
        public string Usage { get; set; }

    }
}