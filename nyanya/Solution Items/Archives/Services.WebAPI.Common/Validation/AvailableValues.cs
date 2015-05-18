// FileInformation: nyanya/Services.WebAPI.Common/AvailableValues.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/08/13   12:27 PM

using Infrastructure.Lib;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Services.WebAPI.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class AvailableValuesAttribute : ValidationAttribute
    {
        #region Private Fields

        private readonly string[] availableValues;

        #endregion Private Fields

        #region Public Constructors

        public AvailableValuesAttribute(params object[] values)
            : base(NyanyaResources.AvailableValuesAttribute_AvailableValuesAttribute_ValueNotAvailable)
        {
            this.availableValues = values.Select(v => v.ToString()).ToArray();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <returns>
        ///     An instance of the formatted error message.
        /// </returns>
        /// <param name="name">The name to include in the formatted message.</param>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name);
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
            return value != null && this.availableValues.Contains(value.ToString());
        }

        #endregion Public Methods
    }
}