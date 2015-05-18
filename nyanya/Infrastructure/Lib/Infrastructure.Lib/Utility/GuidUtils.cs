// FileInformation: nyanya/Infrastructure.Lib/GuidUtils.cs
// CreatedTime: 2014/06/10   12:07 PM
// LastUpdatedTime: 2014/06/10   12:07 PM

using System;
using System.Linq;

namespace Infrastructure.Lib.Utility
{
    public static class GuidUtils
    {
        private const int FactorPrime = 29;
        private const int InitialPrime = 23;
        private static readonly long EpochMilliseconds = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000L;

        /// <summary>
        ///     Gets the hash code for an object based on the given array of hash
        ///     codes from each property of the object.
        /// </summary>
        /// <param name="hashCodesForProperties">
        ///     The array of the hash codes
        ///     that are from each property of the object.
        /// </param>
        /// <returns>The hash code.</returns>
        public static int GetHashCode(params int[] hashCodesForProperties)
        {
            unchecked
            {
                return hashCodesForProperties.Aggregate(InitialPrime, (current, code) => current * FactorPrime + code);
            }
        }

        /// <summary>
        ///     Creates a sequential GUID string that have removed the '-' char according to SQL
        ///     Server's ordering rules.
        /// </summary>
        public static string NewGuidString()
        {
            return NewSequentialId().ToString().Replace("-", "");
        }

        /// <summary>
        ///     Creates a sequential GUID according to SQL Server's ordering rules.
        /// </summary>
        public static Guid NewSequentialGuid()
        {
            return NewSequentialId();
        }

        /// <summary>
        ///     Creates a sequential GUID according to SQL Server's ordering rules.
        /// </summary>
        public static Guid NewSequentialId()
        {
            // This code was not reviewed to guarantee uniqueness under most conditions, nor
            // completely optimize for avoiding page splits in SQL Server when doing inserts from
            // multiple hosts, so do not re-use in production systems.
            byte[] guidBytes = Guid.NewGuid().ToByteArray();

            // get the milliseconds since Jan 1 1970
            byte[] sequential = BitConverter.GetBytes((DateTime.Now.Ticks / 10000L) - EpochMilliseconds);

            // discard the 2 most significant bytes, as we only care about the milliseconds
            // increasing, but the highest ones should be 0 for several thousand years to come (non-issue).
            if (BitConverter.IsLittleEndian)
            {
                guidBytes[10] = sequential[5];
                guidBytes[11] = sequential[4];
                guidBytes[12] = sequential[3];
                guidBytes[13] = sequential[2];
                guidBytes[14] = sequential[1];
                guidBytes[15] = sequential[0];
            }
            else
            {
                Buffer.BlockCopy(sequential, 2, guidBytes, 10, 6);
            }

            return new Guid(guidBytes);
        }
    }
}