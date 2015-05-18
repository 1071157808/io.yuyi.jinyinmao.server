using Cat.Domain.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Lib.Extensions;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     ZCBUserResponse
    /// </summary>
    public class ZCBUserResponse
    {
        /// <summary>
        ///     产品唯一标示符
        /// </summary>
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     产品编号
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        ///     投资总金额
        /// </summary>
        public decimal TotalPrincipal { get; set; }

        /// <summary>
        ///     当前正在投资总额
        /// </summary>
        public decimal CurrentPrincipal { get; set; }

        /// <summary>
        ///     累计总收益
        /// </summary>
        public decimal TotalInterest { get; set; }

        /// <summary>
        ///     累计提现收益
        /// </summary>
        public decimal TotalRedeemInterest { get; set; }

        /// <summary>
        ///     昨日收益
        /// </summary>
        public decimal YesterdayInterest { get; set; }

        /// <summary>
        ///     今日利率
        /// </summary>
        public decimal Yield { get; set; }
    }

    internal static partial class ZCBUserResponseExtensions
    {
        #region Internal Methods
        internal static ZCBUserResponse ToZCBUserResponse(this ZCBUser zcbUser, decimal yield)
        {
            ZCBUserResponse item = new ZCBUserResponse
            {
                ProductIdentifier = zcbUser.ProductIdentifier,
                ProductNo = zcbUser.ProductNo,
                TotalPrincipal = zcbUser.TotalPrincipal,
                CurrentPrincipal = zcbUser.CurrentPrincipal,
                TotalInterest = zcbUser.TotalInterest.ToFloor(2),
                TotalRedeemInterest = zcbUser.TotalRedeemInterest.ToFloor(2),
                YesterdayInterest = zcbUser.YesterdayInterest.ToFloor(2),
                Yield = yield
            };
            return item;
        }
        #endregion Internal Methods
    }
}