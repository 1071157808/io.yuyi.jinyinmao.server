// FileInformation: nyanya/nyanya.AspDotNet.Common/MinimumAttribute.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:18 AM

using System;
using System.ComponentModel.DataAnnotations;

namespace nyanya.AspDotNet.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinimumAttribute : ValidationAttribute
    {
        private readonly int minimumValue;

        public MinimumAttribute(int minimun)
        {
            this.minimumValue = minimun;
        }

        /// <summary>
        ///     Determines whether the specified value of the object is valid.
        /// </summary>
        /// <returns>
        ///     true if the specified value is valid; otherwise, false.
        /// </returns>
        /// <param name="value">The value of the object to validate. </param>
        public override bool IsValid(object value)
        {
            int intValue;
            if (value != null && int.TryParse(value.ToString(), out intValue))
            {
                return (intValue >= this.minimumValue);
            }
            return false;
        }
    }
}