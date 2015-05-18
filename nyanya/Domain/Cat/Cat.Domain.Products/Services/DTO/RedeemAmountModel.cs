using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Products.Services.DTO
{
    /// <summary>
    ///     赎回金额所需参数
    /// </summary>
    public class RedeemAmountModel
    {
        /// <summary>
        ///     累计购买金额
        /// </summary>
        public decimal TotalSaleAmount { get; set; }

        /// <summary>
        ///     当日赎回剩余金额
        /// </summary>
        public decimal PerRemainRedeemAmount { get; set; }
    }
}
