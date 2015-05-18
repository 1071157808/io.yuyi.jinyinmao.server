// FileInformation: nyanya/Cqrs.Domain.Product/BAProductInfoService.cs
// CreatedTime: 2014/07/27   9:04 PM
// LastUpdatedTime: 2014/07/28   2:12 AM

using Xingye.Domain.Products.ReadModels;
using Xingye.Domain.Products.Services.Interfaces;

namespace Xingye.Domain.Products.Services
{
    public class BAProductInfoService : ProductInfoServiceBase<BAProductInfo>, IExactBAProductInfoService
    {
    }
}