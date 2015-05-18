// FileInformation: nyanya/Cqrs.Domain.Order/OrderService.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/29   9:46 AM

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xingye.Domain.Orders.Database;
using Xingye.Domain.Orders.Models;
using Xingye.Domain.Orders.Services.Interfaces;
using Infrastructure.Lib;
using Infrastructure.Lib.Extensions;
using Infrastructure.SMS;

namespace Xingye.Domain.Orders.Services
{
    public class OrderService : IOrderService
    {
        #region Public Methods

        public async Task PublishOrderBuildedEvent(string orderIdentifier)
        {
            Order order = await Order.LoadData(orderIdentifier);
            order.PublishOrderBuildedEvent();
        }

        public async Task SetOrdersRepaidForProductAsync(string productIdentifier)
        {
            ISmsService service = new SmsService();
            int pageIndex = 0;
            int pageNumber = 50;
            while (true)
            {
                List<Order> orders;
                using (OrderContext context = new OrderContext())
                {
                    orders = await context.ReadonlyQuery<Order>().Include(o => o.PaymentInfo).Include(o => o.ProductInfo)
                        .Where(o => o.ProductInfo.ProductIdentifier == productIdentifier && o.PaymentInfo.IsPaid)
                        .OrderBy(o => o.OrderIdentifier).Skip(pageIndex * pageNumber).Take(pageNumber).ToListAsync();
                }

                if (orders.Count == 0)
                {
                    break;
                }

                foreach (Order order in orders)
                {
                    string productTitle = order.ProductInfo.ProductName + "第" + order.ProductInfo.ProductNumber + "期";
                    string amount = (order.Principal + order.Interest + order.ExtraInterest).ToIntFormat().ToString();
                    string message = NyanyaResources.Sms_xy_ProductRepaid.FormatWith(productTitle, amount, order.PaymentInfo.BankCardNo.GetLast(4), order.PaymentInfo.BankName);
                    await service.SendAsync(order.InvestorInfo.Cellphone, message, 1);
                }
            }
        }

        #endregion Public Methods
    }
}