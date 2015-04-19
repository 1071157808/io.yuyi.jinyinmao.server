// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  12:55 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  2:57 PM
// ***********************************************************************
// <copyright file="SmsController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Moe.AspNet.Filters;
using Yuyi.Jinyinmao.Api.Sms.Filters;
using Yuyi.Jinyinmao.Api.Sms.Models;
using Yuyi.Jinyinmao.Api.Sms.Services;

namespace Yuyi.Jinyinmao.Api.Sms.Controllers
{
    /// <summary>
    ///     Class SmsController.
    /// </summary>
    [RoutePrefix("")]
    public class SmsController : ApiController
    {
        /// <summary>
        ///     获取短信通道的余额
        /// </summary>
        /// <remarks>
        ///     如果短信通道不支持余额的查询，余额的值会为-1
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="401"></response>
        [HttpGet, Route("Balance"), ResponseType(typeof(ChannelBalance))]
        public async Task<IHttpActionResult> GetChannelBalance(SmsChannel channel)
        {
            ISmsService service = SmsServiceFactory.GetSmsService(channel);
            int? result = await service.GetBalanceAsync();
            if (result.HasValue)
            {
                return this.Ok(new ChannelBalance
                {
                    Balance = result.Value,
                    SupportBalanceQuery = true
                });
            }

            return this.Ok(new ChannelBalance());
        }

        /// <summary>
        ///     获取所有的短信通道
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        [HttpGet, Route("Channels")]
        public IHttpActionResult GetSmsChannels()
        {
            return this.Ok(SmsChannelEnumHelper.GetChannels());
        }

        /// <summary>
        ///     发送短信(ApiKeyRequired)
        /// </summary>
        /// <remarks>
        ///     短信发送成功，会直接返回200;
        ///     <br />
        ///     请求格式不正确，会返回400;
        ///     <br />
        ///     短信发送失败，会返回500
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400"></response>
        /// <response code="401"></response>
        /// <response code="500"></response>
        [Route("Send"), ActionParameterRequired, ActionParameterValidate(Order = 1), HMACAuthentication]
        public async Task<IHttpActionResult> SendMessage(SmsMessageRequest request)
        {
            ISmsService service = SmsServiceFactory.GetSmsService(request.Channel);
            await service.SendMessageAsync(request.Cellphones, request.Message, request.Signature);

            return this.Ok();
        }
    }
}
