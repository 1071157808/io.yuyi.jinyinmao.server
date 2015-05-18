// FileInformation: nyanya/Xingye.Domain.Auth/VeriCode.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System;
using Domian.Models;

namespace Xingye.Domain.Auth.Models
{
    public partial class VeriCode : IAggregateRoot
    {
        #region VeriCodeType enum

        public enum VeriCodeType
        {
            SignUp = 10,
            ResetLoginPassword = 20,
            ResetPaymentPassword = 30,
            TempSignUp = 40
        }

        #endregion VeriCodeType enum

        /// <summary>
        ///     Only for Entity Framework.
        /// </summary>
        public VeriCode()
        {
        }

        public VeriCode(string identifier)
        {
            this.Identifier = identifier;
        }

        public DateTime BuildAt { get; set; }

        public string Cellphone { get; set; }

        public string Code { get; set; }

        public int ErrorCount { get; set; }

        public string Identifier { get; set; }

        public byte[] RowVersion { get; set; }

        public int Times { get; set; }

        public VeriCodeType Type { get; set; }

        public bool Used { get; set; }

        public bool Verified { get; set; }
    }
}