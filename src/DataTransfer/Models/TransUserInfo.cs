// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : TransUserInfo.cs
// Created          : 2015-07-28  11:38 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-28  11:43 AM
// ***********************************************************************
// <copyright file="TransUserInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    [Table("TransUserInfo")]
    public class TransUserInfo
    {
        public int? Args { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Balance { get; set; }

        public int? BankCardsCount { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Cellphone { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ClientType { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Closed { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ContractId { get; set; }

        public int? Credential { get; set; }

        [StringLength(50)]
        public string CredentialNo { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Crediting { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Debiting { get; set; }

        [Key]
        [Column(Order = 23)]
        [StringLength(80)]
        public string EncryptedPassword { get; set; }

        [StringLength(80)]
        public string EncryptedPaymentPassword { get; set; }

        public int? HasSetPassword { get; set; }

        public int? HasSetPaymentPassword { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvestingInterest { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvestingPrincipal { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(36)]
        public string InviteBy { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int JBYAccrualAmount { get; set; }

        public decimal? JBYLastInterest { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int JBYTotalAmount { get; set; }

        public decimal? JBYTotalInterest { get; set; }

        public decimal? JBYTotalPricipal { get; set; }

        public decimal? JBYWithdrawalableAmount { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(20)]
        public string LoginNames { get; set; }

        [Key]
        [Column(Order = 13)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MonthWithdrawalCount { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(30)]
        public string OutletCode { get; set; }

        [Key]
        [Column(Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PasswordErrorCount { get; set; }

        public int? PaymentPasswordErrorCount { get; set; }

        [StringLength(80)]
        public string PaymentSalt { get; set; }

        [StringLength(20)]
        public string RealName { get; set; }

        [Key]
        [Column(Order = 16, TypeName = "datetime2")]
        public DateTime RegisterTime { get; set; }

        [Key]
        [Column(Order = 24)]
        [StringLength(80)]
        public string Salt { get; set; }

        [Key]
        [Column(Order = 17)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TodayJBYWithdrawalAmount { get; set; }

        [Key]
        [Column(Order = 18)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TodayWithdrawalCount { get; set; }

        [Key]
        [Column(Order = 19)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TotalInterest { get; set; }

        [Key]
        [Column(Order = 20)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TotalPrincipal { get; set; }

        [Key]
        [Column(Order = 21)]
        [StringLength(50)]
        public string UserId { get; set; }

        public bool? Verified { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? VerifiedTime { get; set; }

        [Key]
        [Column(Order = 22)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WithdrawalableAmount { get; set; }
    }
}