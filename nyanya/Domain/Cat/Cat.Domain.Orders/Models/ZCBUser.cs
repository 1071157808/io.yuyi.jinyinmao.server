using Domian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Orders.Models
{
    public partial class ZCBUser
    {
        public ZCBUser()
        {
        }

        public string ProductIdentifier { get; set; }

        public string UserIdentifier { get; set; }

        public decimal TotalPrincipal { get; set; }

        public decimal CurrentPrincipal { get; set; }

        public decimal TotalInterest { get; set; }

        public decimal TotalRedeemInterest { get; set; }

        public decimal YesterdayInterest { get; set; }

        public string ProductNo { get; set; }
    }
}