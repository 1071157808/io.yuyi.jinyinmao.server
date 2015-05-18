// FileInformation: nyanya/Cat.Events.Products/TAProductLaunched.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Products
{
    /// <summary>
    ///     商票产品上架
    /// </summary>
    public class TAProductLaunched : Event
    {
        /// <summary>
        ///     初始化<see cref="TAProductLaunched" />类的新实例. 
        /// </summary>
        public TAProductLaunched()
        {
        }

        /// <summary>
        ///     初始化<see cref="TAProductLaunched" />类的新实例. 
        /// </summary>
        /// <param name="productIdentifier">产品标识</param>
        /// <param name="sourceType">源类型</param>
        public TAProductLaunched(string productIdentifier, Type sourceType)
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
    ///     商票产品上架（验证）
    /// </summary>
    public class TAProductLaunchedValidator : AbstractValidator<TAProductLaunched>
    {
        /// <summary>
        ///     初始化<see cref="TAProductLaunchedValidator" />类的新实例. 
        /// </summary>
        public TAProductLaunchedValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}