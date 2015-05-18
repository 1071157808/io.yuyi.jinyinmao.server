// FileInformation: nyanya/Cat.Events.Orders/SetRedeemBillResult.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System;
using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;

namespace Cat.Events.Orders
{
    /// <summary>
    ///     取现成功
    /// </summary>
    public class SetRedeemBillResult : Event
    {
        /// <summary>
        ///     初始化<see cref="SetRedeemBillResult" />类的新实例. 
        /// </summary>
        /// <param name="orderIdentifier">订单标识</param>
        /// <param name="sourceType">源类型</param>
        public SetRedeemBillResult(string orderIdentifier, Type sourceType)
            : base(orderIdentifier, sourceType)
        {
            this.OrderIdentifier = orderIdentifier;
        }

        /// <summary>
        ///     初始化<see cref="SetRedeemBillResult" />类的新实例. 
        /// </summary>
        public SetRedeemBillResult()
        {
        }

        /// <summary>
        ///     订单标识
        /// </summary>
        public string OrderIdentifier { get; set; }

        /// <summary>
        ///     提现本金金额
        /// </summary>
        public decimal Principal { get; set; }

        /// <summary>
        ///     提现利息金额
        /// </summary>
        public decimal RedeemInterest { get; set; }
        
        /// <summary>
        ///     银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        ///     手机号码
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        ///     提现编号
        /// </summary>
        public string SN { get; set; }
    }
    /// <summary>
    ///     取现成功(验证)
    /// </summary>
    public class SetRedeemBillResultValidator : AbstractValidator<SetRedeemBillResult>
    {
        /// <summary>
        ///     初始化<see cref="SetRedeemBillResultValidator" />类的新实例.
        /// </summary>
        public SetRedeemBillResultValidator()
        {
            this.RuleFor(c => c.Principal).GreaterThan(new decimal(0));

            this.RuleFor(c => c.RedeemInterest).GreaterThanOrEqualTo(new decimal(0));

            this.RuleFor(c => c.BankCardNo).NotNull();
            this.RuleFor(c => c.BankCardNo).NotEmpty();

            this.RuleFor(c => c.BankName).NotNull();
            this.RuleFor(c => c.BankName).NotEmpty();

            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();
            this.RuleFor(c => c.Cellphone).Matches(RegexUtils.CellphoneRegex.ToString());

            this.RuleFor(c => c.OrderIdentifier).NotNull();
            this.RuleFor(c => c.OrderIdentifier).NotEmpty();

            this.RuleFor(c => c.SN).NotNull();
            this.RuleFor(c => c.SN).NotEmpty();
        }
    }
}