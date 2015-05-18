using Cat.Domain.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Lib.Extensions;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     ZCBUserBillResponse
    /// </summary>
    public class ZCBUserBillResponse
    {
        /// <summary>
        ///     日期
        /// </summary>
        public string BillDate { get; set; }

        /// <summary>
        ///     投资总额
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
        ///     备注
        /// </summary>
        public string Remark { get; set; }
    }

    internal static partial class ZCBUserBillResponseExtensions
    {
        #region Internal Methods
        internal static ZCBUserBillResponse ToZCBUserBillResponse(this ZCBUserBill zcbUserBill)
        {
            ZCBUserBillResponse item = new ZCBUserBillResponse
            {
                BillDate = zcbUserBill.BillDate.ToShortFormat(),
                Principal = zcbUserBill.Principal,
                Yield = zcbUserBill.Yield,
                Interest = zcbUserBill.Interest.ToFloor(2),
                Remark = zcbUserBill.Remark
            };
            return item;
        }
        #endregion Internal Methods
    }
}