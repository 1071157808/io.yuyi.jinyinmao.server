using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;

namespace Cat.Commands.Orders
{
    /// <summary>
    /// 生成提现请求类
    /// </summary>
    [Route("/BuildRedeemPrincipal")]
    public class BuildRedeemPrincipal : Command
    {
        /// <summary>
        ///     初始化<see cref="BuildRedeemPrincipal" />类的新实例.
        /// </summary>
        public BuildRedeemPrincipal()
        {
        }

        /// <summary>
        ///     初始化<see cref="BuildRedeemPrincipal" />类的新实例.
        /// </summary>
        /// <param name="userIdentifier">用户唯一标示符</param>
        public BuildRedeemPrincipal(string userIdentifier)
            : base("USER_" + userIdentifier)
        {
            this.UserIdentifier = userIdentifier;
        }

        /// <summary>
        ///   银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     用户唯一标示符
        /// </summary>
        public string UserIdentifier { get; set; }

        /// <summary>
        ///     取回金额
        /// </summary>
        public decimal RedeemPrincipal { get; set; }

        /// <summary>
        ///     是否最后一笔取回金额
        /// </summary>
        public bool FinallyRedeem { get; set; }

        /// <summary>
        ///     订单所属产品编号
        /// </summary>
        public string ProductIdentifier { get; set; }
    }

    /// <summary>
    /// 提现请求（验证）
    /// </summary>
    public class BuildRedeemPrincipalValidator : AbstractValidator<BuildRedeemPrincipal>
    {
        /// <summary>
        ///     初始化<see cref="BuildRedeemPrincipalValidator" />类的新实例.
        /// </summary>
        public BuildRedeemPrincipalValidator()
        {
            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();

            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}