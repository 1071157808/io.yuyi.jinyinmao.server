using Domian.Models;
using System;
using System.Collections.Generic;

namespace Cat.Domain.Orders.Models
{
    public partial class ZCBUserBill : IValueObject
    {
        public ZCBUserBill()
        {
        }

        public int Id { get; set; }
        /// <summary>
        ///     用户唯一标识
        /// </summary>
        public string UserIdentifier { get; set; }

        /// <summary>
        ///     产品唯一标识
        /// </summary>
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     投资总金额
        /// </summary>
        public decimal Principal { get; set; }

        /// <summary>
        ///     收益率
        /// </summary>
        public decimal Yield { get; set; }

        /// <summary>
        ///     收益
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        ///     日期
        /// </summary>
        public DateTime BillDate { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }
    }
}
