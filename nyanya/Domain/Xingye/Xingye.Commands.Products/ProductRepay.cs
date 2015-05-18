// FileInformation: nyanya/Xingye.Commands.Products/ProductRepay.cs
// CreatedTime: 2014/09/01   5:52 PM
// LastUpdatedTime: 2014/09/02   10:04 AM

using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Xingye.Commands.Products
{
    [Route("/ProductRepay")]
    public class ProductRepay : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductRepay" /> class.
        ///     Only for Serialization
        /// </summary>
        public ProductRepay()
        {
        }

        public ProductRepay(string productIdentifier)
            : base("JB")
        {
            this.ProductIdentifier = productIdentifier;
        }

        public string ProductIdentifier { get; set; }
    }

    public class ProductRepayValidator : AbstractValidator<ProductRepay>
    {
        public ProductRepayValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}