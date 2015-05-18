// FileInformation: nyanya/Cat.Domain.Products/ProductInfo.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:25 PM

using System;
using Cat.Commands.Products;

namespace Cat.Domain.Products.ReadModels
{
    public class ProductInfo
    {
        public int ConsignmentAgreementId { get; set; }

        public DateTime CurrentValueDate
        {
            get
            {
                if (this.ValueDateMode == ValueDateMode.T0)
                {
                    return DateTime.Today;
                }
                if (this.ValueDateMode == ValueDateMode.T1)
                {
                    return DateTime.Now.AddDays(1).Date;
                }
                if (this.ValueDateMode == ValueDateMode.FixedDate && this.ValueDate.HasValue)
                {
                    return this.ValueDate.Value.Date;
                }
                return this.EndSellTime.Date;
            }
        }

        public string EndorseImageLink { get; set; }

        public string EndorseImageThumbnailLink { get; set; }

        public DateTime EndSellTime { get; set; }

        public decimal FinancingSum { get; set; }

        public int FinancingSumCount { get; set; }

        public int Id { get; set; }

        public DateTime LaunchTime { get; set; }

        public int MaxShareCount { get; set; }

        public int MinShareCount { get; set; }

        public bool OnPreSale
        {
            get
            {
                return this.PreStartSellTime.HasValue && this.PreEndSellTime.HasValue
                       && DateTime.Now.AddSeconds(2) > this.PreStartSellTime.Value && DateTime.Now.AddSeconds(2) < this.PreEndSellTime.Value;
            }
        }

        public bool OnSale
        {
            get { return DateTime.Now.AddSeconds(2) > this.StartSellTime && DateTime.Now.AddSeconds(2) < this.EndSellTime; }
        }

        public int Period { get; set; }

        public int PledgeAgreementId { get; set; }

        public DateTime? PreEndSellTime { get; set; }

        public bool PreSale
        {
            get { return this.PreStartSellTime.HasValue && this.PreEndSellTime.HasValue; }
        }

        public DateTime? PreStartSellTime { get; set; }

        public string ProductIdentifier { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public int ProductNumber { get; set; }

        public ProductType ProductType { get; set; }

        public bool Repaid { get; set; }

        public DateTime RepaymentDeadline { get; set; }

        public DateTime SettleDate { get; set; }

        public bool SoldOut { get; set; }

        public DateTime? SoldOutTime { get; set; }

        public DateTime StartSellTime { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime? ValueDate { get; set; }

        public ValueDateMode ValueDateMode { get; set; }

        public decimal Yield { get; set; }

        /// <summary>
        /// 产品分类（10金银猫 20富滇 40阜新）
        /// </summary>
        public ProductCategory ProductCategory { get; set; }

        public string SubProductNo { get; set; }
    }
}