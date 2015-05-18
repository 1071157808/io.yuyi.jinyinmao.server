// FileInformation: nyanya/Infrastructure.Lib/HandleGenerator.cs
// CreatedTime: 2014/06/17   1:30 PM
// LastUpdatedTime: 2014/06/17   1:30 PM

using System;

namespace Infrastructure.Lib.Utility
{
    /// <summary>
    ///     Generates random alphnumerical strings.
    /// </summary>
    public static class HandleGenerator
    {
        private static readonly char[] AllowableChars = "ABCDEFGHJKMNPQRSTUVWXYZ123456789".ToCharArray();
        private static readonly Random Rnd = new Random(DateTime.UtcNow.Millisecond);

        public static string Generate(int length)
        {
            char[] result = new char[length];
            lock (Rnd)
            {
                for (int i = 0; i < length; i++)
                {
                    result[i] = AllowableChars[Rnd.Next(0, AllowableChars.Length)];
                }
            }

            return new string(result);
        }
    }
}