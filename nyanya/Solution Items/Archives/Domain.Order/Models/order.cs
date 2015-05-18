// FileInformation: nyanya/Domain.Order/Order.cs
// CreatedTime: 2014/04/01   2:47 PM
// LastUpdatedTime: 2014/04/22   3:14 PM

using System;

namespace Domain.Order.Models
{
    public enum OrderStatus
    {
        /// <summary>
        ///     等待付款
        /// </summary>
        Unpaid = 10,

        /// <summary>
        ///     付款成功
        /// </summary>
        Paid = 20,

        /// <summary>
        ///     已取消
        /// </summary>
        Canceled = 30,

        /// <summary>
        ///     退款
        /// </summary>
        Refunded = 40,

        /// <summary>
        ///     开始起息
        /// </summary>
        InterestAccruing = 50,

        /// <summary>
        ///     已结息
        /// </summary>
        InterestSettled = 60
    }

    public class Order
    {
        public virtual DateTime CreatedAt { get; set; }

        public virtual Nullable<decimal> ExpectedPrice { get; set; }

        public virtual int Id { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual Nullable<decimal> Price { get; set; }

        public virtual OrderStatus Status { get; set; }

        public virtual DateTime UpdatedAt { get; set; }

        public virtual int UserId { get; set; }

        public virtual string UserUuid { get; set; }
    }
}