using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Commands;
using ServiceStack;

namespace Domain.Users.Contract.Commands
{
    [Route("/TestRabbit")]
    public class TestRabbit : Command, IReturn<CommandExcuteResult>
    {
        public string Message { get; set; }
    }
}
