// FileInformation: nyanya/Xingye.Domain.Products/Agreement.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:25 PM

using Domian.Models;

namespace Xingye.Domain.Products.Models
{
    public class Agreement : IValueObject
    {
        public string Content { get; set; }

        public int Id { get; set; }
    }
}