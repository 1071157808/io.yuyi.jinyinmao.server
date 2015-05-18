// FileInformation: nyanya/Cat.Domain.Yilian/YilianPaymentRequest.cs
// CreatedTime: 2014/08/04   9:45 AM
// LastUpdatedTime: 2014/08/04   10:04 AM

namespace Cat.Domain.Yilian.Models
{
    public partial class YilianPaymentRequest : YilianRequest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianPaymentRequest" /> class.
        /// </summary>
        /// <param name="requestIdentifier">The request identifier.</param>
        public YilianPaymentRequest(string requestIdentifier)
        {
            this.RequestIdentifier = requestIdentifier;
            this.RequestInfo = new RequestInfo
            {
                RequestIdentifier = requestIdentifier
            };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianPaymentRequest" /> class.
        ///     Only for Entity framework
        /// </summary>
        private YilianPaymentRequest()
        {
        }
    }
}