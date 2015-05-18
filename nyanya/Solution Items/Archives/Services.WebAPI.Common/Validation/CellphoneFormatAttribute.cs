// FileInformation: nyanya/Services.WebAPI.Common/CellphoneFormatAttribute.cs
// CreatedTime: 2014/03/30   10:59 PM
// LastUpdatedTime: 2014/04/01   3:36 PM

using Infrastructure.Lib.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.WebAPI.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CellphoneFormatAttribute : RegularExpressionAttribute
    {
        #region Public Constructors

        public CellphoneFormatAttribute()
            : base(RegexUtils.CellphoneRegex.ToString())
        {
        }

        #endregion Public Constructors
    }
}