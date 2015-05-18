// FileInformation: nyanya/Cat.Events.Products/ProductSoldOut.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

using System;
using Domian.Events;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Products
{
    /// <summary>
    ///     ProductSoldOut
    /// </summary>
    [Route("/ProductSoldOut")]
    public class ProductSoldOut : Event
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductSoldOut" /> class.
        ///     Only for Serialization
        /// </summary>
        public ProductSoldOut()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        /// <param name="productIdentifier">The product identifier.</param>
        /// <param name="sourceType">Type of the source.</param>
        public ProductSoldOut(string productIdentifier, Type sourceType)
            : base(productIdentifier, sourceType)
        {
            this.ProductIdentifier = productIdentifier;
        }

        public string ProductIdentifier { get; set; }
    }

    public class ProductSoldOutValidator : AbstractValidator<ProductSoldOut>
    {
        public ProductSoldOutValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}