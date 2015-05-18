// FileInformation: nyanya/Cat.Commands.Products/BAProductUnShelves.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:23 PM

using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Cat.Commands.Products
{
    /// <summary>
    /// 银承产品下架请求类
    /// </summary>
    [Route("/BA/UnShelves")]
    public class BAProductUnShelves : Command
    {
        /// <summary>
        ///     初始化<see cref="BAProductUnShelves" />类的新实例.
        /// </summary>
        public BAProductUnShelves()
        {
        }

        /// <summary>
        ///     初始化<see cref="BAProductUnShelves" />类的新实例.
        /// </summary>
        /// <param name="productIdentifier">产品唯一标示符</param>
        public BAProductUnShelves(string productIdentifier)
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
    /// 银承产品下架（验证）
    /// </summary>
    public class BAProductUnShelvesValidator : AbstractValidator<BAProductUnShelves>
    {
        /// <summary>
        ///     初始化<see cref="BAProductUnShelvesValidator" />类的新实例.
        /// </summary>
        public BAProductUnShelvesValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}