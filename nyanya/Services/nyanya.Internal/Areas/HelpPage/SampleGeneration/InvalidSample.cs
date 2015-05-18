// FileInformation: nyanya/nyanya.Internal/InvalidSample.cs
// CreatedTime: 2014/09/01   10:42 AM
// LastUpdatedTime: 2014/09/01   10:45 AM

using System;

namespace nyanya.Internal.Areas.HelpPage
{
    /// <summary>
    ///     This represents an invalid sample on the help page. There's a display template named InvalidSample associated with this class.
    /// </summary>
    public class InvalidSample
    {
        public InvalidSample(string errorMessage)
        {
            if (errorMessage == null)
            {
                throw new ArgumentNullException("errorMessage");
            }
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; private set; }

        public override bool Equals(object obj)
        {
            InvalidSample other = obj as InvalidSample;
            return other != null && this.ErrorMessage == other.ErrorMessage;
        }

        public override int GetHashCode()
        {
            return this.ErrorMessage.GetHashCode();
        }

        public override string ToString()
        {
            return this.ErrorMessage;
        }
    }
}