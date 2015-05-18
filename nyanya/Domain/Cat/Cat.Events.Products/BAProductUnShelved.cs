// FileInformation: nyanya/Cat.Events.Products/BAProductUnShelved.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Products
{
    /// <summary>
    ///     银票产品下架
    /// </summary>
    public class BAProductUnShelved : Event
    {
        /// <summary>
        ///     初始化<see cref="BAProductUnShelved" />类的新实例. 
        /// </summary>
        public BAProductUnShelved()
        {
        }
        /// <summary>
        ///     初始化<see cref="BAProductUnShelved" />类的新实例. 
        /// </summary>
        /// <param name="productIdentifier">产品标识</param>
        /// <param name="sourceType">源类型</param>
        public BAProductUnShelved(string productIdentifier, Type sourceType)
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
    ///     银票产品下架（验证）
    /// </summary>
    public class BAProductUnShelvedValidator : AbstractValidator<BAProductUnShelved>
    {
        public BAProductUnShelvedValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}