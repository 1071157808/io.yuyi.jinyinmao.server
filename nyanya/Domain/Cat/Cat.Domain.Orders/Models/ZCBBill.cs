using Cat.Commands.Orders;
using Domian.Models;
using System;

namespace Cat.Domain.Orders.Models
{
    /// <summary>
    ///
    /// </summary>
    public partial class ZCBBill : IReadModel
    {
        public ZCBBill(string sn)
        {
            this.SN = sn;
        }

        public ZCBBill()
        {
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the sn.
        /// </summary>
        /// <value>
        /// The sn.
        /// </value>
        public string SN { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        public string OrderIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the type. 10=>订单 20=>取回
        /// </summary>
        /// <value>
        /// The type. 10=>订单 20=>取回
        /// </value>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>
        /// The create time.
        /// </value>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Gets or sets the result time.
        /// </summary>
        /// <value>
        /// The result time.
        /// </value>
        public DateTime? ResultTime { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the redeem interest.
        /// </summary>
        /// <value>
        /// The redeem interest.
        /// </value>
        public decimal RedeemInterest { get; set; }

        /// <summary>
        /// Gets or sets the principal.
        /// </summary>
        /// <value>
        /// The principal.
        /// </value>
        public decimal Principal { get; set; }

        /// <summary>
        /// Gets or sets the bank card no.
        /// </summary>
        /// <value>
        /// The bank card no.
        /// </value>
        public string BankCardNo { get; set; }

        /// <summary>
        /// Gets or sets the name of the bank.
        /// </summary>
        /// <value>
        /// The name of the bank.
        /// </value>
        public string BankName { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public ZCBBillStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the remark.
        /// </summary>
        /// <value>
        /// The remark. 10=>付款中 ，20 =>付款成功, 30=>付款失败,40=>取款中,50=>取款成功,60=>取款失败
        /// </value>
        public string Remark { get; set; }

        /// <summary>
        /// Gets or sets the delay date.
        /// </summary>
        /// <value>
        /// The delay date.
        /// </value>
        public DateTime? DelayDate { get; set; }

        /// <summary>
        ///     协议名称
        /// </summary>
        public string AgreementName { get; set; }
    }
}