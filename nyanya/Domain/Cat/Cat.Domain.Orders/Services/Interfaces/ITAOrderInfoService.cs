// FileInformation: nyanya/Cqrs.Domain.Order/ITAOrderInfoService.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/14   7:53 AM

using Cat.Domain.Orders.ReadModels;

namespace Cat.Domain.Orders.Services.Interfaces
{
    public interface ITAOrderInfoService : IOrderInfoService<TAOrderInfo>
    {
    }
}