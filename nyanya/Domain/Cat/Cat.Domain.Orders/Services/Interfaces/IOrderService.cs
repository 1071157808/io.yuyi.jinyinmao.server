// FileInformation: nyanya/Cqrs.Domain.Order/IOrderService.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/29   10:45 AM

using System.Threading.Tasks;

namespace Cat.Domain.Orders.Services.Interfaces
{
    public interface IOrderService
    {
        Task PublishOrderBuildedEvent(string orderIdentifier);

        Task SetOrdersRepaidForProductAsync(string productIdentifier);

        Task SetOrderInterestAsync(string orderIdentifier,decimal interest);
    }
}