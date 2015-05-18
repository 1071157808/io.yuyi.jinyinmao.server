// FileInformation: nyanya/Services.WebAPI.Common/MaximumAttribute.cs
// CreatedTime: 2014/03/30   10:59 PM
// LastUpdatedTime: 2014/03/30   11:01 PM

using System;
using System.ComponentModel.DataAnnotations;

namespace Services.WebAPI.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaximumAttribute : ValidationAttribute
    {
        #region Private Fields

        private readonly int maximumValue;

        #endregion Private Fields

        #region Public Constructors

        public MaximumAttribute(int maximun)
        {
            this.maximumValue = maximun;
        }

        #endregion Public Constructors

        #region Public Methods

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
                return (intValue <= this.maximumValue);
            }
            return false;
        }

        #endregion Public Methods
    }
}