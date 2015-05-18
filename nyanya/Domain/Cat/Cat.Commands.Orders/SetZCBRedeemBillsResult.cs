using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Commands.Orders
{
    /// <summary>
    /// 生成资产包提现结果请求类
    /// </summary>
    [Route("/SetZCBRedeemBillsResult")]
    public class SetZCBRedeemBillsResult : Command
    {        
        /// <summary>
        ///     初始化<see cref="SetZCBRedeemBillsResult" />类的新实例.
        /// </summary>
        public SetZCBRedeemBillsResult()
        {
        }

        /// <summary>
        ///     初始化<see cref="SetZCBRedeemBillsResult" />类的新实例.
        /// </summary>
        /// <param name="snList">需要处理的提现流水号集合</param>
        public SetZCBRedeemBillsResult(IList<string> snList)
            : base("RedeemBills")
        {
            SNList = snList;
        }

        /// <summary>
        /// 提现流水号集合
        /// </summary>
        public IList<string> SNList { get; set; }
    }

    /// <summary>
    /// 生成资产包提现结果（验证）
    /// </summary>
    public class SetZCBRedeemBillsResultValidator : AbstractValidator<SetZCBRedeemBillsResult>
    {
        /// <summary>
        ///     初始化<see cref="SetZCBRedeemBillsResultValidator" />类的新实例.
        /// </summary>
        public SetZCBRedeemBillsResultValidator()
        {
            this.RuleFor(c => c.SNList).NotNull();
        }
    }
}