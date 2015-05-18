// FileInformation: nyanya/Infrastructure.Lib/CommandExcuteFaildException.cs
// CreatedTime: 2014/07/30   5:41 PM
// LastUpdatedTime: 2014/08/01   2:02 AM

using System;

namespace Infrastructure.Lib.Exceptions
{
    /// <summary>
    ///     该异常标识命令未成功执行，往往应该有日志，报警和反馈
    /// </summary>
    public class CommandExcuteFaildException : NyanyaException
    {
        /// <summary>
        ///     Initializes a new instance of the <c>CommandExcuteFaildException</c> class.
        /// </summary>
        public CommandExcuteFaildException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>CommandExcuteFaildException</c> class.
        /// </summary>
        /// <param name="commandId">The command identifier.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="friendlyMessage">The friendly message.</param>
        public CommandExcuteFaildException(Guid commandId, string message, string errorCode = "0000", string friendlyMessage = "请稍后再试")
            : base(errorCode + message)
        {
            this.CommandId = commandId;
            this.FriendlyMessage = friendlyMessage;
            this.ErrorCode = errorCode;
        }

        /// <summary>
        ///     Initializes a new instance of the <c>CommandExcuteFaildException</c> class.
        /// </summary>
        /// <param name="commandId">The command identifier.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="friendlyMessage">The friendly message.</param>
        public CommandExcuteFaildException(Guid commandId, string message, Exception innerException, string errorCode = "0000", string friendlyMessage = "请稍后再试")
            : base(errorCode + message, innerException)
        {
            this.CommandId = commandId;
            this.FriendlyMessage = friendlyMessage;
            this.ErrorCode = errorCode;
        }

        public Guid CommandId { get; set; }

        public string ErrorCode { get; set; }

        public string FriendlyMessage { get; set; }
    }
}