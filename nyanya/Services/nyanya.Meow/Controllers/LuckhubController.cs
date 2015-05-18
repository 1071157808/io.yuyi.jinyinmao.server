using Cat.Commands.Orders;
using Cat.Domain.Auth.Models;
using Cat.Domain.Auth.Services.DTO;
using Cat.Domain.Auth.Services.Interfaces;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Users.ReadModels;
using Cat.Domain.Users.Services.Interfaces;
using Domian.Bus;
using Infrastructure.Lib.Extensions;
using Newtonsoft.Json;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Meow.Filters;
using nyanya.Meow.Models.Luckhub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace nyanya.Meow.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class ActivityInfo
    {
        /// <summary>
        /// Gets or sets the activity no.
        /// </summary>
        /// <value>
        /// The activity no.
        /// </value>
        public string ActivityNo { get; set; }

        /// <summary>
        /// Gets or sets the begin time.
        /// </summary>
        /// <value>
        /// The begin time.
        /// </value>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime EndTime { get; set; }
    }

    /// <summary>
    /// 幸运大转盘
    /// </summary>
    [RoutePrefix("Luckhub")]
    public class LuckhubController : ApiControllerBase
    {
        /// <summary>
        /// The command bus
        /// </summary>
        private readonly ICommandBus commandBus;

        /// <summary>
        /// The user information service
        /// </summary>
        private readonly IExactUserInfoService userInfoService;

        /// <summary>
        /// The order information service
        /// </summary>
        private readonly IOrderInfoService orderInfoService;

        /// <summary>
        /// The default list
        /// </summary>
        private static readonly List<int> DefaultList = new List<int>() { 20, 30, 40, 50, 60, 70, -1 };

        /// <summary>
        /// The begin time
        /// </summary>
        private static readonly DateTime BeginTime = new DateTime(2015, 1, 9);

        /// <summary>
        ///     The user service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// The activity no
        /// </summary>
        //private const string ActivityNo = "AC150218";

        /// <summary>
        /// The activity no new
        /// </summary>
        private const string ActivityNoNew = "AC150210";

        /// <summary>
        /// The end time
        /// </summary>
        private static readonly DateTime EndTime = new DateTime(2015, 1, 17);

        /// <summary>
        /// The activity new
        /// </summary>
        private static readonly ActivityInfo ActivityNew = new ActivityInfo()
        {
            ActivityNo = "AC150210",
            BeginTime = new DateTime(2015, 2, 11),
            EndTime = new DateTime(2015, 2, 26)
        };

        /// <summary>
        /// The veri code service
        /// </summary>
        private readonly IVeriCodeService veriCodeService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandBus">commandBus</param>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="orderInfoService">The order information service.</param>
        /// <param name="veriCodeService">The veri code service.</param>
        /// <param name="userService"></param>
        public LuckhubController(ICommandBus commandBus, IExactUserInfoService userInfoService, IOrderInfoService orderInfoService, IVeriCodeService veriCodeService, IUserService userService)
        {
            this.commandBus = commandBus;
            this.userInfoService = userInfoService;
            this.orderInfoService = orderInfoService;
            this.veriCodeService = veriCodeService;
            this.userService = userService;
        }

        /// <summary>
        /// 获取用户奖品信息,登录状态下
        /// </summary>
        /// <returns>
        /// 状态(10=没有资格，20=未领取未使用，30=领取未使用，40=已使用，50=已过期，60=奖品无效)
        /// </returns>
        [HttpGet, Route("GetUserCanPalyStatu")]
        [TokenAuthorize]
        [ResponseType(typeof(UserLuckStatuResponse))]
        public async Task<IHttpActionResult> GetUserCanPalyStatu()
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Identifier);
            int awardStatus = 60;
            if (userInfo == null)
            {
                this.Warning("无法获取用户信息.{0}".FormatWith(this.CurrentUser.Identifier));
                return this.BadRequest("无法获取用户信息");
            }
            if (DateTime.Now < ActivityNew.BeginTime)
            {
                return this.BadRequest("活动未开始");
            }
            if (DateTime.Now > ActivityNew.EndTime)
            {
                return this.BadRequest("活动已过期");
            }
            else
            {
                if (userInfo.SignUpTime < ActivityNew.BeginTime)
                {
                    awardStatus = 10;
                }
                else
                {
                    //查询该用户是否抽过奖
                    var queryLuckRecordResult =
                        commandBus.ObjectExcute(new QueryLuckRecord(userInfo.Cellphone, ActivityNew.ActivityNo));
                    var queryLuckRecordData =
                        JsonConvert.DeserializeObject(queryLuckRecordResult.Data.ToString(),
                            typeof(int)) as int?;
                    if (queryLuckRecordData == null)
                    {
                        Warning("无法获取抽奖信息.{0}".FormatWith(CurrentUser.Identifier));
                        return this.BadRequest("无法获取抽奖信息");
                    }
                    else
                    {
                        awardStatus = queryLuckRecordData.Value;
                    }
                }
            }
            return Ok(new UserLuckStatuResponse
            {
                Status = awardStatus
            });
        }

        /// <summary>
        /// 获取用户奖品信息(非登录状态）
        /// </summary>
        /// <param name="cellphone">The request.
        ///     Cellphone：手机号
        /// </param>
        /// <returns>
        /// 状态(true=>已领取，false=>未领取)
        /// </returns>
        [HttpGet, Route("GetUserGetStatus")]
        [ParameterRequire("cellphone")]
        [ResponseType(typeof(UserGetLuckResponse))]
        public async Task<IHttpActionResult> GetUserGetStatus(string cellphone)
        {
            if (DateTime.Now < ActivityNew.BeginTime)
            {
                return this.BadRequest("活动未开始");
            }
            if (DateTime.Now > ActivityNew.EndTime)
            {
                return this.BadRequest("活动已过期");
            }

            /*            if ((await userService.CheckCellPhoneAsync(request.Cellphone)).Result)
                        {
                            return this.BadRequest("号码已注册");
                        }*/

            //查询该用户是否抽过奖
            var queryLuckRecordResult = commandBus.ObjectExcute(new QueryPhoneLuckRecord(cellphone, ActivityNew.ActivityNo));

            var queryLuckRecordData = JsonConvert.DeserializeObject(queryLuckRecordResult.Data.ToString(), typeof(bool?)) as bool?;
            if (queryLuckRecordData == null)
            {
                Warning("无法获信息.{0}".FormatWith(CurrentUser.Identifier));

                return this.BadRequest("无法获取信息");
            }

            return this.Ok(new UserGetLuckResponse
            {
                IsGet = (bool)queryLuckRecordData
            });
        }

        /*        /// <summary>
                ///     获取用户奖品信息
                /// </summary>
                /// <returns>
                ///     Status[int]：状态(20=>可以使用奖，30=>使用奖次数已满，40=>已过期)
                ///     UserLuckStatuResponses[array]:奖品信息列表,如果Status==40,返回空
                ///     -- AwardLevel[int]：奖品等级（20=>一等奖（0.6%），30=>二等奖（0.5%），40=>三等奖（0.4%），50=>四等奖（0.3%），60=>五等奖（0.2%）,70=>六等奖（0.1%）,-1=>谢谢参与）
                ///     -- AwardCount[int] : 奖品数量(0-100)
                /// </returns>
                [HttpGet, Route("GetUserGetStatus")]
                [ResponseType(typeof(string))]
                public async Task<IHttpActionResult> GetUserShareKey()
                {
                    if (DateTime.Now < ActivityNew.BeginTime || DateTime.Now > ActivityNew.EndTime)
                    {
                        return BadRequest("活动已过期");
                    }
                    //查询该用户是否抽过奖
                    UserInfo userInfo = await userInfoService.GetUserInfoAsync(CurrentUser.Identifier);
                    var queryLuckRecordResult = commandBus.ObjectExcute(new BuildUserShareKey(userInfo.Cellphone));
                    var queryLuckRecordData = JsonConvert.DeserializeObject(queryLuckRecordResult.Data.ToString(), typeof(bool?)) as bool?;
                    if (queryLuckRecordData == null)
                    {
                        Warning("无法获取抽奖信息.{0}".FormatWith(CurrentUser.Identifier));
                        return BadRequest("无法获取抽奖信息");
                    }
                    return this.Ok(new
                    {
                        ShareKey = userInfo.Cellphone
                    });
                }

                /// <summary>
                /// 获取用户奖品信息
                /// </summary>
                /// <returns>
                /// Status[int]：状态(20=&gt;可以使用奖，30=&gt;使用奖次数已满，40=&gt;已过期)
                /// UserLuckStatuResponses[array]:奖品信息列表,如果Status==40,返回空
                /// -- AwardLevel[int]：奖品等级（20=&gt;一等奖（0.6%），30=&gt;二等奖（0.5%），40=&gt;三等奖（0.4%），50=&gt;四等奖（0.3%），60=&gt;五等奖（0.2%）,70=&gt;六等奖（0.1%）,-1=&gt;谢谢参与）
                /// -- AwardCount[int] : 奖品数量(0-100)
                /// </returns>
                [HttpGet, Route("GetUserGetStatus")]
                [ResponseType(typeof(string))]
                public async Task<IHttpActionResult> GetUserAwardsInfo()
                {
                    if (DateTime.Now < ActivityNew.BeginTime || DateTime.Now > ActivityNew.EndTime)
                    {
                        return BadRequest("活动已过期");
                    }
                    //查询该用户是否抽过奖
                    UserInfo userInfo = await userInfoService.GetUserInfoAsync(CurrentUser.Identifier);
                    var queryLuckRecordResult = commandBus.ObjectExcute(new QueryUserAwards(userInfo.Cellphone));
                    var queryLuckRecordData = JsonConvert.DeserializeObject(queryLuckRecordResult.Data.ToString(), typeof(bool?)) as bool?;
                    if (queryLuckRecordData == null)
                    {
                        Warning("无法获取抽奖信息.{0}".FormatWith(CurrentUser.Identifier));
                        return BadRequest("无法获取抽奖信息");
                    }
                    return this.Ok(new
                    {
                        UserAwards = queryLuckRecordData
                    });
                }

                /// <summary>
                /// 获取用户奖品信息
                /// </summary>
                /// <returns>
                /// Status[int]：状态(20=&gt;可以使用奖，30=&gt;使用奖次数已满，40=&gt;已过期)
                /// UserLuckStatuResponses[array]:奖品信息列表,如果Status==40,返回空
                /// -- AwardLevel[int]：奖品等级（20=&gt;一等奖（0.6%），30=&gt;二等奖（0.5%），40=&gt;三等奖（0.4%），50=&gt;四等奖（0.3%），60=&gt;五等奖（0.2%）,70=&gt;六等奖（0.1%）,-1=&gt;谢谢参与）
                /// -- AwardCount[int] : 奖品数量(0-100)
                /// </returns>
                [HttpPost, Route("GetUserGetStatus")]
                [ResponseType(typeof(string))]
                public async Task<IHttpActionResult> OpenAward(LuckUserRequest request)
                {
                    if (DateTime.Now < ActivityNew.BeginTime || DateTime.Now > ActivityNew.EndTime)
                    {
                        return BadRequest("活动已过期");
                    }
                    //查询该用户是否抽过奖
                    UserInfo userInfo = await userInfoService.GetUserInfoAsync(CurrentUser.Identifier);
                    var queryLuckRecordResult = commandBus.ObjectExcute(new OpenUserAward(userInfo.Cellphone, request.Cellphone));
                    var queryLuckRecordData = JsonConvert.DeserializeObject(queryLuckRecordResult.Data.ToString(), typeof(bool?)) as bool?;
                    if (queryLuckRecordData == null)
                    {
                        Warning("无法获取抽奖信息.{0}".FormatWith(CurrentUser.Identifier));
                        return BadRequest("无法获取抽奖信息");
                    }
                    return this.Ok(new
                    {
                        UserAwards = queryLuckRecordData
                    });
                }*/

        /// <summary>
        /// 领取或使用用户奖品
        /// </summary>
        /// <param name="request">The request.
        ///      Cellphone：手机号（领取时填，使用时为空）
        ///     Token:图形验证码验证成功后的Token（领取时填，使用时不检查）
        /// </param>
        /// <returns>
        /// 状态(10=没有资格，20=未领取未使用，30=领取成功，40=之前已领取，50=使用成功，60=之前已使用，70=已过期，80=奖品无效,90=验证码错误)
        /// </returns>
        [HttpPost, Route("BuildLuckRecord")]
        [ResponseType(typeof(UserLuckStatuResponse))]
        public async Task<IHttpActionResult> BuildLuckRecord(LuckUserRequest request)
        {
            if (DateTime.Now < ActivityNew.BeginTime)
            {
                return this.BadRequest("活动未开始");
            }
            if (DateTime.Now > ActivityNew.EndTime)
            {
                return this.BadRequest("活动已过期");
            }
            int status = 70;
            UserInfo userInfo = await userInfoService.GetUserInfoAsync(CurrentUser.Identifier);
            //使用奖品
            if (userInfo != null && (request == null || request.Cellphone.IsNullOrEmpty()))
            {
                var queryLuckRecordResult = commandBus.ObjectExcute(new UseAward(userInfo.Cellphone, ActivityNew.ActivityNo, 10));
                var queryLuckRecordData =
                    JsonConvert.DeserializeObject(queryLuckRecordResult.Data.ToString(), typeof(int)) as int?;
                if (queryLuckRecordData == null || queryLuckRecordData == 0) { return this.BadRequest("无法获取使用信息"); }
                else
                {
                    if (queryLuckRecordData == 30)
                    {
                        status = 50;
                    }
                    else if (queryLuckRecordData == 40)
                    {
                        status = 60;
                    }
                    else if (queryLuckRecordData == 20)
                    {
                        status = 20;
                    }
                    return this.Ok(new UserLuckStatuResponse
                    {
                        Status = status
                    });
                }
            }
            //登录状态去领取
            if (userInfo != null && request != null && request.Cellphone.IsNotNullOrEmpty())
            {
                status = 10;
                return this.Ok(new UserLuckStatuResponse { Status = status });
            }
            //未登录状态去领取
            if (userInfo == null && request != null && request.Cellphone.IsNotNullOrEmpty())
            {
                UseVeriCodeResult usedResult = await this.veriCodeService.UseAsync(request.Token, VeriCode.VeriCodeType.VeriImage);
                if (!usedResult.Result)
                {
                    return this.Ok(new UserLuckStatuResponse
                    {
                        Status = 90
                    });
                }
                var user = await userInfoService.GetLoginInfoAsync(request.Cellphone);
                if (user != null && user.SignUpTime < ActivityNew.BeginTime)
                {
                    return this.Ok(new UserLuckStatuResponse
                    {
                        Status = 10
                    });
                }
                var result = commandBus.ObjectExcute(new BuildPhoneLuckRecord(request.Cellphone, 10, ActivityNew.ActivityNo));
                var buildLuckRecordResult = JsonConvert.DeserializeObject(result.Data.ToString(), typeof(int)) as int?;
                if (buildLuckRecordResult == null || buildLuckRecordResult == 0) { return this.BadRequest("无法获取信息"); }
                else
                {
                    if (buildLuckRecordResult == 20)
                    {
                        status = 30;
                    } if (buildLuckRecordResult == 30)
                    {
                        status = 40;
                    }
                    return this.Ok(new UserLuckStatuResponse
                    {
                        Status = status
                    });
                }
            }
            return this.BadRequest("无法获取信息");
        }

        /// <summary>
        /// Builds the luck records.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns></returns>
        private BuildLuckRecordResponse BuildLuckRecords(UserInfo userInfo)
        {
            BuildLuckRecord record = new BuildLuckRecord(userInfo.UserIdentifier, 10, ActivityNew.ActivityNo, 20);

            var result = commandBus.ObjectExcute(record);
            var buildLuckRecordResult = JsonConvert.DeserializeObject(result.Data.ToString(), typeof(BuildLuckRecordResponse)) as BuildLuckRecordResponse;
            if (buildLuckRecordResult != null)
            {
                return buildLuckRecordResult;
            }
            return null;
        }
    }
}
