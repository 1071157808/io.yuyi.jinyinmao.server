// FileInformation: nyanya/Cqrs.Domain.Order/TAOrderInfoService.cs
// CreatedTime: 2014/08/09   3:20 PM
// LastUpdatedTime: 2014/08/09   3:21 PM

using Cat.Domain.Orders.ReadModels;
using Cat.Domain.Orders.Services.Interfaces;

namespace Cat.Domain.Orders.Services
{
    public class TAOrderInfoService : OrderInfoServiceBase<TAOrderInfo>, ITAOrderInfoService
    {
    }
}