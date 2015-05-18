// FileInformation: nyanya/Cat.Events.Products/BAProductLaunched.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Products
{
    public class BAProductLaunched : Event
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BAProductLaunched" /> class.
        ///     Only for Serialization
        /// </summary>
        public BAProductLaunched()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        /// <param name="productIdentifier">The product identifier.</param>
        /// <param name="sourceType">Type of the source.</param>
        public BAProductLaunched(string productIdentifier, Type sourceType)
            : base(productIdentifier, sourceType)
        {
            this.ProductIdentifier = productIdentifier;
        }

        public string ProductIdentifier { get; set; }
    }

    public class BAProductLaunchedValidator : AbstractValidator<BAProductLaunched>
    {
        public BAProductLaunchedValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}