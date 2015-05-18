// FileInformation: nyanya/Domain.Meow/IAvailability.cs
// CreatedTime: 2014/04/22   3:07 PM
// LastUpdatedTime: 2014/05/07   4:09 PM

using Domain.Order.Models;
using Domain.Order.Services.Interfaces;

namespace Domain.Meow.Models
{
    public interface IAvailability
    {
        bool Available(OrderListItem orderContext);

        bool Available(Order.Models.Order orderContext);

        bool Available(IOrder orderContext);
    }
}