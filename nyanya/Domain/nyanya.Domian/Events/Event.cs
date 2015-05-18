// FileInformation: nyanya/Domian/Event.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/01   10:15 AM

using System;
using Infrastructure.Lib.Utility;

namespace Domian.Events
{
    /// <summary>
    ///     Represents the base class of the commands.
    /// </summary>
    [Serializable]
    public abstract class Event : IEvent
    {
        protected Event()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        /// <param name="source">The source.</param>
        protected Event(string source)
        {
            this.EventId = GuidUtils.NewSequentialGuid();
            this.SourceId = source;
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        /// <param name="sourceId">The source identifier.</param>
        /// <param name="sourceType">Type of the source.</param>
        protected Event(string sourceId, Type sourceType)
        {
            this.EventId = GuidUtils.NewSequentialGuid();
            this.SourceId = sourceId;
            this.SourceType = sourceType;
        }

        #region IEvent Members

        public Guid EventId { get; set; }

        public string SourceId { get; set; }

        public Type SourceType { get; set; }

        #endregion IEvent Members

        /// <summary>
        ///     Returns a <see cref="System.Boolean" /> value indicating whether this instance is equal to a specified
        ///     object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        ///     True if obj is an instance of the <see cref="Domian.Events.Event" /> type and equals the value of this
        ///     instance; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            Event other = obj as Event;
            if (other == null)
                return false;
            return this.EventId == other.EventId;
        }

        /// <summary>
        ///     Returns the hash code for current event object.
        /// </summary>
        /// <returns>The calculated hash code for the current event object.</returns>
        public override int GetHashCode()
        {
            return GuidUtils.GetHashCode(this.EventId.GetHashCode());
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.EventId.ToString();
        }
    }
}