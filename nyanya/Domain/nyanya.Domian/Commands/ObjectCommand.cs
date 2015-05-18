using System;
using Infrastructure.Lib.Utility;

namespace Domian.Commands
{
    /// <summary>
    ///     Represents the base class of the commands.
    /// </summary>
    [Serializable]
    public abstract class ObjectCommand : IObjectCommand
    {
        /// <summary>
        ///     Initializes a new instance of the <c>ObjectCommand</c> class.
        /// </summary>
        protected ObjectCommand()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>ObjectCommand</c> class.
        /// </summary>
        /// <param name="source">The source identifier.</param>
        protected ObjectCommand(string source)
        {
            this.CommandId = GuidUtils.NewSequentialGuid();
            this.Source = source;
        }

        /// <summary>
        ///     Initializes a new instance of the <c>ObjectCommand</c> class.
        /// </summary>
        /// <param name="commandId">The command identifier.</param>
        /// <param name="source">The source identifier.</param>
        protected ObjectCommand(Guid commandId, string source)
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

        #endregion IObjectCommand Members

        /// <summary>
        ///     Returns a <see cref="System.Boolean" /> value indicating whether this instance is equal to a specified
        ///     object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        ///     True if obj is an instance of the <see cref="Domain.Commands.IObjectCommand" /> type and equals the value of this
        ///     instance; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            var other = obj as ObjectCommand;
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
