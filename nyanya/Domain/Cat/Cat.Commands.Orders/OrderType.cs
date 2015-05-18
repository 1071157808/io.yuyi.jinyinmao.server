// FileInformation: nyanya/Cat.Commands.Orders/OrderType.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:49 PM

using Cat.Commands.Products;

namespace Cat.Commands.Orders
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 银票
        /// </summary>
        BankAcceptance = 10,
        /// <summary>
        /// 商票
        /// </summary>
        TradeAcceptance = 20,
        /// <summary>
        /// 资产包
        /// </summary>
        ZCBAcceptance = 30
    }

    /// <summary>
    /// 订单类型扩展类
    /// </summary>
    public static class OrderTypeExtensions
    {
        /// <summary>
        /// 根据产品类型返回订单类型
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>订单类型</returns>
        public static OrderType ToOrderType(this ProductType productType)
        {
            return productType == ProductType.TradeAcceptance ? OrderType.TradeAcceptance : productType == ProductType.ZCBAcceptance ? OrderType.ZCBAcceptance : OrderType.BankAcceptance;
        }

        /// <summary>
        /// 根据订单类型返回产品类型
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <returns>产品类型</returns>
        public static ProductType ToProductType(this OrderType orderType)
        {
            return orderType == OrderType.TradeAcceptance ? ProductType.TradeAcceptance : orderType == OrderType.ZCBAcceptance ? ProductType.ZCBAcceptance : ProductType.BankAcceptance;
        }
    }

    /// <summary>
    ///     认购/提现状态
    ///     10付款中 20认购成功 30认购失败 40取现已申请 50取现成功 60提现失败
    /// </summary>
    public enum ZCBBillStatus
    {
        /// <summary>
        /// 付款中
        /// </summary>
        InvestPaying = 10,
        /// <summary>
        /// 认购成功
        /// </summary>
        InvestSuccess = 20,
        /// <summary>
        /// 认购失败
        /// </summary>
        InvestFailed = 30,
        /// <summary>
        /// 提现已申请
        /// </summary>
        RedeemApplication = 40,
        /// <summary>
        /// 提现成功
        /// </summary>
        RedeemSuccess = 50,
        /// <summary>
        /// 提现失败
        /// </summary>
        RedeemFailed = 60
    }

    /// <summary>
    /// 认购/提现状态扩展类
    /// </summary>
    public static class ZCBBillStatusExtensions
    {
        /// <summary>
        /// 根据认购/提现状态返回状态字符串
        /// </summary>
        /// <param name="zcbBillStatus"></param>
        /// <returns></returns>
        public static string ToZCBBillStatusString(this ZCBBillStatus zcbBillStatus)
        {
            switch (zcbBillStatus)
            {
                case ZCBBillStatus.InvestPaying:
                    return "付款中";
                case ZCBBillStatus.InvestSuccess:
                    return "认购成功";
                case ZCBBillStatus.InvestFailed:
                    return "认购失败";
                case ZCBBillStatus.RedeemApplication:
                    return "提现已申请";
                case ZCBBillStatus.RedeemSuccess:
                    return "提现成功"; 
                case ZCBBillStatus.RedeemFailed:
                    return "提现失败";
            }
            return "";
        }
    }
}