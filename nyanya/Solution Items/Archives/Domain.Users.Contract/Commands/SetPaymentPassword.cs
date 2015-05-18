// FileInformation: nyanya/Domain.Users/SetPaymentPassword.cs
// CreatedTime: 2014/06/16   1:29 PM
// LastUpdatedTime: 2014/06/24   10:42 AM

using Domain.Commands;

namespace Domain.Users.Contract.Commands
{
    public class SetPaymentPassword : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        /// <param name="source">The source identifier.</param>
        public SetPaymentPassword(string source)
            : base(source)
        {
            this.UserIdentifier = source;
        }

        public string PaymentPassword { get; set; }

        public string UserIdentifier { get; set; }
    }
}