// FileInformation: nyanya/Infrastructure.Lib.Extensions/ObjectExtensions.cs
// CreatedTime: 2014/05/27   2:29 PM
// LastUpdatedTime: 2014/05/27   2:31 PM

namespace Infrastructure.Lib.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Determines whether the specified o is not null.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns>bool</returns>
        public static bool IsNotNull(this object o)
        {
            return !o.IsNull();
        }

        /// <summary>
        ///     Determines whether the specified o is null.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns>bool</returns>
        public static bool IsNull(this object o)
        {
            return o == null;
        }
    }
}