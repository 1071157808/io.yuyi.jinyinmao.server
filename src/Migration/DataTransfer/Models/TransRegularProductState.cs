// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : TransRegularProductState.cs
// Created          : 2015-08-02  7:06 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-02  7:18 AM
// ***********************************************************************
// <copyright file="TransRegularProductState.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    /// <summary>
    ///     TransRegularProductState.
    /// </summary>
    [Table("TransRegularProductState")]
    public class TransRegularProductState
    {
        /// <summary>
        ///     Gets or sets the agreement1.
        /// </summary>
        /// <value>The agreement1.</value>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Agreement1 { get; set; }

        /// <summary>
        ///     Gets or sets the agreement2.
        /// </summary>
        /// <value>The agreement2.</value>
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Agreement2 { get; set; }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public int? Args { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        /// <value>The name of the bank.</value>
        [StringLength(100)]
        public string BankName { get; set; }

        /// <summary>
        ///     Gets or sets the drawee.
        /// </summary>
        /// <value>The drawee.</value>
        [StringLength(300)]
        public string Drawee { get; set; }

        /// <summary>
        ///     Gets or sets the drawee information.
        /// </summary>
        /// <value>The drawee information.</value>
        [StringLength(1000)]
        public string DraweeInfo { get; set; }

        /// <summary>
        ///     Gets or sets the endorse image link.
        /// </summary>
        /// <value>The endorse image link.</value>
        [Key]
        [Column(Order = 3)]
        [StringLength(300)]
        public string EndorseImageLink { get; set; }

        /// <summary>
        ///     Gets or sets the end sell time.
        /// </summary>
        /// <value>The end sell time.</value>
        [Key]
        [Column(Order = 4, TypeName = "datetime2")]
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the enterprise information.
        /// </summary>
        /// <value>The enterprise information.</value>
        [StringLength(1000)]
        public string EnterpriseInfo { get; set; }

        /// <summary>
        ///     Gets or sets the enterprise license.
        /// </summary>
        /// <value>The enterprise license.</value>
        [StringLength(80)]
        public string EnterpriseLicense { get; set; }

        /// <summary>
        ///     Gets or sets the name of the enterprise.
        /// </summary>
        /// <value>The name of the enterprise.</value>
        [StringLength(300)]
        public string EnterpriseName { get; set; }

        /// <summary>
        ///     Gets or sets the financing sum amount.
        /// </summary>
        /// <value>The financing sum amount.</value>
        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FinancingSumAmount { get; set; }

        /// <summary>
        ///     Gets or sets the issue no.
        /// </summary>
        /// <value>The issue no.</value>
        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IssueNo { get; set; }

        /// <summary>
        ///     Gets or sets the issue time.
        /// </summary>
        /// <value>The issue time.</value>
        [Key]
        [Column(Order = 7, TypeName = "datetime2")]
        public DateTime IssueTime { get; set; }

        /// <summary>
        ///     Gets or sets the orders.
        /// </summary>
        /// <value>The orders.</value>
        public int? Orders { get; set; }

        /// <summary>
        ///     Gets or sets the period.
        /// </summary>
        /// <value>The period.</value>
        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Period { get; set; }

        /// <summary>
        ///     Gets or sets the pledge no.
        /// </summary>
        /// <value>The pledge no.</value>
        [StringLength(80)]
        public string PledgeNo { get; set; }

        /// <summary>
        ///     Gets or sets the product category.
        /// </summary>
        /// <value>The product category.</value>
        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductCategory { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        [Key]
        [Column(Order = 11)]
        [StringLength(50)]
        public string ProductName { get; set; }

        /// <summary>
        ///     Gets or sets the product no.
        /// </summary>
        /// <value>The product no.</value>
        [Key]
        [Column(Order = 12)]
        [StringLength(40)]
        public string ProductNo { get; set; }

        /// <summary>
        ///     Gets or sets the type of the product.
        /// </summary>
        /// <value>The type of the product.</value>
        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="TransRegularProductState" /> is repaid.
        /// </summary>
        /// <value><c>true</c> if repaid; otherwise, <c>false</c>.</value>
        [Key]
        [Column(Order = 13)]
        public bool Repaid { get; set; }

        /// <summary>
        ///     Gets or sets the repaid time.
        /// </summary>
        /// <value>The repaid time.</value>
        public int? RepaidTime { get; set; }

        /// <summary>
        ///     Gets or sets the repayment deadline.
        /// </summary>
        /// <value>The repayment deadline.</value>
        [Key]
        [Column(Order = 14, TypeName = "datetime2")]
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        ///     Gets or sets the risk management.
        /// </summary>
        /// <value>The risk management.</value>
        [StringLength(300)]
        public string RiskManagement { get; set; }

        /// <summary>
        ///     Gets or sets the risk management information.
        /// </summary>
        /// <value>The risk management information.</value>
        [StringLength(1000)]
        public string RiskManagementInfo { get; set; }

        /// <summary>
        ///     Gets or sets the risk management mode.
        /// </summary>
        /// <value>The risk management mode.</value>
        public int? RiskManagementMode { get; set; }

        /// <summary>
        ///     Gets or sets the settle date.
        /// </summary>
        /// <value>The settle date.</value>
        [Key]
        [Column(Order = 15, TypeName = "datetime2")]
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [sold out].
        /// </summary>
        /// <value><c>true</c> if [sold out]; otherwise, <c>false</c>.</value>
        [Key]
        [Column(Order = 16)]
        public bool SoldOut { get; set; }

        /// <summary>
        ///     Gets or sets the sold out time.
        /// </summary>
        /// <value>The sold out time.</value>
        [Column(TypeName = "datetime2")]
        public DateTime? SoldOutTime { get; set; }

        /// <summary>
        ///     Gets or sets the start sell time.
        /// </summary>
        /// <value>The start sell time.</value>
        [Key]
        [Column(Order = 17, TypeName = "datetime2")]
        public DateTime StartSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the unit price.
        /// </summary>
        /// <value>The unit price.</value>
        [Key]
        [Column(Order = 18)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        ///     Gets or sets the usage.
        /// </summary>
        /// <value>The usage.</value>
        [StringLength(1000)]
        public string Usage { get; set; }

        /// <summary>
        ///     Gets or sets the value date.
        /// </summary>
        /// <value>The value date.</value>
        [Column(TypeName = "datetime2")]
        public DateTime? ValueDate { get; set; }

        /// <summary>
        ///     Gets or sets the value date mode.
        /// </summary>
        /// <value>The value date mode.</value>
        [Key]
        [Column(Order = 19)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ValueDateMode { get; set; }

        /// <summary>
        ///     Gets or sets the yield.
        /// </summary>
        /// <value>The yield.</value>
        [Key]
        [Column(Order = 20)]
        public decimal Yield { get; set; }
    }
}