
using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Products
{
    /// <summary>
    ///     资产包产品上架
    /// </summary>
    public class ZCBProductLaunched : Event
    {
        /// <summary>
        ///     初始化<see cref="ZCBProductLaunched" />类的新实例. 
        /// </summary>
        public ZCBProductLaunched()
        {
        }

        /// <summary>
        ///     初始化<see cref="ZCBProductLaunched" />类的新实例. 
        /// </summary>
        /// <param name="productIdentifier">产品标识</param>
        /// <param name="sourceType">源类型</param>
        public ZCBProductLaunched(string productIdentifier, Type sourceType)
            : base(productIdentifier, sourceType)
        {
            this.ProductIdentifier = productIdentifier;
        }
        /// <summary>
        ///     产品标识
        /// </summary>
        public string ProductIdentifier { get; set; }
    }
    /// <summary>
    ///     资产包产品上架（验证）
    /// </summary>
    public class ZCBProductLaunchedValidator : AbstractValidator<ZCBProductLaunched>
    {
        /// <summary>
        ///     初始化<see cref="ZCBProductLaunchedValidator"/>类的新实例.
        /// </summary>
        public ZCBProductLaunchedValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}
