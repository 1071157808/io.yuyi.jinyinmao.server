// FileInformation: nyanya/Cat.Events.Products/ProductRepaid.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Products
{
    public class ProductRepaid : Event
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductRepaid" /> class.
        ///     Only for Serialization
        /// </summary>
        public ProductRepaid()
        {
        }

        public ProductRepaid(string productIdentifier, Type sourceType)
            : base(productIdentifier, sourceType)
        {
            this.ProductIdentifier = productIdentifier;
        }

        public string ProductIdentifier { get; set; }
    }

    public class ProductRepaidValidator : AbstractValidator<ProductRepaid>
    {
        public ProductRepaidValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}