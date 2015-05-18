// FileInformation: nyanya/Services.WebAPI.Common/MinimumAttribute.cs
// CreatedTime: 2014/03/30   10:59 PM
// LastUpdatedTime: 2014/03/30   11:01 PM

using System;
using System.ComponentModel.DataAnnotations;

namespace Services.WebAPI.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinimumAttribute : ValidationAttribute
    {
        #region Private Fields

        private readonly int minimumValue;

        #endregion Private Fields

        #region Public Constructors

        public MinimumAttribute(int minimun)
        {
            this.minimumValue = minimun;
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
                return (intValue >= this.minimumValue);
            }
            return false;
        }

        #endregion Public Methods
    }
}