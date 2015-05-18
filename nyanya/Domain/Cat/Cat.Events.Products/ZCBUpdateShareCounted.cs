using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Products
{
    /// <summary>
    ///     资产包产品更新份额
    /// </summary>
    public class ZCBUpdateShareCounted : Event
    {
        /// <summary>
        ///     初始化<see cref="ZCBUpdateShareCounted"/>类的新实例.
        /// </summary>
        public ZCBUpdateShareCounted()
        {
        }
        /// <summary>
        ///     初始化<see cref="ZCBUpdateShareCounted"/>类的新实例.
        /// </summary>
        /// <param name="productIdentifier">产品标识</param>
        /// <param name="sourceType">源类型</param>
        public ZCBUpdateShareCounted(string productIdentifier, Type sourceType)
            : base(productIdentifier, sourceType)
        {
            this.ProductIdentifier = productIdentifier;
        }
        /// <summary>
        ///     产品标识
        /// </summary>
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     类型（0添加 1更新）
        /// </summary>
        public int Type { get; set; }
    }
    /// <summary>
    ///     资产包产品更新份额（验证）
    /// </summary>
    public class ZCBUpdateShareCountedValidator : AbstractValidator<ZCBUpdateShareCounted>
    {
        /// <summary>
        ///     初始化<see cref="ZCBUpdateShareCountedValidator"/>类的新实例.
        /// </summary>
        public ZCBUpdateShareCountedValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}
