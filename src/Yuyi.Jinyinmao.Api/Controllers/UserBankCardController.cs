// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : UserBankCardController.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-18  14:05
// ***********************************************************************
// <copyright file="UserBankCardController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Moe.AspNet.Filters;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Api.Models.User;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     UserBankCardController.
    /// </summary>
    [RoutePrefix("User/BankCards")]
    public class UserBankCardController : ApiControllerBase
    {
        private readonly IUserInfoService userInfoService;
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserBankCardController" /> class.
        /// </summary>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="userService">The user service.</param>
        public UserBankCardController(IUserInfoService userInfoService, IUserService userService)
        {
            this.userInfoService = userInfoService;
            this.userService = userService;
        }

        /// <summary>
        ///     添加新银行卡
        /// </summary>
        /// <remarks>
        ///     该接口只会添加银行卡信息，不会对银行卡的信息进行校验
        /// </remarks>
        /// <param name="request">
        ///     添加银行卡请求
        /// </param>
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     UBCABC1:无法添加银行卡
        ///     <br />
        ///     UBCABC2:最多绑定10张银行卡
        ///     <br />
        ///     UBCABC3:银行卡添加失败
        ///     <br />
        ///     UBCABC4:该银行卡已经被使用
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [Route("AddBankCard")]
        [CookieAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [ResponseType(typeof(BankCardInfoResponse))]
        public async Task<IHttpActionResult> AddBankCard(AddBankCardRequest request)
        {
            if (await this.userInfoService.CheckBankCardUsedAsync(request.BankCardNo))
            {
                return this.BadRequest("UBCABC4:该银行卡已经被使用");
            }

            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);
            if (userInfo == null)
            {
                return this.BadRequest("USERNOTFOUND:用户信息加载错误，请重新登陆");
            }

            if (userInfo.Closed)
            {
                return this.BadRequest("UC:该账户已经被锁定，请联系金银猫客服");
            }

            if (userInfo.BankCardsCount >= 10)
            {
                return this.BadRequest("UBCABC2:最多绑定10张银行卡");
            }

            BankCardInfo bankCardInfo = await this.userInfoService.GetBankCardInfoAsync(this.CurrentUser.Id, request.BankCardNo);

            if (bankCardInfo != null && bankCardInfo.Dispaly)
            {
                return this.BadRequest("UBCABC4:该银行卡已经被使用");
            }

            bankCardInfo = await this.userService.AddBankCardAsync(this.BuildAddBankCardCommand(request));

            if (bankCardInfo == null)
            {
                return this.BadRequest("UBCABC3:银行卡添加失败");
            }

            return this.Ok(bankCardInfo.ToResponse());
        }

        /// <summary>
        ///     添加新银行卡（通过易联）
        /// </summary>
        /// <remarks>
        ///     该接口只适合已经绑定过实名信息的用户添加新银行卡，最多只能绑定10张银行卡
        /// </remarks>
        /// <param name="request">
        ///     添加银行卡请求
        /// </param>
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     UBCABCBY1:无法添加银行卡
        ///     <br />
        ///     UBCABCBY2:请先进行实名认证
        ///     <br />
        ///     UBCABCBY3:最多绑定10张银行卡
        ///     <br />
        ///     UBCABCBY5:该银行卡已经被使用
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [Route("AddBankCardByYilian")]
        [CookieAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> AddBankCardByYilian(AddBankCardRequest request)
        {
            if (await this.userInfoService.CheckBankCardUsedAsync(request.BankCardNo))
            {
                return this.BadRequest("UBCABCBY4:该银行卡已经被使用");
            }

            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);
            if (userInfo == null)
            {
                return this.BadRequest("USERNOTFOUND:用户信息加载错误，请重新登陆");
            }

            if (userInfo.Closed)
            {
                return this.BadRequest("UC:该账户已经被锁定，请联系金银猫客服");
            }

            if (!userInfo.Verified)
            {
                return this.BadRequest("UBCABCBY2:请先进行实名认证");
            }

            if (userInfo.BankCardsCount >= 10)
            {
                return this.BadRequest("UBCABCBY3:最多绑定10张银行卡");
            }

            AddBankCard addBankCardCommand = this.BuildAddBankCardCommand(request);

            VerifyBankCard verifyBankCardCommand = this.BuildVerifyBankCardCommand(request);

            await this.userService.AddBankCardAsync(addBankCardCommand, verifyBankCardCommand);

            return this.Ok();
        }

        /// <summary>
        ///     获取用户银行卡信息
        /// </summary>
        /// <remarks>
        ///     会获取所有的银行卡信息，无分页功能，排序规则为易联认证的优先，然后按添加时间倒序
        ///     <br />
        ///     这里的银行卡的可提现额为只考虑单卡进出的提现额度
        /// </remarks>
        /// <response code="200">注册成功</response>
        /// <response code="400">UBI1:无法获取用户信息</response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet]
        [Route("Index")]
        [CookieAuthorize]
        [ResponseType(typeof(List<BankCardInfoResponse>))]
        public async Task<IHttpActionResult> Index()
        {
            List<BankCardInfo> cards = await this.userInfoService.GetBankCardInfosAsync(this.CurrentUser.Id);

            return this.Ok(cards.OrderByDescending(c => c.VerifiedByYilian).ThenByDescending(c => c.AddingTime).Select(c => c.ToResponse()).ToList());
        }

        /// <summary>
        ///     获取用户可以用于取现的银行卡信息
        /// </summary>
        /// <remarks>
        ///     会获取所有的银行卡信息，无分页功能，排序规则为按可提现额度从大到小排序
        /// </remarks>
        /// <response code="200">注册成功</response>
        /// <response code="400">UBI:无该银行卡信息</response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet]
        [Route("Info/{bankCardNo:length(15,19)}")]
        [CookieAuthorize]
        [ResponseType(typeof(BankCardInfoResponse))]
        public async Task<IHttpActionResult> Info(string bankCardNo)
        {
            BankCardInfo card = await this.userInfoService.GetBankCardInfoAsync(this.CurrentUser.Id, bankCardNo);
            if (card == null || !card.Dispaly)
            {
                return this.BadRequest("UBI:无该银行卡信息");
            }

            return this.Ok(card.ToResponse());
        }

        /// <summary>
        ///     隐藏银行卡
        /// </summary>
        /// <remarks>
        ///     银行卡会隐藏，重新添加会再次还原
        /// </remarks>
        /// <param name="request">
        ///     隐藏银行卡请求
        /// </param>
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     UBCDBC1:无法添加银行卡
        ///     <br />
        ///     UBCDBC2:银行卡不存在
        ///     <br />
        ///     UBCDBC3:该银行卡尚有资金未完全取出
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [Route("Remove")]
        [CookieAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> RemoveBankCard(DeleteBankCardRequest request)
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);
            if (userInfo == null)
            {
                return this.BadRequest("USERNOTFOUND:用户信息加载错误，请重新登陆");
            }

            if (userInfo.Closed)
            {
                return this.BadRequest("UC:该账户已经被锁定，请联系金银猫客服");
            }

            BankCardInfo info = await this.userInfoService.GetBankCardInfoAsync(this.CurrentUser.Id, request.BankCardNo);

            if (info == null || !info.Dispaly)
            {
                return this.BadRequest("UBCDBC2:银行卡不存在");
            }

            if (info.VerifiedByYilian && info.WithdrawAmount != 0)
            {
                return this.BadRequest("UBCDBC3:该银行卡尚有资金未完全取出");
            }

            await this.userService.HideBankCardAsync(this.BuildHideBankCardCommand(request));

            return this.Ok();
        }

        /// <summary>
        ///     易联认证银行卡
        /// </summary>
        /// <remarks>
        ///     该接口只适合已经绑定过实名信息的用户再次认证其他新银行卡
        /// </remarks>
        /// <param name="request">
        ///     添加银行卡请求
        /// </param>
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     UBCABCBY1:无法添加银行卡
        ///     <br />
        ///     UBCABCBY2:请先进行实名认证
        ///     <br />
        ///     UBCABCBY3:最多绑定10张银行卡
        ///     <br />
        ///     UBCVBC4:该银行卡已经通过认证，请直接使用
        ///     <br />
        ///     UBCVBC5:该银行卡已经被使用
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [Route("VerifyBankCardByYilian")]
        [CookieAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> VerifyBankCardByYilian(VerifyBankCardRequest request)
        {
            if (await this.userInfoService.CheckBankCardUsedAsync(request.BankCardNo))
            {
                return this.BadRequest("UBCVBC5:该银行卡已经被使用");
            }

            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);
            if (userInfo == null)
            {
                return this.BadRequest("USERNOTFOUND:用户信息加载错误，请重新登陆");
            }

            if (userInfo.Closed)
            {
                return this.BadRequest("UC:该账户已经被锁定，请联系金银猫客服");
            }

            if (!userInfo.Verified)
            {
                return this.BadRequest("UBCVBC2:请先进行实名认证");
            }

            BankCardInfo bankCardInfo = await this.userInfoService.GetBankCardInfoAsync(userInfo.UserId, request.BankCardNo);

            if (bankCardInfo == null)
            {
                return this.BadRequest("UBCVBC3:请先添加银行卡");
            }

            if (bankCardInfo.VerifiedByYilian)
            {
                return this.BadRequest("UBCVBC4:该银行卡已经通过认证，请直接使用");
            }

            VerifyBankCard verifyBankCardCommand = this.BuildVerifyBankCardCommand(bankCardInfo);

            await this.userService.VerifyBankCardAsync(verifyBankCardCommand);

            return this.Ok();
        }

        /// <summary>
        ///     获取用户可以用于取现的银行卡信息
        /// </summary>
        /// <remarks>
        ///     会获取所有的银行卡信息，无分页功能，排序规则为按可提现额度从大到小排序
        /// </remarks>
        /// <response code="200">注册成功</response>
        /// <response code="400">UBWCI:无法获取用户信息</response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet]
        [Route("Withdrawalable")]
        [CookieAuthorize]
        [ResponseType(typeof(List<BankCardInfoResponse>))]
        public async Task<IHttpActionResult> WithdrawalableCardInfos()
        {
            List<BankCardInfo> cards = await this.userInfoService.GetWithdrawalableBankCardInfosAsync(this.CurrentUser.Id);

            return this.Ok(cards.OrderByDescending(c => c.WithdrawAmount).Select(c => c.ToResponse()).ToList());
        }

        private AddBankCard BuildAddBankCardCommand(AddBankCardRequest request)
        {
            return new AddBankCard
            {
                EntityId = this.CurrentUser.Id,
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                BankName = request.BankName,
                CityName = request.CityName,
                UserId = this.CurrentUser.Id
            };
        }

        private HideBankCard BuildHideBankCardCommand(DeleteBankCardRequest request)
        {
            return new HideBankCard
            {
                EntityId = this.CurrentUser.Id,
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                UserId = this.CurrentUser.Id
            };
        }

        private VerifyBankCard BuildVerifyBankCardCommand(AddBankCardRequest request)
        {
            return new VerifyBankCard
            {
                EntityId = this.CurrentUser.Id,
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                BankName = request.BankName,
                CityName = request.CityName,
                Cellphone = this.CurrentUser.Cellphone,
                UserId = this.CurrentUser.Id
            };
        }

        private VerifyBankCard BuildVerifyBankCardCommand(BankCardInfo bankCardInfo)
        {
            return new VerifyBankCard
            {
                EntityId = this.CurrentUser.Id,
                Args = this.BuildArgs(),
                BankCardNo = bankCardInfo.BankCardNo,
                BankName = bankCardInfo.BankName,
                CityName = bankCardInfo.CityName,
                Cellphone = this.CurrentUser.Cellphone,
                UserId = this.CurrentUser.Id
            };
        }
    }
}