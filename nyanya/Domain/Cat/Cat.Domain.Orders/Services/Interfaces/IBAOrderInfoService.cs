// FileInformation: nyanya/Cqrs.Domain.Order/IBAOrderInfoService.cs
// CreatedTime: 2014/08/09   3:16 PM
// LastUpdatedTime: 2014/08/09   3:18 PM

using Cat.Domain.Orders.ReadModels;

namespace Cat.Domain.Orders.Services.Interfaces
{
    public interface IBAOrderInfoService : IOrderInfoService<BAOrderInfo>
    {
    }
}