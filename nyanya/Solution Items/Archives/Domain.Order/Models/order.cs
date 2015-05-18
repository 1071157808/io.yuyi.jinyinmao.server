// FileInformation: nyanya/Domain.Order/Order.cs
// CreatedTime: 2014/04/01   2:47 PM
// LastUpdatedTime: 2014/04/22   3:14 PM

using System;

namespace Domain.Order.Models
{
    public enum OrderStatus
    {
        /// <summary>
        ///     �ȴ�����
        /// </summary>
        Unpaid = 10,

        /// <summary>
        ///     ����ɹ�
        /// </summary>
        Paid = 20,

        /// <summary>
        ///     ��ȡ��
        /// </summary>
        Canceled = 30,

        /// <summary>
        ///     �˿�
        /// </summary>
        Refunded = 40,

        /// <summary>
        ///     ��ʼ��Ϣ
        /// </summary>
        InterestAccruing = 50,

        /// <summary>
        ///     �ѽ�Ϣ
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