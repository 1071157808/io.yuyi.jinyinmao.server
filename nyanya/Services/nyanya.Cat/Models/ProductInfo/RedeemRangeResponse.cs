using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     取现额度
    /// </summary>
    public class RedeemRangeResponse
    {
        /// <summary>
        ///     取现额度百分比
        /// </summary>
        public int RedeemRangePercent { get; set; }

        /// <summary>
        ///     可取现额度
        /// </summary>
        public decimal CanRedeemAmount { get; set; }

        /// <summary>
        ///     用户当天已取现次数
        /// </summary>
        public int RedeemNum { get; set; }
    }

}