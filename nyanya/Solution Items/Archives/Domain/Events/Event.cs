// FileInformation: nyanya/Domain/Event.cs
// CreatedTime: 2014/06/24   11:39 AM
// LastUpdatedTime: 2014/06/24   11:49 AM

using System;
using Infrastructure.Lib.Utility;

namespace Domain.Events
{
    /// <summary>
    ///     Represents the base class of the events.
    /// </summary>
    [Serializable]
    public abstract class Event : IEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        /// <param name="sourceGuid">The source unique identifier.</param>
        protected Event(Guid sourceGuid)
        {
            this.Guid = GuidUtils.NewSequentialGuid();
            this.SourceGuid = sourceGuid;
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        /// <param name="guid">The identifier which identifies a single command instance.</param>
        /// <param name="sourceGuid">The source unique identifier.</param>
        protected Event(Guid guid, Guid sourceGuid)
        {
            this.Guid = guid;
            this.SourceGuid = sourceGuid;
        }

        #region IEvent Members

        /// <summary>
        ///     Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        ///     The unique identifier.
        /// </value>
        public Guid Guid { get; protected set; }

        public string Source
        {
            get { return this.SourceGuid.ToString().Replace("-", ""); }
        }

        /// <summary>
        ///     Gets or sets the source unique identifier.
        /// </summary>
        /// <value>
        ///     The source unique identifier.
        /// </value>
        public Guid SourceGuid { get; protected set; }

        #endregion IEvent Members
    }
}