// FileInformation: nyanya/Domain/Command.cs
// CreatedTime: 2014/07/01   1:28 PM
// LastUpdatedTime: 2014/07/07   1:23 AM

using System;
using System.Collections.Generic;
using Infrastructure.Lib.Utility;

namespace Domain.Commands
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
            this.Guid = GuidUtils.NewSequentialGuid();
            this.Source = "";
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        /// <param name="source">The source identifier.</param>
        protected Command(string source)
        {
            this.Guid = GuidUtils.NewSequentialGuid();
            this.Source = source;
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        /// <param name="guid">The identifier which identifies a single command instance.</param>
        /// <param name="source">The source identifier.</param>
        protected Command(Guid guid, string source)
        {
            this.Guid = guid;
            this.Source = source;
        }

        /// <summary>
        ///     Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        ///     The unique identifier.
        /// </value>
        public Guid Guid { get; protected set; }

        #region ICommand Members

        /// <summary>
        ///     Gets or sets the source identifier.
        /// </summary>
        /// <value>
        ///     The source.
        /// </value>
        public string Source { get; protected set; }

        public Guid CommandId
        {
            get { throw new NotImplementedException(); }
        }

        public Dictionary<string, string> Headers
        {
            get { throw new NotImplementedException(); }
        }

        public string Payload
        {
            get { throw new NotImplementedException(); }
        }

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
            return this.Guid == other.Guid;
        }

        /// <summary>
        ///     Returns the hash code for current command object.
        /// </summary>
        /// <returns>The calculated hash code for the current command object.</returns>
        public override int GetHashCode()
        {
            return GuidUtils.GetHashCode(this.Guid.GetHashCode());
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.Guid.ToString();
        }
    }
}