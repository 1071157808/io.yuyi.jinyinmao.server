// FileInformation: nyanya/nyanya.AspDotNet.Common/PasswordFormatAttribute.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:18 AM

using System;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib.Utility;

namespace nyanya.AspDotNet.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PasswordFormatAttribute : RegularExpressionAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.ComponentModel.DataAnnotations.RegularExpressionAttribute" /> class.
        /// </summary>
        public PasswordFormatAttribute()
            : base(RegexUtils.PasswordRegex.ToString())
        {
        }
    }
}