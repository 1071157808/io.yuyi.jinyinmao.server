// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : TransSettleAccountTransaction.cs
// Created          : 2015-08-02  7:06 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-02  7:19 AM
// ***********************************************************************
// <copyright file="TransSettleAccountTransaction.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    /// <summary>
    ///     TransSettleAccountTransaction.
    /// </summary>
    [Table("TransSettleAccountTransaction")]
    public class TransSettleAccountTransaction
    {
        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        /// <value>The bank card no.</value>
        [StringLength(30)]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the callback time.
        /// </summary>
        /// <value>The callback time.</value>
        [Column(TypeName = "datetime2")]
        public DateTime? CallbackTime { get; set; }

        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        [Key]
        [StringLength(50)]
        public string OrderId { get; set; }
    }
}