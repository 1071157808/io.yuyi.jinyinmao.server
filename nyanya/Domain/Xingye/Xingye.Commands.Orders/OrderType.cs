// FileInformation: nyanya/XingyeCommands.Orders/OrderType.cs
// CreatedTime: 2014/09/01   5:52 PM
// LastUpdatedTime: 2014/09/01   6:20 PM

using Xingye.Commands.Products;

namespace Xingye.Commands.Orders
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 银承
        /// </summary>
        BankAcceptance = 10,

        /// <summary>
        /// 商承
        /// </summary>
        TradeAcceptance = 20
    }

    public static class OrderTypeExtensions
    {
        public static OrderType ToOrderType(this ProductType productType)
        {
            return productType == ProductType.TradeAcceptance ? OrderType.TradeAcceptance : OrderType.BankAcceptance;
        }

        public static ProductType ToProductType(this OrderType orderType)
        {
            return orderType == OrderType.TradeAcceptance ? ProductType.TradeAcceptance : ProductType.BankAcceptance;
        }
    }
}