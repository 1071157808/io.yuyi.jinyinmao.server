// FileInformation: nyanya/Cat.Commands.Products/ProductRepay.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:21 PM

using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Cat.Commands.Products
{
    /// <summary>
    /// 产品还款请求类
    /// </summary>
    [Route("/ProductRepay")]
    public class ProductRepay : Command
    {
        /// <summary>
        ///     初始化<see cref="ProductRepay" />类的新实例.
        /// </summary>
        public ProductRepay()
        {
        }

        /// <summary>
        ///     初始化<see cref="ProductRepay" />类的新实例.
        /// </summary>
        /// <param name="productIdentifier">产品唯一标示符</param>
        public ProductRepay(string productIdentifier)
            : base("JB")
        {
            this.ProductIdentifier = productIdentifier;
        }

        /// <summary>
        /// 产品唯一标示符
        /// </summary>
        public string ProductIdentifier { get; set; }
    }

    /// <summary>
    /// 产品还款（验证）
    /// </summary>
    public class ProductRepayValidator : AbstractValidator<ProductRepay>
    {
        /// <summary>
        ///     初始化<see cref="ProductRepayValidator" />类的新实例.
        /// </summary>
        public ProductRepayValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}