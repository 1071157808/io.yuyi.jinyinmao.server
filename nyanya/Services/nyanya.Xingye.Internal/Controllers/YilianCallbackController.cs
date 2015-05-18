// FileInformation: nyanya/nyanya.Xingye.Internal/YilianCallbackController.cs
// CreatedTime: 2014/08/27   4:47 PM
// LastUpdatedTime: 2014/09/01   5:25 PM

using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Xingye.Events.Yilian;
using Domian.Bus;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Log;
using nyanya.Xingye.Internal.Filters;

namespace nyanya.Xingye.Internal.Controllers
{
    /// <summary>
    ///     易联支付回调接口
    /// </summary>
    [RoutePrefix("Callback/Yilian")]
    public class YilianCallbackController : ApiControllerBase
    {
        private readonly HttpRequestLogger callbackLogger = new HttpRequestLogger("CallbackLogger");
        private readonly IEventBus eventBus;

        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianCallbackController" /> class.
        /// </summary>
        /// <param name="eventBus">The event bus.</param>
        public YilianCallbackController(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

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
    }
}