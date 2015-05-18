// FileInformation: nyanya/Services.WebAPI.Common/PasswordFormatAttribute.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/07/09   9:55 AM

using Infrastructure.Lib.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.WebAPI.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PasswordFormatAttribute : RegularExpressionAttribute
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.ComponentModel.DataAnnotations.RegularExpressionAttribute" /> class.
        /// </summary>
        public PasswordFormatAttribute()
            : base(RegexUtils.PasswordRegex.ToString())
        {
        }

        #endregion Public Constructors
    }
}