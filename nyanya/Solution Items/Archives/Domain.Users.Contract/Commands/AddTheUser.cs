// FileInformation: nyanya/Domain.Users.Contract/AddTheUser.cs
// CreatedTime: 2014/07/01   1:32 PM
// LastUpdatedTime: 2014/07/01   2:51 PM

using Domain.Commands;
using ServiceStack;

namespace Domain.Users.Contract.Commands
{
    [Route("/AddUser")]
    public class AddTheUser : Command, IReturn<CommandExcuteResult>
    {
        public string Cellphone { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string UserIdentifier { get; set; }
    }
}