// FileInformation: nyanya/Cat.Events.Products/BAProductUnShelved.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Products
{
    public class BAProductUnShelved : Event
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BAProductUnShelved" /> class.
        ///     Only for Serialization
        /// </summary>
        public BAProductUnShelved()
        {
        }

        public BAProductUnShelved(string productIdentifier, Type sourceType)
            : base(productIdentifier, sourceType)
        {
            this.ProductIdentifier = productIdentifier;
        }

        public string ProductIdentifier { get; set; }
    }

    public class BAProductUnShelvedValidator : AbstractValidator<BAProductUnShelved>
    {
        public BAProductUnShelvedValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}