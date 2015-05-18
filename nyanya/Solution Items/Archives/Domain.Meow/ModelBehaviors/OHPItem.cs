// FileInformation: nyanya/Domain.Meow/OHPItem.cs
// CreatedTime: 2014/05/13   8:14 AM
// LastUpdatedTime: 2014/05/14   3:05 PM

using System;
using System.Threading.Tasks;
using Domain.Order.Models;
using Domain.Order.Services.Interfaces;

namespace Domain.Meow.Models
{
    public partial class OHPItem
    {
        public static bool AvailableForOrder(IOrder orderContext)
        {
            return orderContext.Status == OrderStatus.Paid;
        }

        public override bool Available(IOrder orderContext)
        {
            bool result = AvailableForOrder(orderContext);
            return result && base.Available(orderContext);
        }

        public async Task<decimal?> ConsumeByOrderAsync(IOrder orderContext)
        {
            if (!this.Available(orderContext))
            {
                return null;
            }

            using (MeowContext meowContext = new MeowContext())
            {
                OHPItem item = await meowContext.OHPItems.FindAsync(this.Id);
                item.UseTime = DateTime.Now;
                item.IsUsed = true;
                item.OrderId = orderContext.OrderId;

                // HACK:Hard Code 道具金额
                // 2014-05-09 16:32 道具金额修改为200元
                // 200 * (orderContext.Yield / 100) * orderContext.Duration / 360
                item.ExtraInterest = orderContext.Yield * orderContext.Duration / 180;
                await meowContext.SaveChangesAsync();
                return item.ExtraInterest;
            }
        }
    }
}