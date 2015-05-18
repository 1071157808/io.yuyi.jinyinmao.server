// FileInformation: nyanya/Xingye.Commands.Products/BAProductUnShelves.cs
// CreatedTime: 2014/09/01   5:52 PM
// LastUpdatedTime: 2014/09/02   10:04 AM

using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Xingye.Commands.Products
{
    [Route("/BA/UnShelves")]
    public class BAProductUnShelves : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BAProductUnShelves" /> class.
        ///     Only for Serialization
        /// </summary>
        public BAProductUnShelves()
        {
        }

        public BAProductUnShelves(string productIdentifier)
            : base("JB")
        {
            this.ProductIdentifier = productIdentifier;
        }

        public string ProductIdentifier { get; set; }
    }

    public class BAProductUnShelvesValidator : AbstractValidator<BAProductUnShelves>
    {
        public BAProductUnShelvesValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}