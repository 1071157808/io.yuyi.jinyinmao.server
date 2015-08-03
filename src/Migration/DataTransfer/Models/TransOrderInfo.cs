// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : TransOrderInfo.cs
// Created          : 2015-08-02  7:06 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-02  7:19 AM
// ***********************************************************************
// <copyright file="TransOrderInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    /// <summary>
    ///     TransOrderInfo.
    /// </summary>
    [Table("TransOrderInfo")]
    public class TransOrderInfo
    {
        /// <summary>
        ///     Gets or sets the account transaction identifier.
        /// </summary>
        /// <value>The account transaction identifier.</value>
        [Key]
        [Column(Order = 0)]
        [StringLength(1)]
        public string AccountTransactionIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string Args { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        [Key]
        [Column(Order = 2)]
        [StringLength(15)]
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the extra interest.
        /// </summary>
        /// <value>The extra interest.</value>
        [Key]
        [Column(Order = 3)]
        public decimal ExtraInterest { get; set; }

        /// <summary>
        ///     Gets or sets the extra interest records.
        /// </summary>
        /// <value>The extra interest records.</value>
        public int? ExtraInterestRecords { get; set; }

        /// <summary>
        ///     Gets or sets the extra yield.
        /// </summary>
        /// <value>The extra yield.</value>
        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ExtraYield { get; set; }

        /// <summary>
        ///     Gets or sets the interest.
        /// </summary>
        /// <value>The interest.</value>
        [Key]
        [Column(Order = 5)]
        public decimal Interest { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is repaid.
        /// </summary>
        /// <value><c>true</c> if this instance is repaid; otherwise, <c>false</c>.</value>
        [Key]
        [Column(Order = 6)]
        public bool IsRepaid { get; set; }

        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string OrderId { get; set; }

        /// <summary>
        ///     Gets or sets the order no.
        /// </summary>
        /// <value>The order no.</value>
        [Key]
        [Column(Order = 8)]
        [StringLength(20)]
        public string OrderNo { get; set; }

        /// <summary>
        ///     Gets or sets the order time.
        /// </summary>
        /// <value>The order time.</value>
        [Key]
        [Column(Order = 9, TypeName = "datetime2")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     Gets or sets the principal.
        /// </summary>
        /// <value>The principal.</value>
        [Key]
        [Column(Order = 10)]
        public decimal Principal { get; set; }

        /// <summary>
        ///     Gets or sets the product category.
        /// </summary>
        /// <value>The product category.</value>
        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductCategory { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        [Key]
        [Column(Order = 12)]
        [StringLength(50)]
        public string ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the product snapshot.
        /// </summary>
        /// <value>The product snapshot.</value>
        public int? ProductSnapshot { get; set; }

        /// <summary>
        ///     Gets or sets the type of the product.
        /// </summary>
        /// <value>The type of the product.</value>
        public int? ProductType { get; set; }

        /// <summary>
        ///     Gets or sets the repaid time.
        /// </summary>
        /// <value>The repaid time.</value>
        public int? RepaidTime { get; set; }

        /// <summary>
        ///     Gets or sets the result code.
        /// </summary>
        /// <value>The result code.</value>
        public int? ResultCode { get; set; }

        /// <summary>
        ///     Gets or sets the result time.
        /// </summary>
        /// <value>The result time.</value>
        [Column(TypeName = "datetime2")]
        public DateTime? ResultTime { get; set; }

        /// <summary>
        ///     Gets or sets the settle date.
        /// </summary>
        /// <value>The settle date.</value>
        [Key]
        [Column(Order = 13, TypeName = "datetime2")]
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        [StringLength(200)]
        public string TransDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Key]
        [Column(Order = 14)]
        [StringLength(50)]
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public int? UserInfo { get; set; }

        /// <summary>
        ///     Gets or sets the value date.
        /// </summary>
        /// <value>The value date.</value>
        [Key]
        [Column(Order = 15, TypeName = "datetime2")]
        public DateTime ValueDate { get; set; }

        /// <summary>
        ///     Gets or sets the yield.
        /// </summary>
        /// <value>The yield.</value>
        [Key]
        [Column(Order = 16)]
        public decimal Yield { get; set; }
    }
}