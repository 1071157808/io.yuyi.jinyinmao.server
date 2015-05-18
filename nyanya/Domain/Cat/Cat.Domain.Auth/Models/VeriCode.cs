// FileInformation: nyanya/Cat.Domain.Auth/VeriCode.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using Domian.Models;
using System;

namespace Cat.Domain.Auth.Models
{
    public partial class VeriCode : IAggregateRoot
    {
        #region VeriCodeType enum

        public enum VeriCodeType
        {
            SignUp = 10,
            ResetLoginPassword = 20,
            ResetPaymentPassword = 30,
            VeriImage = 100
        }

        #endregion VeriCodeType enum

        public static VeriCodeType ToCodeType(int type)
        {
            switch (type)
            {
                case 10:
                    return VeriCodeType.SignUp;

                case 20:
                    return VeriCodeType.ResetLoginPassword;

                case 30:
                    return VeriCodeType.ResetPaymentPassword;

                case 100:
                    return VeriCodeType.VeriImage;

                default:
                    return 0;
            }
        }

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

        public string ClientId { get; set; }
    }
}