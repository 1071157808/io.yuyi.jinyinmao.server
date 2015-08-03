// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : TransUserInfo.cs
// Created          : 2015-08-02  7:06 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-02  7:19 AM
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
    /// <summary>
    /// TransUserInfo.
    /// </summary>
    [Table("TransUserInfo")]
    public class TransUserInfo
    {
        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public int? Args { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>The balance.</value>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Balance { get; set; }

        /// <summary>
        /// Gets or sets the bank cards count.
        /// </summary>
        /// <value>The bank cards count.</value>
        public int? BankCardsCount { get; set; }

        /// <summary>
        /// Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Cellphone { get; set; }

        /// <summary>
        /// Gets or sets the type of the client.
        /// </summary>
        /// <value>The type of the client.</value>
        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ClientType { get; set; }

        /// <summary>
        /// Gets or sets the closed.
        /// </summary>
        /// <value>The closed.</value>
        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Closed { get; set; }

        /// <summary>
        /// Gets or sets the contract identifier.
        /// </summary>
        /// <value>The contract identifier.</value>
        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ContractId { get; set; }

        /// <summary>
        /// Gets or sets the credential.
        /// </summary>
        /// <value>The credential.</value>
        public int? Credential { get; set; }

        /// <summary>
        /// Gets or sets the credential no.
        /// </summary>
        /// <value>The credential no.</value>
        [StringLength(50)]
        public string CredentialNo { get; set; }

        /// <summary>
        /// Gets or sets the crediting.
        /// </summary>
        /// <value>The crediting.</value>
        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Crediting { get; set; }

        /// <summary>
        /// Gets or sets the debiting.
        /// </summary>
        /// <value>The debiting.</value>
        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Debiting { get; set; }

        /// <summary>
        /// Gets or sets the encrypted password.
        /// </summary>
        /// <value>The encrypted password.</value>
        [Key]
        [Column(Order = 23)]
        [StringLength(80)]
        public string EncryptedPassword { get; set; }

        /// <summary>
        /// Gets or sets the encrypted payment password.
        /// </summary>
        /// <value>The encrypted payment password.</value>
        [StringLength(80)]
        public string EncryptedPaymentPassword { get; set; }

        /// <summary>
        /// Gets or sets the has set password.
        /// </summary>
        /// <value>The has set password.</value>
        public int? HasSetPassword { get; set; }

        /// <summary>
        /// Gets or sets the has set payment password.
        /// </summary>
        /// <value>The has set payment password.</value>
        public int? HasSetPaymentPassword { get; set; }

        /// <summary>
        /// Gets or sets the investing interest.
        /// </summary>
        /// <value>The investing interest.</value>
        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvestingInterest { get; set; }

        /// <summary>
        /// Gets or sets the investing principal.
        /// </summary>
        /// <value>The investing principal.</value>
        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvestingPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the invite by.
        /// </summary>
        /// <value>The invite by.</value>
        [Key]
        [Column(Order = 9)]
        [StringLength(36)]
        public string InviteBy { get; set; }

        /// <summary>
        /// Gets or sets the jby accrual amount.
        /// </summary>
        /// <value>The jby accrual amount.</value>
        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int JBYAccrualAmount { get; set; }

        /// <summary>
        /// Gets or sets the jby last interest.
        /// </summary>
        /// <value>The jby last interest.</value>
        public decimal? JBYLastInterest { get; set; }

        /// <summary>
        /// Gets or sets the jby total amount.
        /// </summary>
        /// <value>The jby total amount.</value>
        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int JBYTotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the jby total interest.
        /// </summary>
        /// <value>The jby total interest.</value>
        public decimal? JBYTotalInterest { get; set; }

        /// <summary>
        /// Gets or sets the jby total pricipal.
        /// </summary>
        /// <value>The jby total pricipal.</value>
        public decimal? JBYTotalPricipal { get; set; }

        /// <summary>
        /// Gets or sets the jby withdrawalable amount.
        /// </summary>
        /// <value>The jby withdrawalable amount.</value>
        public decimal? JBYWithdrawalableAmount { get; set; }

        /// <summary>
        /// Gets or sets the login names.
        /// </summary>
        /// <value>The login names.</value>
        [Key]
        [Column(Order = 12)]
        [StringLength(20)]
        public string LoginNames { get; set; }

        /// <summary>
        /// Gets or sets the month withdrawal count.
        /// </summary>
        /// <value>The month withdrawal count.</value>
        [Key]
        [Column(Order = 13)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MonthWithdrawalCount { get; set; }

        /// <summary>
        /// Gets or sets the outlet code.
        /// </summary>
        /// <value>The outlet code.</value>
        [Key]
        [Column(Order = 14)]
        [StringLength(30)]
        public string OutletCode { get; set; }

        /// <summary>
        /// Gets or sets the password error count.
        /// </summary>
        /// <value>The password error count.</value>
        [Key]
        [Column(Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PasswordErrorCount { get; set; }

        /// <summary>
        /// Gets or sets the payment password error count.
        /// </summary>
        /// <value>The payment password error count.</value>
        public int? PaymentPasswordErrorCount { get; set; }

        /// <summary>
        /// Gets or sets the payment salt.
        /// </summary>
        /// <value>The payment salt.</value>
        [StringLength(80)]
        public string PaymentSalt { get; set; }

        /// <summary>
        /// Gets or sets the name of the real.
        /// </summary>
        /// <value>The name of the real.</value>
        [StringLength(20)]
        public string RealName { get; set; }

        /// <summary>
        /// Gets or sets the register time.
        /// </summary>
        /// <value>The register time.</value>
        [Key]
        [Column(Order = 16, TypeName = "datetime2")]
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// Gets or sets the salt.
        /// </summary>
        /// <value>The salt.</value>
        [Key]
        [Column(Order = 24)]
        [StringLength(80)]
        public string Salt { get; set; }

        /// <summary>
        /// Gets or sets the today jby withdrawal amount.
        /// </summary>
        /// <value>The today jby withdrawal amount.</value>
        [Key]
        [Column(Order = 17)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TodayJBYWithdrawalAmount { get; set; }

        /// <summary>
        /// Gets or sets the today withdrawal count.
        /// </summary>
        /// <value>The today withdrawal count.</value>
        [Key]
        [Column(Order = 18)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TodayWithdrawalCount { get; set; }

        /// <summary>
        /// Gets or sets the total interest.
        /// </summary>
        /// <value>The total interest.</value>
        [Key]
        [Column(Order = 19)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TotalInterest { get; set; }

        /// <summary>
        /// Gets or sets the total principal.
        /// </summary>
        /// <value>The total principal.</value>
        [Key]
        [Column(Order = 20)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TotalPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Key]
        [Column(Order = 21)]
        [StringLength(50)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TransUserInfo"/> is verified.
        /// </summary>
        /// <value><c>null</c> if [verified] contains no value, <c>true</c> if [verified]; otherwise, <c>false</c>.</value>
        public bool? Verified { get; set; }

        /// <summary>
        /// Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        [Column(TypeName = "datetime2")]
        public DateTime? VerifiedTime { get; set; }

        /// <summary>
        /// Gets or sets the withdrawalable amount.
        /// </summary>
        /// <value>The withdrawalable amount.</value>
        [Key]
        [Column(Order = 22)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WithdrawalableAmount { get; set; }
    }
}