// FileInformation: nyanya/Domain.User/StartMobilePayment.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/06/24   10:39 AM

using Domain.Commands;

namespace Domain.Users.Contract.Commands
{
    public class StartMobilePayment : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        /// <param name="source">The source identifier.</param>
        public StartMobilePayment(string source)
            : base(source)
        {
        }

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string CityName { get; set; }

        public string Credential { get; set; }

        public string CredentialNo { get; set; }

        public string RealName { get; set; }

        public string UserIdentifier { get; set; }
    }
}