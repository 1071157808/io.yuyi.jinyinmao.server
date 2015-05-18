// FileInformation: nyanya/Cqrs.Domain.Product/TAProductInfoService.cs
// CreatedTime: 2014/07/27   9:04 PM
// LastUpdatedTime: 2014/07/28   2:09 AM

using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.Interfaces;

namespace Cat.Domain.Products.Services
{
    public class TAProductInfoService : ProductInfoServiceBase<TAProductInfo>, IExactTAProductInfoService
    {
    }
}