using System;

namespace Domian.Commands
{
    public class ObjectCommandResult : CommandResult
    {
        public ObjectCommandResult()
        {
        }

        public ObjectCommandResult(Guid commandId, bool result = true, object data = null, string message = "")
            : base(commandId, result, message)
        {
            CommandId = commandId;
            Result = result;
            Data = data;
            Message = message;
        }
        /// <summary>
        /// 其他父级站点传递数据
        /// </summary>
        public object Data { get; set; }
    }
}