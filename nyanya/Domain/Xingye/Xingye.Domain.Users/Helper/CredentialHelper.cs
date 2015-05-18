// FileInformation: nyanya/Xingye.Domain.Users/CredentialHelper.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using Infrastructure.Lib.Extensions;
using System.Collections.Generic;
using System.Text;
using Xingye.Commands.Users;

namespace Xingye.Domain.Users.Helper
{
    public static class CredentialHelper
    {
        private static readonly Dictionary<Credential, int> CredentialCode = new Dictionary<Credential, int>
        {
            { Credential.IdCard, 0 },
            { Credential.Passport, 1 },
            { Credential.Taiwan, 2 },
            { Credential.Junguan, 3 },
        };

        /// <summary>
        ///     Credentials the type desc.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>证件类型的中文名称</returns>
        public static int CredentialTypeCode(this Credential credential)
        {
            return GetCredentialTypeCode(credential);
        }

        /// <summary>
        ///     Credentials the type desc.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>
        ///     证件类型的代码
        /// </returns>
        public static int GetCredentialTypeCode(Credential credential)
        {
            int credentialTypeCode;
            return CredentialCode.TryGetValue(credential, out credentialTypeCode) ? credentialTypeCode : -1;
        }

        /// <summary>
        ///     Credentials the type desc.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>
        ///     证件类型的代码
        /// </returns>
        public static int GetCredentialTypeCode(Credential? credential)
        {
            int credentialTypeCode;
            if (!credential.HasValue)
            {
                return -1;
            }
            return CredentialCode.TryGetValue(credential.Value, out credentialTypeCode) ? credentialTypeCode : -1;
        }

        /// <summary>
        ///     Gets the hiden credential no.
        /// </summary>
        /// <param name="credentialNo">The credential no.</param>
        /// <returns></returns>
        public static string GetHidenCredentialNo(string credentialNo)
        {
            if (credentialNo.IsNullOrEmpty())
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            if (credentialNo.Length >= 12)
            {
                sb.Append(credentialNo.GetFirst(4));
                (credentialNo.Length - 8).Times().Do(() => sb.Append("*"));
                sb.Append(credentialNo.GetLast(4));
            }
            else
            {
                (credentialNo.Length - 4).Times().Do(() => sb.Append("*"));
                sb.Append(credentialNo.GetLast(4));
            }

            return sb.ToString();
        }
    }
}