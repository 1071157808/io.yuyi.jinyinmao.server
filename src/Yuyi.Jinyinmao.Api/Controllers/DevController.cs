// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : DevController.cs
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-07  2:10 PM
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
using Moe.AspNet.Filters;
using Moe.AspNet.Utility;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Api.Models.Product;
using Yuyi.Jinyinmao.Api.Models.User;
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
        ///     Cancels the jby account transaction.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="transactionIdentifier">The transaction identifier.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [Route("CancelJBYAccountTransaction/{userIdentifier:length(32)}/{transactionIdentifier:length(32)}"), ActionParameterRequired, ActionParameterValidate(Order = 1), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(JBYTransactionInfoResponse))]
        public async Task<IHttpActionResult> CancelJBYAccountTransaction(string userIdentifier, string transactionIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            Guid transacationId = Guid.ParseExact(transactionIdentifier, "N");

            JBYAccountTransactionInfo info = await UserFactory.GetGrain(userId).CancelJBYAccountTransactionAsync(transacationId, this.BuildArgs());

            if (info == null)
            {
                return this.BadRequest("未找到对应的账户流水");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     Cancels the order.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="orderIdentifier">The order identifier.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [Route("CancelOrder/{userIdentifier:length(32)}/{orderIdentifier:length(32)}"), ActionParameterRequired, ActionParameterValidate(Order = 1), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(OrderInfoResponse))]
        public async Task<IHttpActionResult> CancelOrder(string userIdentifier, string orderIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            Guid orderId = Guid.ParseExact(orderIdentifier, "N");

            OrderInfo info = await UserFactory.GetGrain(userId).CancelOrderAsync(orderId, this.BuildArgs());

            if (info == null)
            {
                return this.BadRequest("未找到对应的订单");
            }

            return this.Ok(info.ToResponse());
        }

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
        ///     Cancels the settle account transaction result.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="transactionIdentifier">The transaction identifier.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [Route("CancelSettleAccountTransactionResult/{userIdentifier:length(32)}/{transactionIdentifier:length(32)}"), ActionParameterRequired, ActionParameterValidate(Order = 1), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(SettleAccountTransactionInfoResponse))]
        public async Task<IHttpActionResult> CancelSettleAccountTransactionResult(string userIdentifier, string transactionIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            Guid transacationId = Guid.ParseExact(transactionIdentifier, "N");

            SettleAccountTransactionInfo info = await UserFactory.GetGrain(userId).CancelSettleAccountTransactionAsync(transacationId, this.BuildArgs());

            if (info == null)
            {
                return this.BadRequest("未找到对应的账户流水");
            }

            return this.Ok(info.ToResponse());
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
        ///     ClearUnauthenticatedInfoAsync
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("ClearUnauthenticatedInfoAsync/{userIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> ClearUnauthenticatedInfoAsync(string userIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            UserInfo info = await UserFactory.GetGrain(userId).ClearUnauthenticatedInfoAsync();
            return this.Ok(info.ToResponse());
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
        ///     DumpJBYProduct
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("DumpJBYProduct"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DumpJBYProduct()
        {
            await JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).DumpAsync();
            return this.Ok();
        }

        /// <summary>
        ///     DumpProduct
        /// </summary>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("DumpProduct/{productIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DumpProduct(string productIdentifier)
        {
            Guid productId = Guid.ParseExact(productIdentifier, "N");
            await RegularProductFactory.GetGrain(productId).DumpAsync();
            return this.Ok();
        }

        /// <summary>
        ///     DumpUser
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("DumpUser/{userIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DumpUser(string userIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            await UserFactory.GetGrain(userId).DumpAsync();
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
        ///     InsertJBYAccountTranscation
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("InsertJBYAccountTranscation"), ActionParameterRequired, ActionParameterValidate(Order = 1), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(JBYTransactionInfoResponse))]
        public async Task<IHttpActionResult> InsertJBYAccountTranscation(InsertJBYAccountTransactionRequest request)
        {
            Guid userId = Guid.ParseExact(request.UserIdentifier, "N");

            InsertJBYAccountTransactionDto dto = new InsertJBYAccountTransactionDto
            {
                Amount = request.Amount,
                Args = this.BuildArgs(),
                Trade = request.Trade,
                TradeCode = request.TradeCode,
                TransDesc = request.TransDesc,
                UserId = userId
            };

            JBYAccountTransactionInfo info = await UserFactory.GetGrain(userId).InsertJBYAccountTranscationAsync(dto);
            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     InsertSettleAccountTranscation
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("InsertSettleAccountTranscation"), ActionParameterRequired, ActionParameterValidate(Order = 1), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(SettleAccountTransactionInfoResponse))]
        public async Task<IHttpActionResult> InsertSettleAccountTranscation(InsertSettleAccountTransactionRequest request)
        {
            Guid userId = Guid.ParseExact(request.UserIdentifier, "N");

            InsertSettleAccountTransactionDto dto = new InsertSettleAccountTransactionDto
            {
                Amount = request.Amount,
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                OrderId = request.OrderId.GetValueOrDefault(Guid.Empty),
                Trade = request.Trade,
                TradeCode = request.TradeCode,
                TransDesc = request.TransDesc,
                UserId = userId
            };

            SettleAccountTransactionInfo info = await UserFactory.GetGrain(userId).InsertSettleAccountTranscationAsync(dto);
            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     LockUser
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("LockUser/{userIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> LockUser(string userIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");

            UserInfo info = await UserFactory.GetGrain(userId).LockAsync();

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     RefreshJBYProduct
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("RefreshJBYProduct"), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(JBYInfoResponse))]
        public async Task<IHttpActionResult> RefreshJBYProduct()
        {
            await JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).RefreshAsync();
            return this.Ok();
        }

        /// <summary>
        ///     ReloadCellphone
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("ReloadCellphone/{cellphone:length(11)}"), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(CellphoneInfoResponse))]
        public async Task<IHttpActionResult> ReloadCellphone(string cellphone)
        {
            CellphoneInfo info = await CellphoneFactory.GetGrain(GrainTypeHelper.GetCellphoneGrainTypeLongKey(cellphone)).ReloadAsync();
            return this.Ok(info.ToResponse());
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
        ///     RemoveJBYReversalTransactions
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <param name="transactionIdentifier">流水唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("RemoveJBYReversalTransaction/{userIdentifier:length(32)}/{transactionIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> RemoveJBYReversalTransaction(string userIdentifier, string transactionIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            Guid transactionId = Guid.ParseExact(transactionIdentifier, "N");

            bool result = await UserFactory.GetGrain(userId).RemoveJBYTransactionsAsync(transactionId);
            return this.Ok(new { Result = result });
        }

        /// <summary>
        ///     ReprocessDepositSaga
        /// </summary>
        /// <param name="sagaIdentifier">支付流程唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("ReprocessDepositSaga/{sagaIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> ReprocessDepositSaga(string sagaIdentifier)
        {
            Guid sagaId = Guid.ParseExact(sagaIdentifier, "N");
            await DepositSagaFactory.GetGrain(sagaId).ReprocessAsync();

            return this.Ok();
        }

        /// <summary>
        ///     Sets the jby account transaction result.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [Route("SetJBYAccountTransactionResult"), ActionParameterRequired, ActionParameterValidate(Order = 1), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(JBYTransactionInfoResponse))]
        public async Task<IHttpActionResult> SetJBYAccountTransactionResult(SetJBYAccountTransactionResultRequest request)
        {
            Guid userId = Guid.ParseExact(request.UserIdentifier, "N");
            Guid transacationId = Guid.ParseExact(request.TransactionIdentifier, "N");

            JBYAccountTransactionInfo info = await UserFactory.GetGrain(userId).SetJBYAccountTransactionResultAsync(transacationId, request.Result, request.Message, this.BuildArgs());

            if (info == null)
            {
                return this.BadRequest("未找到对应的账户流水");
            }

            return this.Ok(info.ToResponse());
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
        ///     Sets the settle account transaction result.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        [Route("SetSettleAccountTransactionResult"), ActionParameterRequired, ActionParameterValidate(Order = 1), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(SettleAccountTransactionInfoResponse))]
        public async Task<IHttpActionResult> SetSettleAccountTransactionResult(SetSettleAccountTransactionResultRequest request)
        {
            Guid userId = Guid.ParseExact(request.UserIdentifier, "N");
            Guid transacationId = Guid.ParseExact(request.TransactionIdentifier, "N");

            SettleAccountTransactionInfo info = await UserFactory.GetGrain(userId).SetSettleAccountTransactionResultAsync(transacationId, request.Result, request.Message, this.BuildArgs());

            if (info == null)
            {
                return this.BadRequest("未找到对应的账户流水");
            }

            return this.Ok(info.ToResponse());
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
        ///     Transfers the jby transaction.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="transactionIdentifier">The transaction identifier.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [Route("TransferJBYTransaction/{userIdentifier:length(32)}/{transactionIdentifier:length(32)}"), ActionParameterRequired, ActionParameterValidate(Order = 1), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(JBYInfoResponse))]
        public async Task<IHttpActionResult> TransferJBYTransaction(string userIdentifier, string transactionIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            Guid transactionId = Guid.ParseExact(transactionIdentifier, "N");

            JBYAccountTransactionInfo info = await UserFactory.GetGrain(userId).TransferJBYTransactionAsync(transactionId, this.BuildArgs());

            if (info == null)
            {
                return this.BadRequest("未找到对应的账户流水");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     Transfers the order.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="orderIdentifier">The order identifier.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [Route("TransferOrder/{userIdentifier:length(32)}/{orderIdentifier:length(32)}"), ActionParameterRequired, ActionParameterValidate(Order = 1), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(OrderInfoResponse))]
        public async Task<IHttpActionResult> TransferOrder(string userIdentifier, string orderIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            Guid orderId = Guid.ParseExact(orderIdentifier, "N");

            OrderInfo info = await UserFactory.GetGrain(userId).TransferOrderAsync(orderId, this.BuildArgs());

            if (info == null)
            {
                return this.BadRequest("未找到对应的订单");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     UnlockUser
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("UnlockUser/{userIdentifier:length(32)}"), IpAuthorize(OnlyLocalHost = true), ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> UnlockUser(string userIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");

            UserInfo info = await UserFactory.GetGrain(userId).UnlockAsync();

            return this.Ok(info.ToResponse());
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
                    .UnregisterAsync();
            }
            return this.Ok();
        }
    }
}