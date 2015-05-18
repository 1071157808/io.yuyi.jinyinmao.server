using System.Collections.Generic;

namespace nyanya.Cat.Order
{
    /// <summary>
    ///        TimelineDto
    /// </summary>
    public class TimelineDto
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IEnumerable<TimelineItemDto> Items { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        public string Timestamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TimelineDto"/> is updated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if updated; otherwise, <c>false</c>.
        /// </value>
        public bool Updated { get; set; }
    }
}