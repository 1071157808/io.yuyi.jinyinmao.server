// FileInformation: nyanya/Cat.Domain.Yilian/YilianAuthRequest.cs
// CreatedTime: 2014/07/28   11:35 AM
// LastUpdatedTime: 2014/08/04   11:04 AM

namespace Cat.Domain.Yilian.Models
{
    public partial class YilianAuthRequest : YilianRequest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianAuthRequest" /> class.
        /// </summary>
        /// <param name="requestIdentifier">The request identifier.</param>
        public YilianAuthRequest(string requestIdentifier)
        {
            this.RequestIdentifier = requestIdentifier;
            this.RequestInfo = new RequestInfo
            {
                RequestIdentifier = requestIdentifier
            };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianAuthRequest" /> class.
        ///     Only for Entity framework
        /// </summary>
        private YilianAuthRequest()
        {
        }
    }
}