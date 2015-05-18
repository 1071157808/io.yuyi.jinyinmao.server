// FileInformation: nyanya/nyanya.Meow/TimelineResponse.cs
// CreatedTime: 2014/09/16   6:18 PM
// LastUpdatedTime: 2014/09/16   6:20 PM

using System.Collections.Generic;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     TimelineResponse
    /// </summary>
    public class TimelineResponse
    {
        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        /// <value>
        ///     The items.
        /// </value>
        public IEnumerable<TimelineNodeDto> Items { get; set; }

        /// <summary>
        ///     Gets or sets the timestamp.
        /// </summary>
        /// <value>
        ///     The timestamp.
        /// </value>
        public string Timestamp { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="TimelineResponse" /> is updated.
        /// </summary>
        /// <value>
        ///     <c>true</c> if updated; otherwise, <c>false</c>.
        /// </value>
        public bool Updated { get; set; }
    }
}