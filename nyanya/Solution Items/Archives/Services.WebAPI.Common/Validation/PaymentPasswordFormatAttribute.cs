// FileInformation: nyanya/Services.WebAPI.Common/PaymentPasswordFormatAttribute.cs
// CreatedTime: 2014/08/11   5:58 PM
// LastUpdatedTime: 2014/08/13   2:25 PM

using Infrastructure.Lib.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.WebAPI.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PaymentPasswordFormatAttribute : RegularExpressionAttribute
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.ComponentModel.DataAnnotations.RegularExpressionAttribute" /> class.
        /// </summary>
        public PaymentPasswordFormatAttribute()
            : base(RegexUtils.PaymentPasswordRegex.ToString())
        {
        }

        #endregion Public Constructors
    }
}