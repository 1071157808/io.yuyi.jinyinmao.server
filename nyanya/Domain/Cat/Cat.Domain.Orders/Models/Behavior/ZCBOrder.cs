using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Lib.Utility;

namespace Cat.Domain.Orders.Models
{
    public partial class ZCBOrder
    {
        //public decimal ComputeInterest()
        //{
        //    Guard.IdentifierMustBeAssigned(OrderIdentifier, GetType().ToString());
        //    return ComputeOriginalInterest(RemainPrincipal, ValueDate.Date, DateTime.Now.Date, Yield, FilterBill(ZCBBills));
        //}
        //private IList<ZCBBill> FilterBill(IEnumerable<ZCBBill> bills)
        //{
        //    return bills.Where(x => x.RedeemTime.Date < DateTime.Now.Date)   //只取今天之前的记录
        //                .OrderBy(x=>x.RedeemTime).ToList();             
        //}
    }
}
