// FileInformation: nyanya/Services.WebAPI.V1.nyanya/YilianCallbackController.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/08   1:01 AM

using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Cqrs.Domain.Bus;
using Cqrs.Events.Yilian;
using Services.WebAPI.Common.Controller;
using Services.WebAPI.Common.Log;
using Services.WebAPI.V1.nyanya.Filters;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     易联支付回调接口
    /// </summary>
    [RoutePrefix("Callback/Yilian")]
    public class YilianCallbackController : ApiControllerBase
    {
        #region Private Fields

        private readonly HttpRequestLogger callbackLogger = new HttpRequestLogger("CallbackLogger");
        private readonly IEventBus eventBus;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianCallbackController" /> class.
        /// </summary>
        /// <param name="eventBus">The event bus.</param>
        public YilianCallbackController(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     易联支付代收请求回调接口
        /// </summary>
        /// <returns>200</returns>
        [Route("Payment")]
        [IpAuthorize]
        public async Task<IHttpActionResult> Payment()
        {
            NameValueCollection requestData = await this.Request.Content.ReadAsFormDataAsync();
            string json = requestData.Get("P");
            this.callbackLogger.Log(this.Request, json);
            this.eventBus.Publish(new YilianPaymentRequestCallbackReceived(json));

            return this.OK();
        }

        /// <summary>
        ///     易联支付实名认证回调接口
        /// </summary>
        /// <returns>200</returns>
        [Route("UserAuth")]
        [IpAuthorize]
        public async Task<IHttpActionResult> UserAuth()
        {
            NameValueCollection requestData = await this.Request.Content.ReadAsFormDataAsync();
            string json = requestData.Get("P");
            this.callbackLogger.Log(this.Request, json);
            this.eventBus.Publish(new YilianAuthRequestCallbackReceived(json));

            return this.OK();
        }

        #endregion Public Methods
    }
}