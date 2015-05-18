// FileInformation: nyanya/Cqrs.Domain.Order/BAOrderInfoService.cs
// CreatedTime: 2014/08/09   3:19 PM
// LastUpdatedTime: 2014/08/09   3:20 PM

using Xingye.Domain.Orders.ReadModels;
using Xingye.Domain.Orders.Services.Interfaces;

namespace Xingye.Domain.Orders.Services
{
    public class BAOrderInfoService : OrderInfoServiceBase<BAOrderInfo>, IBAOrderInfoService
    {
    }
}