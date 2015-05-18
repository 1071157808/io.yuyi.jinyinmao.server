using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xingye.Domain.Orders.Services.DTO
{
    public class InvestingInfo
    {
        public decimal Interest { get; set; }

        public decimal Principal { get; set; }

        public decimal TotalInterest { get; set; }
    }
}