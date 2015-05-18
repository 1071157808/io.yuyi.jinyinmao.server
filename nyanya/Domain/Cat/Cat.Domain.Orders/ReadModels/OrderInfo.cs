// FileInformation: nyanya/Cqrs.Domain.Order/OrderInfo.cs
// CreatedTime: 2014/08/08   7:00 PM
// LastUpdatedTime: 2014/08/09   4:34 PM

using System;
using Cat.Commands.Orders;
using Cat.Commands.Users;
using Domian.Models;
using Cat.Commands.Products;

namespace Cat.Domain.Orders.ReadModels
{
    public class OrderInfo : IReadModel
    {
        public string BankCardCity { get; set; }

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string ConsignmentAgreementName { get; set; }

        public string EndorseImageLink { get; set; }

        public string EndorseImageThumbnailLink { get; set; }

        public decimal ExtraInterest { get; set; }

        public bool HasResult { get; set; }

        public int Id { get; set; }

        public decimal Interest { get; set; }

        public string InvestorCellphone { get; set; }

        public Credential InvestorCredential { get; set; }

        public string InvestorCredentialNo { get; set; }

        public string InvestorRealName { get; set; }

        public bool IsPaid { get; set; }

        public string OrderIdentifier { get; set; }

        public string OrderNo { get; set; }

        public DateTime OrderTime { get; set; }

        public OrderType OrderType { get; set; }

        public string PaymentInfoTransDesc { get; set; }

        public string PledgeAgreementName { get; set; }

        public decimal Principal { get; set; }

        public string ProductIdentifier { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public int ProductNumber { get; set; }

        public decimal ProductUnitPrice { get; set; }

        public DateTime RepaymentDeadline { get; set; }

        public DateTime? ResultTime { get; set; }

        public DateTime SettleDate { get; set; }

        public int ShareCount { get; set; }

        public decimal TotalAmount { get; set; }

        public string UserIdentifier { get; set; }

        public DateTime ValueDate { get; set; }

        public decimal Yield { get; set; }

        /// <summary>
        /// 产品分类 （10金银猫 20富滇 40阜新产品）
        /// </summary>
        public ProductCategory ProductCategory { get; set; }

        public bool IsRepaid { get; set; }
    }
}