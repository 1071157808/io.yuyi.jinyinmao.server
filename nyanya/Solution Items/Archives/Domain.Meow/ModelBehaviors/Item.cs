// FileInformation: nyanya/Domain.Meow/Item.cs
// CreatedTime: 2014/04/22   3:09 PM
// LastUpdatedTime: 2014/05/07   4:09 PM

using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Order.Models;
using Domain.Order.Services.Interfaces;

namespace Domain.Meow.Models
{
    public static class ItemFactory<T> where T : Item, new()
    {
        public static T CreateItem()
        {
            T t = new T();
            t.IsUsed = false;
            t.ReceiveTime = DateTime.Now;
            if (typeof(T) == typeof(OHPItem))
            {
                t.CategoryId = 101;
            }
            return t;
        }
    }

    public partial class Item : IAvailability
    {
        #region IAvailability Members

        public virtual bool Available(OrderListItem orderContext)
        {
            return !this.IsUsed && this.Expires > DateTime.Now;
        }

        public virtual bool Available(IOrder orderContext)
        {
            return !this.IsUsed && this.Expires > DateTime.Now;
        }

        public bool Available(Order.Models.Order orderContext)
        {
            return !this.IsUsed && this.Expires > DateTime.Now;
        }

        #endregion IAvailability Members

        public static bool AvailableForOrder(IEnumerable<Item> items, Order.Models.Order orderContext)
        {
            List<Type> categories = items.Where(i => !i.IsUsed && i.Expires < DateTime.Now && i.ForOrder()).Select(i => i.GetConcreteType()).Distinct().ToList();

            return categories.Any(category => ((dynamic)categories).AvailableForOrder(orderContext));
        }

        protected bool ForOrder()
        {
            return this.CategoryId > 100 && this.CategoryId <= 199;
        }

        protected Type GetConcreteType()
        {
            switch (this.CategoryId)
            {
                case 101:
                    return typeof(OHPItem);

                default:
                    return null;
            }
        }
    }
}