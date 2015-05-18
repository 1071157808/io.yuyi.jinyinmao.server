// FileInformation: nyanya/Cqrs.Domain.Order/BAOrderInfoService.cs
// CreatedTime: 2014/08/09   3:19 PM
// LastUpdatedTime: 2014/08/09   3:20 PM

using Cat.Domain.Orders.ReadModels;
using Cat.Domain.Orders.Services.Interfaces;

namespace Cat.Domain.Orders.Services
{
    public class BAOrderInfoService : OrderInfoServiceBase<BAOrderInfo>, IBAOrderInfoService
    {
    }
}