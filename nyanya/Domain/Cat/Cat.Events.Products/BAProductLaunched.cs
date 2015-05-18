// FileInformation: nyanya/Cat.Events.Products/BAProductLaunched.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Products
{
    /// <summary>
    ///     银票产品上架
    /// </summary>
    public class BAProductLaunched : Event
    {
        /// <summary>
        ///      初始化<see cref="BAProductLaunched" />类的新实例. 
        /// </summary>
        public BAProductLaunched()
        {
        }

        /// <summary>
        ///     初始化<see cref="BAProductLaunched" />类的新实例. 
        /// </summary>
        /// <param name="productIdentifier">产品标识</param>
        /// <param name="sourceType">源类型</param>
        public BAProductLaunched(string productIdentifier, Type sourceType)
            : base(productIdentifier, sourceType)
        {
            this.ProductIdentifier = productIdentifier;
        }
        /// <summary>
        /// 产品标识
        /// </summary>
        public string ProductIdentifier { get; set; }
    }
    /// <summary>
    ///     银票产品上架（验证）
    /// </summary>
    public class BAProductLaunchedValidator : AbstractValidator<BAProductLaunched>
    {
        /// <summary>
        ///     初始化<see cref="BAProductLaunchedValidator" />类的新实例. 
        /// </summary>
        public BAProductLaunchedValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}