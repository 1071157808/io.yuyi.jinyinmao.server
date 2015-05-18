using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Orders.Services.DTO
{
    /// <summary>
    ///     检查取回金额所返回结果
    /// </summary>
    public class CheckRedeemPrincipalResult
    {
        /// <summary>
        ///     当前取回次数
        /// </summary>
        public int RedeemCount { get; set; }
    }
}
