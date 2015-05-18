// FileInformation: nyanya/Infrastructure.Lib/Guard.cs
// CreatedTime: 2015/03/04   6:31 PM
// LastUpdatedTime: 2015/03/10   10:49 PM

using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using System;

namespace Infrastructure.Lib.Utility
{
    public static class Guard
    {
        #region Public Methods

        /// <summary>
        ///     Checks an argument to ensure that its 32-bit signed value isn positive.
        /// </summary>
        /// <param name="argumentValue">The <see cref="T:System.Int32" /> value of the argument.</param>
        /// <param name="argumentName">The name of the argument for diagnostic purposes.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void ArgumentMustBePositiveValue(int argumentValue, string argumentName)
        {
            if (argumentValue > 0)
                return;
            throw new ArgumentOutOfRangeException(argumentName, argumentValue, string.Format("ArgumentMustBePositive:{0}", new object[] { argumentName }));
        }

        /// <summary>
        ///     Checks an argument to ensure that its value doesn't exceed the specified ceiling baseline.
        /// </summary>
        /// <param name="argumentValue">The <see cref="T:System.Double" /> value of the argument.</param>
        /// <param name="ceilingValue">
        ///     The <see cref="T:System.Double" /> ceiling value of the argument.
        /// </param>
        /// <param name="argumentName">The name of the argument for diagnostic purposes.</param>
        public static void ArgumentNotGreaterThan(double argumentValue, double ceilingValue, string argumentName)
        {
            if (argumentValue <= ceilingValue)
                return;
            throw new ArgumentOutOfRangeException(argumentName, argumentValue, string.Format("Argument {0} Cannot Be Greater Than Baseline {1}", new object[] { argumentName, ceilingValue }));
        }

        /// <summary>
        ///     Checks an argument to ensure that its 32-bit signed value isn't negative.
        /// </summary>
        /// <param name="argumentValue">The <see cref="T:System.Int32" /> value of the argument.</param>
        /// <param name="argumentName">The name of the argument for diagnostic purposes.</param>
        public static void ArgumentNotNegativeValue(int argumentValue, string argumentName)
        {
            if (argumentValue >= 0)
                return;
            throw new ArgumentOutOfRangeException(argumentName, argumentValue, string.Format("ArgumentCannotBeNegative:{0}", new object[] { argumentName }));
        }

        /// <summary>
        ///     Checks an argument to ensure that its 64-bit signed value isn't negative.
        /// </summary>
        /// <param name="argumentValue">The <see cref="T:System.Int64" /> value of the argument.</param>
        /// <param name="argumentName">The name of the argument for diagnostic purposes.</param>
        public static void ArgumentNotNegativeValue(long argumentValue, string argumentName)
        {
            if (argumentValue >= 0L)
                return;
            throw new ArgumentOutOfRangeException(argumentName, argumentValue, string.Format("ArgumentCannotBeNegative:{0}", new object[] { argumentName }));
        }

        /// <summary>
        ///     Checks an argument to ensure that it isn't null.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            Ensure.ArgumentNotNull(argumentValue, argumentName);
        }

        /// <summary>
        ///     Checks a string argument to ensure that it isn't null or empty.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        public static void ArgumentNotNullOrEmpty(string argumentValue, string argumentName)
        {
            Ensure.ArgumentNotNullOrEmpty(argumentValue, argumentName);
        }

        /// <summary>
        ///     Checks a string value to ensure that it isn't null or empty.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="name">The name of the entity type.</param>
        public static void IdentifierMustBeAssigned(object value, string name)
        {
            Ensure.NotNullOrEmpty<IdentifierMissingException>(value.ToStringExceptNull(), "Identifier of {0} must be assigned".FormatWith(name));
        }

        #endregion Public Methods
    }
}
