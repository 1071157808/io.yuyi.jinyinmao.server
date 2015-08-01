// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : TransBankCard.cs
// Created          : 2015-08-02  7:06 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-02  7:17 AM
// ***********************************************************************
// <copyright file="TransBankCard.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    /// <summary>
    ///     TransBankCard.
    /// </summary>
    [Table("TransBankCard")]
    public class TransBankCard
    {
        /// <summary>
        ///     Gets or sets the adding time.
        /// </summary>
        /// <value>The adding time.</value>
        [Key]
        [Column(Order = 0, TypeName = "datetime2")]
        public DateTime AddingTime { get; set; }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public int? Args { get; set; }

        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        /// <value>The bank card no.</value>
        [Key]
        [Column(Order = 1)]
        [StringLength(25)]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        /// <value>The name of the bank.</value>
        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string BankName { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        [StringLength(20)]
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the name of the city.
        /// </summary>
        /// <value>The name of the city.</value>
        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string CityName { get; set; }

        /// <summary>
        ///     Gets or sets the dispaly.
        /// </summary>
        /// <value>The dispaly.</value>
        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Dispaly { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [StringLength(50)]
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets the verified.
        /// </summary>
        /// <value>The verified.</value>
        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Verified { get; set; }

        /// <summary>
        ///     Gets or sets the verified by yilian.
        /// </summary>
        /// <value>The verified by yilian.</value>
        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VerifiedByYilian { get; set; }

        /// <summary>
        ///     Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        [Key]
        [Column(Order = 7, TypeName = "datetime2")]
        public DateTime VerifiedTime { get; set; }

        /// <summary>
        ///     Gets or sets the withdraw amount.
        /// </summary>
        /// <value>The withdraw amount.</value>
        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WithdrawAmount { get; set; }
    }
}