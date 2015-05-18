using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Orders
{
    /// <summary>
    ///     提现本金
    /// </summary>
    public class RedeemPrincipaled : Event
    {
        /// <summary>
        ///     初始化<see cref="RedeemPrincipaled" />类的新实例. 
        /// </summary>
        public RedeemPrincipaled()
        {
        }

        /// <summary>
        ///     初始化<see cref="RedeemPrincipaled" />类的新实例. 
        /// </summary>
        /// <param name="orderIdentifier">订单标识</param>
        /// <param name="sourceType">源类型</param>
        public RedeemPrincipaled(string orderIdentifier, Type sourceType)
            : base(orderIdentifier, sourceType)
        {
            this.OrderIdentifier = orderIdentifier;
        }

        /// <summary>
        ///     订单标识
        /// </summary>
        public string OrderIdentifier { get; set; }
    }
    /// <summary>
    ///     提现本金(验证)
    /// </summary>
    public class RedeemPrincipaledValidator : AbstractValidator<RedeemPrincipaled>
    {
        /// <summary>
        ///     初始化<see cref="RedeemPrincipaledValidator" />类的新实例.
        /// </summary>
        public RedeemPrincipaledValidator()
        {
            this.RuleFor(c => c.OrderIdentifier).NotNull();
            this.RuleFor(c => c.OrderIdentifier).NotEmpty();
        }
    }
}
