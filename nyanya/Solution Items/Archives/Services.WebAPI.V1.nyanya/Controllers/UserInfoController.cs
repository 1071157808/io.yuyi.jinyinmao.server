// FileInformation: nyanya/Services.WebAPI.V1.nyanya/UserInfoController.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/12   11:33 AM

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Cqrs.Domain.Orders.Services.DTO;
using Cqrs.Domain.Orders.Services.Interfaces;
using Cqrs.Domain.Users.ReadModels;
using Cqrs.Domain.Users.Services.Interfaces;
using Services.WebAPI.Common.Controller;
using Services.WebAPI.V1.nyanya.Filters;
using Services.WebAPI.V1.nyanya.Models;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     用户信息
    /// </summary>
    [RoutePrefix("UserInfo")]
    public class UserInfoController : ApiControllerBase
    {
        #region Private Fields

        private readonly IOrderInfoService orderInfoService;
        private readonly IUserInfoService userInfoService;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserInfoController" /> class.
        /// </summary>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="orderInfoService">The order information service.</param>
        public UserInfoController(IUserInfoService userInfoService, IOrderInfoService orderInfoService)
        {
            this.userInfoService = userInfoService;
            this.orderInfoService = orderInfoService;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     获取用户银行卡信息
        /// </summary>
        /// <returns>
        ///     BankCardNo[string]: 银行卡号
        ///     CardBankName[string]: 银行名称
        ///     IsDefault[bool]: 是否是默认银行卡
        /// </returns>
        [HttpGet, Route("BankCards")]
        [TokenAuthorize]
        [ResponseType(typeof(List<BankCardInfoResponse>))]
        public async Task<IHttpActionResult> BankCards()
        {
            List<BankCardSummaryInfo> cards = await this.userInfoService.GetBankCardsInfoAsync(this.CurrentUser.Identifier);
            if (cards == null)
            {
                this.Warning("Can not load user date.");
                return this.BadRequest("暂时无法查询该用户信息。");
            }

            return this.Ok(cards.Select(c => c.ToBankCardInfoResponse()).ToList());
        }

        /// <summary>
        ///     获取即将结息的产品信息
        /// </summary>
        /// <returns>
        ///     - - Interest[decimal]: 收益
        ///     - - Principal[decimal]: 本金
        ///     - - ProductIdentifier[decimal]: 产品唯一标识
        ///     - - ProductName[decimal]: 产品名称
        ///     - - ProductNo[string]: 产品编号
        ///     - - ProductNumber[int]: 产品期数
        ///     - - ProductType[int]: 产品类型，10 => 银票、20 => 商票
        ///     - - SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息日期
        /// </returns>
        [HttpGet, Route("Index/Settlings")]
        [TokenAuthorize]
        [ResponseType(typeof(List<SettlingProductInfoResponse>))]
        public async Task<IHttpActionResult> GetSettlings()
        {
            IList<SettlingProductInfo> infos = await this.orderInfoService.GetSettlingProductInfosAsync(this.CurrentUser.Identifier);

            return this.Ok(infos.Select(i => i.ToSettlingProductInfoResponse()));
        }

        /// <summary>
        ///     获取用户账户信息
        /// </summary>
        /// <returns>
        ///     TotalInterest[decimal]: 总收益
        ///     ExpectedInterest[decimal]: 预期收益
        ///     InvestingPrincipal[decimal]: 在投金额
        ///     IncomePerMinute[decimal]: 以分钟为单位的赚钱速度
        ///     DefeatedPercent[int]: 打败的百分比
        /// </returns>
        [HttpGet, Route("Index")]
        [TokenAuthorize]
        [ResponseType(typeof(UserInvestingInfoResponse))]
        public async Task<IHttpActionResult> GetUserInfoIndex()
        {
            decimal maxIncomeSpeed = await this.orderInfoService.GetMaxIncomeSpeedAsync();
            decimal userIncomeSpeed = await this.orderInfoService.GetTheUserIncomeSpeedAsync(this.CurrentUser.Identifier);
            InvestingInfo investingInfo = await this.orderInfoService.GetInvestingInfoAsync(this.CurrentUser.Identifier);

            UserInvestingInfoResponse response = new UserInvestingInfoResponse(maxIncomeSpeed, userIncomeSpeed, investingInfo);
            return this.Ok(response);
        }

        /// <summary>
        ///     获取用户信息
        /// </summary>
        /// <returns>
        ///     BankCardNo[string]: 默认银行卡号
        ///     BankCardsCount[int]: 银行卡数量
        ///     CardBankName[string]: 默认银行卡名称
        ///     Cellphone[string]: 手机
        ///     Credential[int]: 证件 -1 => 无，0 => 身份证，1 => 护照，2 => 台湾，3 => 军人
        ///     CredentialNo[string]: 证件编号 (已加码，可以直接显示)
        ///     HasDefaultBankCard[bool]: 用户是否绑定了默认银行卡,如果绑定了，一定是认证成功了
        ///     HasSetPaymentPassword[bool]: 是否设定了支付密码
        ///     HasYSBInfo[bool]: 是否有银生宝的信息
        ///     LoginName[string]: 登录名
        ///     RealName[string]: 用户真实姓名
        ///     SignUpTime[string]: 用户注册时间
        ///     Verified[bool]: 用户是否通过实名认证，也表示是否开通了支付功能，也表示首张银行卡是否绑定成功
        ///     Verifing[bool]: 用户是否正在进行实名认证
        ///     YSBIdCard[string]: 银生宝认证的真实身份证号
        ///     YSBRealName[string]: 银生宝的认证的真实姓名
        /// </returns>
        [HttpGet, Route("")]
        [TokenAuthorize]
        [ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> Info()
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Identifier);
            if (userInfo == null)
            {
                this.Warning("Can not load user date.");
                return this.BadRequest("暂时无法查询该用户信息。");
            }

            return this.Ok(userInfo.ToUserInfoResponse());
        }

        /// <summary>
        ///     获取用户登录信息
        /// </summary>
        /// <returns>
        ///     Valid[bool]: 用户Cookie是否有效
        ///     LoginName[string]: 用户的登录名
        /// </returns>
        [Route("Login")]
        [HttpGet]
        [ResponseType(typeof(LoginInfoResponse))]
        public IHttpActionResult LoginInfo()
        {
            return this.Ok(this.CurrentUser.ToLoginInfoResponse());
        }

        #endregion Public Methods
    }
}