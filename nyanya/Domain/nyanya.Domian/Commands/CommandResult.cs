// FileInformation: nyanya/Domian/CommandResult.cs
// CreatedTime: 2014/07/12   9:55 PM
// LastUpdatedTime: 2014/07/12   10:23 PM

using System;

namespace Domian.Commands
{
    public class CommandResult
    {
        public CommandResult()
        {
        }

        public CommandResult(Guid commandId, bool result = true, string message = "")
        {
            this.CommandId = commandId;
            this.Result = result;
            this.Message = message;
        }

        public Guid CommandId { get; set; }

        public string Message { get; set; }

        public bool Result { get; set; }
    }
}