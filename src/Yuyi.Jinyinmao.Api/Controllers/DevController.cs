// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : DevController.cs
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-30  1:43 PM
// ***********************************************************************
// <copyright file="DevController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Tracing;
using Moe.AspNet.Utility;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models.Order;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Products;
using Yuyi.Jinyinmao.Domain.Sagas;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     Class HomeController.
    /// </summary>
    [RoutePrefix("")]
    public class DevController : ApiControllerBase
    {
        /// <summary>
        ///     CancelOrderFromProduct
        /// </summary>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <param name="orderIdentifier">订单唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("CancelOrderFromProduct/{productIdentifier:length(32)}-{orderIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(OrderInfoResponse))]
        public async Task<IHttpActionResult> CancelOrderFromProduct(string productIdentifier, string orderIdentifier)
        {
            Guid productId = Guid.ParseExact(productIdentifier, "N");
            Guid orderId = Guid.ParseExact(orderIdentifier, "N");
            OrderInfo order = await RegularProductFactory.GetGrain(productId).CancelOrderAsync(orderId);

            if (order == null)
            {
                return this.BadRequest("Can not find the order.");
            }

            return this.Ok(order.ToResponse());
        }

        /// <summary>
        ///     Changes the cellphone.
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <param name="cellphone">需要修改为的手机号</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("ChangeCellphone/{userIdentifier:length(32)}/{cellphone:length(11)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> ChangeCellphone(string userIdentifier, string cellphone)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");

            ICellphone cellphoneGrain = CellphoneFactory.GetGrain(GrainTypeHelper.GetCellphoneGrainTypeLongKey(cellphone));
            CellphoneInfo cellphoneInfo = await cellphoneGrain.GetCellphoneInfoAsync();
            if (cellphoneInfo.Registered)
            {
                return this.BadRequest("修改的目标手机号已经被注册");
            }

            if (RegexUtility.CellphoneRegex.IsMatch(cellphone))
            {
                IUser user = UserFactory.GetGrain(userId);
                UserInfo info = await user.ChangeCellphoneAsync(cellphone);
                await user.SyncAsync();
                return this.Ok(info);
            }
            return this.BadRequest("手机号格式不正确");
        }

        /// <summary>
        ///     CheckJBYProductSaleStatus
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("CheckJBYProductSaleStatus"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> CheckJBYProductSaleStatus()
        {
            await JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).CheckSaleStatusAsync();
            return this.Ok();
        }

        /// <summary>
        ///     CheckProductSaleStatus
        /// </summary>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("CheckProductSaleStatus/{productIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> CheckProductSaleStatus(string productIdentifier)
        {
            Guid productId = Guid.ParseExact(productIdentifier, "N");
            await RegularProductFactory.GetGrain(productId).CheckSaleStatusAsync();
            return this.Ok();
        }

        /// <summary>
        ///     DepositSaga
        /// </summary>
        /// <param name="sagaIdentifier">支付流程唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("DepositSaga/{sagaIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DepositSaga(string sagaIdentifier)
        {
            Guid sagaId = Guid.ParseExact(sagaIdentifier, "N");
            await DepositSagaFactory.GetGrain(sagaId).ProcessAsync();

            return this.Ok();
        }

        /// <summary>
        ///     DoDailyWork
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("DoDailyWork/{userIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DoDailyWork(string userIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            await UserFactory.GetGrain(userId).DoDailyWorkAsync(true);
            return this.Ok();
        }

        /// <summary>
        ///     The default action of the service.
        /// </summary>
        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            this.TraceWriter.Debug(this.Request, "Application", "This is from Yuyi.Jinyinmao.Api. Debug test");
            this.TraceWriter.Info(this.Request, "Application", "This is from Yuyi.Jinyinmao.Api. Info test");
            this.TraceWriter.Warn(this.Request, "Application", "This is from Yuyi.Jinyinmao.Api. Warn test");
            this.TraceWriter.Error(this.Request, "Application", "This is from Yuyi.Jinyinmao.Api. Error test");
            this.TraceWriter.Fatal(this.Request, "Application", "This is from Yuyi.Jinyinmao.Api. Fatal test.");

            return this.Ok(
                new
                {
                    this.Request.RequestUri,
                    this.Request.Headers,
                    QueryParameters = this.Request.GetQueryNameValuePairs(),
                    RequestProperties = this.Request.Properties.Keys,
                    this.RequestContext.ClientCertificate,
                    this.RequestContext.IsLocal,
                    this.RequestContext.VirtualPathRoot,
                    HttpContext.Current.Request.Browser.Browser,
                    HttpContext.Current.Request.IsSecureConnection,
                    HttpContext.Current.Request.Browser.IsMobileDevice,
                    IsFromMobileDevice = HttpUtils.IsFromMobileDevice(this.Request),
                    UserHostAddress = HttpUtils.GetUserHostAddress(this.Request),
                    UserAgent = HttpUtils.GetUserAgent(this.Request),
                    Cookie = this.Request.Headers.GetCookies(),
                    this.Request.Content,
                    ConfigurationProperties = this.Configuration.Properties,
                    ServerIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString()
                });
        }

        /// <summary>
        ///     RefreshJBYProduct
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("RefreshJBYProduct"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> RefreshJBYProduct()
        {
            await JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).RefreshAsync();
            return this.Ok();
        }

        /// <summary>
        ///     ReloadJBYProduct
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("ReloadJBYProduct"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> ReloadJBYProduct()
        {
            await JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).ReloadAsync();
            return this.Ok();
        }

        /// <summary>
        ///     ReloadProduct
        /// </summary>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("ReloadProduct/{productIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> ReloadProduct(string productIdentifier)
        {
            Guid productId = Guid.ParseExact(productIdentifier, "N");
            await RegularProductFactory.GetGrain(productId).ReloadAsync();
            return this.Ok();
        }

        /// <summary>
        ///     ReloadUser
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("ReloadUser/{userIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> ReloadUser(string userIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            await UserFactory.GetGrain(userId).ReloadAsync();
            return this.Ok();
        }

        /// <summary>
        ///     SetJBYProductToSoldOut
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("SetJBYProductToSoldOut"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SetJBYProductToSoldOut()
        {
            await JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).SetToSoldOutAsync();
            return this.Ok();
        }

        /// <summary>
        ///     SetProductToOnSale
        /// </summary>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("SetProductToOnSale/{productIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SetProductToOnSale(string productIdentifier)
        {
            Guid productId = Guid.ParseExact(productIdentifier, "N");
            await RegularProductFactory.GetGrain(productId).SetToOnSaleAsync();
            return this.Ok();
        }

        /// <summary>
        ///     SetProductToSoldOut
        /// </summary>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("SetProductToSoldOut/{productIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SetProductToSoldOut(string productIdentifier)
        {
            Guid productId = Guid.ParseExact(productIdentifier, "N");
            await RegularProductFactory.GetGrain(productId).SetToSoldOutAsync();
            return this.Ok();
        }

        /// <summary>
        ///     SyncJBYProduct
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("SyncJBYProduct"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SyncJBYProduct()
        {
            await JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).SyncAsync();
            return this.Ok();
        }

        /// <summary>
        ///     SyncProduct
        /// </summary>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("ReloadProduct/{productIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SyncProduct(string productIdentifier)
        {
            Guid productId = Guid.ParseExact(productIdentifier, "N");
            await RegularProductFactory.GetGrain(productId).SyncAsync();
            return this.Ok();
        }

        /// <summary>
        ///     SyncUser
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("SyncUser/{userIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SyncUser(string userIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            await UserFactory.GetGrain(userId).SyncAsync();
            return this.Ok();
        }

        /// <summary>
        ///     UnregisteredCellphone
        /// </summary>
        /// <param name="cellphone">手机号</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("UnregisteredCellphone/{cellphone:length(11)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> UnregisteredCellphone(string cellphone)
        {
            if (RegexUtility.CellphoneRegex.IsMatch(cellphone))
            {
                await CellphoneFactory.GetGrain(GrainTypeHelper.GetCellphoneGrainTypeLongKey(cellphone))
                    .Unregister();
            }
            return this.Ok();
        }
    }
}