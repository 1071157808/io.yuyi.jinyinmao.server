using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cat.Domain.Orders.Services.DTO
{
    public class ZCBUserRemainRedeemInfo
    {
        public decimal RemainPrincipal { get; set; }

        public decimal RemainRedeemInterest { get; set; }

        public decimal TodayPrincipal { get; set; }
    }
}