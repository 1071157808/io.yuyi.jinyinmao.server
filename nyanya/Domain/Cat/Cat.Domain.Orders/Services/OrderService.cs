// FileInformation: nyanya/Cat.Domain.Orders/OrderService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/11   10:21 AM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cat.Domain.Orders.Database;
using Cat.Domain.Orders.Models;
using Cat.Domain.Orders.ReadModels;
using Cat.Domain.Orders.Services.Interfaces;
using Infrastructure.Lib;
using Infrastructure.Lib.Extensions;
using Infrastructure.SMS;

namespace Cat.Domain.Orders.Services
{
    public class OrderService : IOrderService
    {
        #region IOrderService Members

        public async Task PublishOrderBuildedEvent(string orderIdentifier)
        {
            Order order = await Order.LoadData(orderIdentifier);
            Console.WriteLine(order.UserIdentifier);
            order.PublishOrderBuildedEvent();
        }

        public async Task SetOrderInterestAsync(string orderIdentifier, decimal interest)
        {
            await new Order(orderIdentifier).SetOrderInterestAsync(interest);
        }

        public async Task SetOrdersRepaidForProductAsync(string productIdentifier)
        {
            List<Order> orders;
            using (OrderContext context = new OrderContext())
            {
                orders = await context.Query<Order>().Include(o => o.PaymentInfo).Include(o => o.ProductInfo)
                    .Where(o => o.ProductInfo.ProductIdentifier == productIdentifier && o.PaymentInfo.IsPaid).ToListAsync();

                foreach (Order order in orders)
                {
                    order.PaymentInfo.IsRepaid = true;
                }

                await context.SaveChangesAsync();
            }

            await SendRepaidSMS(productIdentifier);
        }

        private async Task SendRepaidSMS(string productIdentifier)
        {
            ISmsService service = new SmsService();
            int pageIndex = 0;
            int pageNumber = 50;
            while (true)
            {
                List<OrderInfo> orders;
                using (OrderContext context = new OrderContext())
                {
                    orders = await context.ReadonlyQuery<OrderInfo>()
                        .Where(o => o.ProductIdentifier == productIdentifier && o.IsPaid)
                        .OrderBy(o => o.OrderIdentifier).Skip(pageIndex * pageNumber).Take(pageNumber).ToListAsync();
                }

                if (orders.Count == 0)
                {
                    break;
                }

                foreach (OrderInfo order in orders)
                {
                    string productTitle = order.ProductName + "第" + order.ProductNumber + "期";
                    string amount = (order.Principal + order.Interest + order.ExtraInterest).ToFloor(2).ToString();
                    string message = NyanyaResources.Sms_ProductRepaid.FormatWith(productTitle, amount, order.BankCardNo.GetLast(4), order.BankName);
                    await service.SendAsync(order.InvestorCellphone, message);
                    Thread.Sleep(1000);
                }
                pageIndex++;
            }
        }

        #endregion IOrderService Members
    }
}