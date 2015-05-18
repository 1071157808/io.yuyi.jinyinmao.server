// FileInformation: nyanya/nyanya.AspDotNet.Common/CellphoneFormatAttribute.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:18 AM

using System;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib.Utility;

namespace nyanya.AspDotNet.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CellphoneFormatAttribute : RegularExpressionAttribute
    {
        public CellphoneFormatAttribute()
            : base(RegexUtils.CellphoneRegex.ToString())
        {
        }
    }
}