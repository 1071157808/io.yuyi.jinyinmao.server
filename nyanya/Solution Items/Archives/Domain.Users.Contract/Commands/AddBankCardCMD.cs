// FileInformation: nyanya/Domain.Users.Contract/AddBankCardCMD.cs
// CreatedTime: 2014/06/30   5:14 PM
// LastUpdatedTime: 2014/06/30   6:23 PM

using Domain.Commands;


namespace Domain.Users.Contract.Commands
{
    public class AddBankCardCmd : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        /// <param name="source">The source identifier.</param>
        public AddBankCardCmd(string source)
            : base(source)
        {
        }

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string CityName { get; set; }

        public string UserIdentifier { get; set; }
    }
}