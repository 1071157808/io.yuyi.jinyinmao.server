using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Lib.Extensions;

namespace Infrastructure.Lib.Utility
{
    /// <summary>
    /// Argument-specific ensure methods
    /// </summary>
    public static class Argument
    {
        /// <summary>
        /// Ensures given condition is true
        /// </summary>
        /// <param name="condition">Condition to test</param>
        /// <param name="message">Message of the exception if condition fails</param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref cref="condition"/> is false
        /// </exception>
        public static void Is(bool condition, string message = "")
        {
            Ensure.That<ArgumentException>(condition, message);
        }

        /// <summary>
        /// Ensures given condition is false
        /// </summary>
        /// <param name="condition">Condition to test</param>
        /// <param name="message">Message of the exception if condition is true</param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref cref="condition"/> is true
        /// </exception>
        public static void IsNot(bool condition, string message = "")
        {
            Is(!condition, message);
        }

        /// <summary>
        /// Ensures given value is not null
        /// </summary>
        /// <param name="value">Value to test for null</param>
        /// <param name="paramName">Name of the parameter in the method</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref cref="value" /> is null
        /// </exception>
        public static void NotNull(object value, string paramName = "")
        {
            Ensure.That<ArgumentNullException>(value != null, paramName);
        }

        /// <summary>
        /// Ensures the given string value is not null or empty
        /// </summary>
        /// <param name="value">Value to test for null or empty</param>
        /// <param name="paramName">Name of the parameter in the method</param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref cref="value"/> is null or empty string
        /// </exception>
        public static void NotNullOrEmpty(string value, string paramName = "")
        {
            if (value.IsNullOrEmpty())
            {
                if (paramName.IsNullOrEmpty())
                    throw new ArgumentException("String value cannot be empty");

                throw new ArgumentException("String parameter " + paramName + " cannot be null or empty", paramName);
            }
        }
    }
}