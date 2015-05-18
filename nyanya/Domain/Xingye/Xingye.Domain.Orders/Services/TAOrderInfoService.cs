// FileInformation: nyanya/Cqrs.Domain.Order/TAOrderInfoService.cs
// CreatedTime: 2014/08/09   3:20 PM
// LastUpdatedTime: 2014/08/09   3:21 PM

using Xingye.Domain.Orders.ReadModels;
using Xingye.Domain.Orders.Services.Interfaces;

namespace Xingye.Domain.Orders.Services
{
    public class TAOrderInfoService : OrderInfoServiceBase<TAOrderInfo>, ITAOrderInfoService
    {
    }
}