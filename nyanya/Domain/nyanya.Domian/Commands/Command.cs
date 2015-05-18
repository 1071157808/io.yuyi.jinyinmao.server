// FileInformation: nyanya/Domian/Command.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/01   12:48 PM

using Infrastructure.Lib.Utility;
using System;

namespace Domian.Commands
{
    /// <summary>
    ///     Represents the base class of the commands.
    /// </summary>
    [Serializable]
    public abstract class Command : ICommand
    {
        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        protected Command()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        /// <param name="source">The source identifier.</param>
        protected Command(string source)
        {
            this.CommandId = GuidUtils.NewSequentialGuid();
            this.Source = source;
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        /// <param name="commandId">The command identifier.</param>
        /// <param name="source">The source identifier.</param>
        protected Command(Guid commandId, string source)
        {
            this.CommandId = commandId;
            this.Source = source;
        }

        #region ICommand Members

        /// <summary>
        ///     Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        ///     The unique identifier.
        /// </value>
        public Guid CommandId { get; set; }

        /// <summary>
        ///     Gets or sets the source identifier.
        /// </summary>
        /// <value>
        ///     The source.
        /// </value>
        public string Source { get; set; }

        #endregion ICommand Members

        /// <summary>
        ///     Returns a <see cref="System.Boolean" /> value indicating whether this instance is equal to a specified
        ///     object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        ///     True if obj is an instance of the <see cref="Domain.Commands.ICommand" /> type and equals the value of this
        ///     instance; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            Command other = obj as Command;
            if (other == null)
                return false;
            return this.CommandId == other.CommandId;
        }

        /// <summary>
        ///     Returns the hash code for current command object.
        /// </summary>
        /// <returns>The calculated hash code for the current command object.</returns>
        public override int GetHashCode()
        {
            return GuidUtils.GetHashCode(this.CommandId.GetHashCode());
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.CommandId.ToString();
        }
    }
}