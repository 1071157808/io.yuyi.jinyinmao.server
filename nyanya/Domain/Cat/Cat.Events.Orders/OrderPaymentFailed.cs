// FileInformation: nyanya/Cat.Events.Orders/OrderPaymentFailed.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System;
using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;

namespace Cat.Events.Orders
{
    /// <summary>
    ///     订单支付失败
    /// </summary>
    public class OrderPaymentFailed : Event
    {
        /// <summary>
        ///     初始化<see cref="OrderPaymentFailed" />类的新实例. 
        /// </summary>
        /// <param name="orderIdentifier">订单标识</param>
        /// <param name="sourceType">源类型</param>
        public OrderPaymentFailed(string orderIdentifier, Type sourceType)
            : base(orderIdentifier, sourceType)
        {
            this.OrderIdentifier = orderIdentifier;
        }

        /// <summary>
        ///     初始化<see cref="OrderPaymentFailed" />类的新实例. 
        /// </summary>
        public OrderPaymentFailed()
        {
        }
        /// <summary>
        ///     订单金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///     银行卡号
        /// </summary>
        public string BankCardNo { get; set; }
        /// <summary>
        ///     银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        ///     手机号
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        ///     银行开户城市名
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        ///     证件类型
        /// </summary>
        public int CredentialCode { get; set; }
        /// <summary>
        ///     证件号
        /// </summary>
        public string CredentialNo { get; set; }
        /// <summary>
        ///     订单支付失败消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        ///     订单标识
        /// </summary>
        public string OrderIdentifier { get; set; }
        /// <summary>
        ///     订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        ///     产品标识
        /// </summary>
        public string ProductIdentifier { get; set; }
        /// <summary>
        ///     产品编号
        /// </summary>
        public string ProductNo { get; set; }
        /// <summary>
        ///     用户姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        ///     订单份额
        /// </summary>
        public int ShareCount { get; set; }
        /// <summary>
        ///     用户标识
        /// </summary>
        public string UserIdentifier { get; set; }
    }
    /// <summary>
    ///     订单支付失败(验证)
    /// </summary>
    public class OrderPaymentFailedValidator : AbstractValidator<OrderPaymentFailed>
    {
        /// <summary>
        ///     初始化<see cref="OrderPaymentFailedValidator" />类的新实例.
        /// </summary>
        public OrderPaymentFailedValidator()
        {
            this.RuleFor(c => c.Amount).GreaterThan(new decimal(0));

            this.RuleFor(c => c.BankCardNo).NotNull();
            this.RuleFor(c => c.BankCardNo).NotEmpty();

            this.RuleFor(c => c.BankName).NotNull();
            this.RuleFor(c => c.BankName).NotEmpty();

            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();
            this.RuleFor(c => c.Cellphone).Matches(RegexUtils.CellphoneRegex.ToString());

            this.RuleFor(c => c.CityName).NotNull();
            this.RuleFor(c => c.CityName).NotEmpty();

            this.RuleFor(c => c.RealName).NotNull();
            this.RuleFor(c => c.RealName).NotEmpty();

            this.RuleFor(c => c.CredentialNo).NotNull();
            this.RuleFor(c => c.CredentialNo).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();

            this.RuleFor(c => c.OrderIdentifier).NotNull();
            this.RuleFor(c => c.OrderIdentifier).NotEmpty();

            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();

            this.RuleFor(c => c.ProductNo).NotNull();
            this.RuleFor(c => c.ProductNo).NotEmpty();

            this.RuleFor(c => c.OrderNo).NotNull();
            this.RuleFor(c => c.OrderNo).NotEmpty();
        }
    }
}