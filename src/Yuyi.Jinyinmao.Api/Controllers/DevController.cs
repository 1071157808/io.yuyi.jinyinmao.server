// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : DevController.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-19  19:35
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
using Moe.AspNet.Filters;
using Moe.AspNet.Utility;
using Moe.Lib;
using Orleans;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Api.Models.Dev;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Products;
using Yuyi.Jinyinmao.Domain.Sagas;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     Class HomeController.
    /// </summary>
    [RoutePrefix("")]
    public class DevController : ApiControllerBase
    {
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DevController" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public DevController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        ///     CheckJBYProductSaleStatus
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("CheckJBYProductSaleStatus")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> CheckJBYProductSaleStatus()
        {
            await GrainClient.GrainFactory.GetGrain<IJBYProduct>(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).CheckSaleStatusAsync();
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
        [Route("CheckProductSaleStatus/{productIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> CheckProductSaleStatus(string productIdentifier)
        {
            Guid productId = productIdentifier.AsGuid();
            if (productId == Guid.Empty)
            {
                return this.BadRequest("产品唯一标识错误");
            }

            await GrainClient.GrainFactory.GetGrain<IRegularProduct>(productId).CheckSaleStatusAsync();
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
        [Route("DepositSaga/{sagaIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DepositSaga(string sagaIdentifier)
        {
            Guid sagaId = sagaIdentifier.AsGuid();
            if (sagaId == Guid.Empty)
            {
                return this.BadRequest("Saga唯一标识错误");
            }

            await GrainClient.GrainFactory.GetGrain<IDepositSaga>(sagaId).ProcessAsync();
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
        [Route("DoDailyWork/{userIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DoDailyWork(string userIdentifier)
        {
            Guid userId = userIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            await GrainClient.GrainFactory.GetGrain<IUser>(userId).DoDailyWorkAsync(true);
            return this.Ok();
        }

        /// <summary>
        ///     DumpJBYProduct
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("DumpJBYProduct")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DumpJBYProduct()
        {
            await GrainClient.GrainFactory.GetGrain<IJBYProduct>(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).DumpAsync();
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
        [Route("DumpProduct/{productIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DumpProduct(string productIdentifier)
        {
            Guid productId = productIdentifier.AsGuid();
            if (productId == Guid.Empty)
            {
                return this.BadRequest("产品唯一标识错误");
            }

            await GrainClient.GrainFactory.GetGrain<IRegularProduct>(productId).DumpAsync();
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
        [Route("DumpUser/{userIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> DumpUser(string userIdentifier)
        {
            Guid userId = userIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            await GrainClient.GrainFactory.GetGrain<IUser>(userId).DumpAsync();
            return this.Ok();
        }

        /// <summary>
        ///     The default action of the service.
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
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
        ///     用户信息
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     无该用户信息
        /// </response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("UserInfo/{userIdentifier:length(32)}")]
        [ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> GetUserInfo(string userIdentifier)
        {
            Guid userId = userIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            UserInfo info = await this.userService.GetUserInfoAsync(userId);
            if (info == null)
            {
                return this.BadRequest("无该用户信息");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     InsertJBYAccountTranscation
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("InsertJBYAccountTranscation")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [IpAuthorize(OnlyLocalHost = true)]
        [ResponseType(typeof(JBYTransactionInfoResponse))]
        public async Task<IHttpActionResult> InsertJBYAccountTranscation(InsertJBYAccountTransactionRequest request)
        {
            Guid userId = request.UserIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            InsertJBYAccountTransaction command = this.BuildCommand(request, userId);

            JBYAccountTransactionInfo info = await GrainClient.GrainFactory.GetGrain<IUser>(userId).InsertJBYAccountTranscationAsync(command);
            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     InsertSettleAccountTranscation
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("InsertSettleAccountTranscation")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [IpAuthorize(OnlyLocalHost = true)]
        [ResponseType(typeof(SettleAccountTransactionInfoResponse))]
        public async Task<IHttpActionResult> InsertSettleAccountTranscation(InsertSettleAccountTransactionRequest request)
        {
            Guid userId = request.UserIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            InsertSettleAccountTransaction command = this.BuildCommand(request, userId);

            SettleAccountTransactionInfo info = await GrainClient.GrainFactory.GetGrain<IUser>(userId).InsertSettleAccountTranscationAsync(command);
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
        [Route("LockUser/{userIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        [ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> LockUser(string userIdentifier)
        {
            Guid userId = userIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            UserInfo info = await GrainClient.GrainFactory.GetGrain<IUser>(userId).LockAsync();

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     RefreshJBYProduct
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("RefreshJBYProduct")]
        [IpAuthorize(OnlyLocalHost = true)]
        [ResponseType(typeof(JBYProductInfoResponse))]
        public async Task<IHttpActionResult> RefreshJBYProduct()
        {
            await GrainClient.GrainFactory.GetGrain<IJBYProduct>(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).RefreshAsync();
            return this.Ok();
        }

        /// <summary>
        ///     ReloadCellphone
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("ReloadCellphone/{cellphone:length(11)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        [ResponseType(typeof(CellphoneInfoResponse))]
        public async Task<IHttpActionResult> ReloadCellphone(string cellphone)
        {
            CellphoneInfo info = await GrainClient.GrainFactory.GetGrain<ICellphone>(GrainTypeHelper.GetCellphoneGrainTypeLongKey(cellphone)).ReloadAsync();
            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     ReloadJBYProduct
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("ReloadJBYProduct")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> ReloadJBYProduct()
        {
            await GrainClient.GrainFactory.GetGrain<IJBYProduct>(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).ReloadAsync();
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
        [Route("ReloadProduct/{productIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> ReloadProduct(string productIdentifier)
        {
            Guid productId = productIdentifier.AsGuid();
            if (productId == Guid.Empty)
            {
                return this.BadRequest("产品唯一标识错误");
            }

            await GrainClient.GrainFactory.GetGrain<IRegularProduct>(productId).ReloadAsync();
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
        [Route("ReloadUser/{userIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> ReloadUser(string userIdentifier)
        {
            Guid userId = userIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            await GrainClient.GrainFactory.GetGrain<IUser>(userId).ReloadAsync();
            return this.Ok();
        }

        /// <summary>
        ///     RepayOrder
        /// </summary>
        /// <param name="request">The request.</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("RepayOrder")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [IpAuthorize(OnlyLocalHost = true)]
        [ResponseType(typeof(OrderInfoResponse))]
        public async Task<IHttpActionResult> RepayOrder(RepayOrderRequest request)
        {
            Guid userId = request.UserIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            Guid orderId = request.OrderIdentifier.AsGuid();
            if (orderId == Guid.Empty)
            {
                return this.BadRequest("产品唯一标识错误");
            }

            OrderInfo orderInfo = await GrainClient.GrainFactory.GetGrain<IUser>(userId).RepayOrderAsync(this.BuildOrderRepayCommand(request, userId, orderId));

            return this.Ok(orderInfo.ToResponse());
        }

        /// <summary>
        ///     ReprocessDepositSaga
        /// </summary>
        /// <param name="sagaIdentifier">支付流程唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("ReprocessDepositSaga/{sagaIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> ReprocessDepositSaga(string sagaIdentifier)
        {
            Guid sagaId = sagaIdentifier.AsGuid();
            if (sagaId == Guid.Empty)
            {
                return this.BadRequest("Saga唯一标识错误");
            }

            await GrainClient.GrainFactory.GetGrain<IDepositSaga>(sagaId).ReprocessAsync();

            return this.Ok();
        }

        /// <summary>
        ///     Sets the jby account transaction result.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [Route("SetJBYAccountTransactionResult")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [IpAuthorize(OnlyLocalHost = true)]
        [ResponseType(typeof(JBYTransactionInfoResponse))]
        public async Task<IHttpActionResult> SetJBYAccountTransactionResult(SetJBYAccountTransactionResultRequest request)
        {
            Guid userId = request.UserIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            Guid transacationId = request.TransactionIdentifier.AsGuid();
            if (transacationId == Guid.Empty)
            {
                return this.BadRequest("流水唯一标识错误");
            }

            SetJBYAccountTransactionResult command = this.BuildCommand(request, userId, transacationId);

            JBYAccountTransactionInfo info = await GrainClient.GrainFactory.GetGrain<IUser>(userId).SetJBYAccountTransactionResultAsync(command);

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
        [Route("SetJBYProductToSoldOut")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SetJBYProductToSoldOut()
        {
            await GrainClient.GrainFactory.GetGrain<IJBYProduct>(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).SetToSoldOutAsync();
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
        [Route("SetProductToOnSale/{productIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SetProductToOnSale(string productIdentifier)
        {
            Guid productId = productIdentifier.AsGuid();
            if (productId == Guid.Empty)
            {
                return this.BadRequest("产品唯一标识");
            }

            await GrainClient.GrainFactory.GetGrain<IRegularProduct>(productId).SetToOnSaleAsync();
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
        [Route("SetProductToSoldOut/{productIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SetProductToSoldOut(string productIdentifier)
        {
            Guid productId = productIdentifier.AsGuid();
            if (productId == Guid.Empty)
            {
                return this.BadRequest("产品唯一标识");
            }

            await GrainClient.GrainFactory.GetGrain<IRegularProduct>(productId).SetToSoldOutAsync();
            return this.Ok();
        }

        /// <summary>
        ///     Sets the settle account transaction result.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        [Route("SetSettleAccountTransactionResult")]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [IpAuthorize(OnlyLocalHost = true)]
        [ResponseType(typeof(SettleAccountTransactionInfoResponse))]
        public async Task<IHttpActionResult> SetSettleAccountTransactionResult(SetSettleAccountTransactionResultRequest request)
        {
            Guid userId = request.UserIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            Guid transacationId = request.TransactionIdentifier.AsGuid();
            if (transacationId == Guid.Empty)
            {
                return this.BadRequest("流水唯一标识错误");
            }

            SetSettleAccountTransactionResult command = this.BuildCommand(request, userId, transacationId);

            SettleAccountTransactionInfo info = await GrainClient.GrainFactory.GetGrain<IUser>(userId).SetSettleAccountTransactionResultAsync(command);

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
        [Route("SyncJBYProduct")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SyncJBYProduct()
        {
            await GrainClient.GrainFactory.GetGrain<IJBYProduct>(GrainTypeHelper.GetJBYProductGrainTypeLongKey()).SyncAsync();
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
        [Route("SyncProduct/{productIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SyncProduct(string productIdentifier)
        {
            Guid productId = productIdentifier.AsGuid();
            if (productId == Guid.Empty)
            {
                return this.BadRequest("产品唯一标识");
            }

            await GrainClient.GrainFactory.GetGrain<IRegularProduct>(productId).SyncAsync();
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
        [Route("SyncUser/{userIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> SyncUser(string userIdentifier)
        {
            Guid userId = userIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            await GrainClient.GrainFactory.GetGrain<IUser>(userId).SyncAsync();
            return this.Ok();
        }

        /// <summary>
        ///     UnlockUser
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="403"></response>
        /// <response code="500"></response>
        [Route("UnlockUser/{userIdentifier:length(32)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        [ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> UnlockUser(string userIdentifier)
        {
            Guid userId = userIdentifier.AsGuid();
            if (userId == Guid.Empty)
            {
                return this.BadRequest("用户唯一标识错误");
            }

            UserInfo info = await GrainClient.GrainFactory.GetGrain<IUser>(userId).UnlockAsync();

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
        [Route("UnregisteredCellphone/{cellphone:length(11)}")]
        [IpAuthorize(OnlyLocalHost = true)]
        public async Task<IHttpActionResult> UnregisteredCellphone(string cellphone)
        {
            if (RegexUtility.CellphoneRegex.IsMatch(cellphone))
            {
                await GrainClient.GrainFactory.GetGrain<ICellphone>(GrainTypeHelper.GetCellphoneGrainTypeLongKey(cellphone))
                    .UnregisterAsync();
            }
            return this.Ok();
        }

        private InsertSettleAccountTransaction BuildCommand(InsertSettleAccountTransactionRequest request, Guid userId)
        {
            return new InsertSettleAccountTransaction
            {
                EntityId = userId,
                Amount = request.Amount,
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                OrderId = request.OrderId.GetValueOrDefault(Guid.Empty),
                Trade = request.Trade,
                TradeCode = request.TradeCode,
                TransDesc = request.TransDesc,
                UserId = userId
            };
        }

        private SetJBYAccountTransactionResult BuildCommand(SetJBYAccountTransactionResultRequest request, Guid userId, Guid transacationId)
        {
            return new SetJBYAccountTransactionResult
            {
                EntityId = userId,
                Args = this.BuildArgs(),
                Result = request.Result,
                TransacationId = transacationId,
                TransDesc = request.TransDesc,
                UserId = userId
            };
        }

        private SetSettleAccountTransactionResult BuildCommand(SetSettleAccountTransactionResultRequest request, Guid userId, Guid transacationId)
        {
            return new SetSettleAccountTransactionResult
            {
                EntityId = userId,
                Args = this.BuildArgs(),
                Result = request.Result,
                TransacationId = transacationId,
                TransDesc = request.TransDesc,
                UserId = userId
            };
        }

        private InsertJBYAccountTransaction BuildCommand(InsertJBYAccountTransactionRequest request, Guid userId)
        {
            return new InsertJBYAccountTransaction
            {
                EntityId = userId,
                Amount = request.Amount,
                Args = this.BuildArgs(),
                Trade = request.Trade,
                TradeCode = request.TradeCode,
                TransDesc = request.TransDesc,
                UserId = userId
            };
        }

        private OrderRepay BuildOrderRepayCommand(RepayOrderRequest request, Guid userId, Guid orderId)
        {
            return new OrderRepay
            {
                EntityId = userId,
                Args = this.BuildArgs(),
                OrderId = orderId,
                RepayTime = request.RepayTime
            };
        }
    }
}