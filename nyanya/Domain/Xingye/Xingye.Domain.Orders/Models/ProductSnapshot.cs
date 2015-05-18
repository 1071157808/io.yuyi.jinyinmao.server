// FileInformation: nyanya/Cqrs.Domain.Order/BAProductSnapshot.cs
// CreatedTime: 2014/08/07   6:16 PM
// LastUpdatedTime: 2014/08/07   6:18 PM

namespace Xingye.Domain.Orders.Models
{
    public class ProductSnapshot
    {
        public string OrderIdentifier { get; set; }

        public string Snapshot { get; set; }
    }
}