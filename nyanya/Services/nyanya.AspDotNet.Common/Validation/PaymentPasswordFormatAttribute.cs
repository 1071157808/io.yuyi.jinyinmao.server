// FileInformation: nyanya/nyanya.AspDotNet.Common/PaymentPasswordFormatAttribute.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:09 AM

using System;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib.Utility;

namespace nyanya.AspDotNet.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PaymentPasswordFormatAttribute : RegularExpressionAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.ComponentModel.DataAnnotations.RegularExpressionAttribute" /> class.
        /// </summary>
        public PaymentPasswordFormatAttribute()
            : base(RegexUtils.PaymentPasswordRegex.ToString())
        {
        }
    }
}