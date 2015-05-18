// FileInformation: nyanya/PublishOrderBuildedEventToolkit/Program.cs
// CreatedTime: 2014/08/29   9:33 AM
// LastUpdatedTime: 2014/09/01   4:30 PM

using System;
using Cat.Domain.Orders.Services;
using Cat.Domain.Orders.Services.Interfaces;
using Infrastructure.Lib.Utility;

namespace PublishOrderBuildedEventToolkit
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IOrderService orderService = new OrderService();
            while (true)
            {
                Console.WriteLine("==================================");
                string input = Console.ReadLine().ToStringIncludeNull();
                if (input.ToUpper() == "EXIST" || input.Length != 32)
                {
                    break;
                }

                orderService.PublishOrderBuildedEvent(input).Wait();
            }
            Console.WriteLine("Existing.............");
            Console.ReadLine();
        }
    }
}