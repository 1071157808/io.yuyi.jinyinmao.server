using Cat.Commands.Orders;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Users.ReadModels;
using Cat.Domain.Users.Services.Interfaces;
using Domian.Bus;
using Infrastructure.Lib.Extensions;
using Newtonsoft.Json;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Cat.Filters;
using nyanya.Cat.Models.Luckhub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace nyanya.Cat.Controllers
{
    public class ActivityInfo
    {
        public string ActivityNo { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    /// <summary>
    ///     幸运大转盘
    /// </summary>
    [RoutePrefix("Luckhub")]
    public class LuckhubController : ApiControllerBase
    {
        private readonly ICommandBus commandBus;
        private readonly IExactUserInfoService userInfoService;
        private readonly IOrderInfoService orderInfoService;
        private static readonly List<int> DefaultList = new List<int>() { 20, 30, 40, 50, 60, 70, -1 };
        private static readonly DateTime BeginTime = new DateTime(2015, 1, 9);
        private const string ActivityNo = "AC150218";
        private const string ActivityNoNew = "AC150210";
        private static readonly DateTime EndTime = new DateTime(2015, 1, 17);

        private static readonly ActivityInfo ActivityNew = new ActivityInfo()
        {
            ActivityNo = "AC150210",
            BeginTime = new DateTime(2015, 2, 11),
            EndTime = new DateTime(2015, 2, 26)
        };

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="commandBus">commandBus</param>
        public LuckhubController(ICommandBus commandBus, IExactUserInfoService userInfoService, IOrderInfoService orderInfoService)
        {
            this.commandBus = commandBus;
            this.userInfoService = userInfoService;
            this.orderInfoService = orderInfoService;
        }

        /// <summary>
        ///     获取用户奖品信息
        /// </summary>
        /// <returns>
        /// 状态(10=>没有资格，20=>未领取未使用，30=>领取未使用，40=>已使用，50=>已过期，60=>奖品无效)
        /// </returns>
        [HttpGet, Route("GetUserCanPalyStatu")]
        [TokenAuthorize]
        [ResponseType(typeof(UserLuckStatusSumResponse))]
        public async Task<IHttpActionResult> GetUserCanPalyStatu()
        {
            return this.BadRequest("无法参加");
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Identifier);
            int awardStatus = 60;
            if (userInfo == null)
            {
                this.Warning("无法获取用户信息.{0}".FormatWith(this.CurrentUser.Identifier));
                return this.BadRequest("无法获取用户信息");
            }
            if (DateTime.Now < ActivityNew.BeginTime || DateTime.Now > ActivityNew.EndTime)
            {
                awardStatus = 50;
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
                        commandBus.ObjectExcute(new QueryLuckRecord(userInfo.UserIdentifier, ActivityNo));
                    var queryLuckRecordData =
                        JsonConvert.DeserializeObject(queryLuckRecordResult.Data.ToString(),
                            typeof(int)) as int?;
                    if (queryLuckRecordData == null)
                    {
                        Warning("无法获取抽奖信息.{0}".FormatWith(CurrentUser.Identifier));
                        return BadRequest("无法获取抽奖信息");
                    }
                    else
                    {
                        awardStatus = queryLuckRecordData.Value;
                    }
                }
            }
            return Ok(new
            {
                status = awardStatus
            });
        }

        /// <summary>
        ///     获取用户奖品信息
        /// </summary>
        /// <returns>
        ///     Status[int]：状态(20=>可以使用奖，30=>使用奖次数已满，40=>已过期)
        ///     UserLuckStatuResponses[array]:奖品信息列表,如果Status==40,返回空
        ///     -- AwardLevel[int]：奖品等级（20=>一等奖（0.6%），30=>二等奖（0.5%），40=>三等奖（0.4%），50=>四等奖（0.3%），60=>五等奖（0.2%）,70=>六等奖（0.1%）,-1=>谢谢参与）
        ///     -- AwardCount[int] : 奖品数量(0-100)
        /// </returns>
        [HttpGet, Route("GetUserGetStatus")]
        [ParameterRequire("cellphone")]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> GetUserGetStatus(string cellphone)
        {
            return this.BadRequest("无法参加");
            if (DateTime.Now < ActivityNew.BeginTime || DateTime.Now > ActivityNew.EndTime)
            {
                return BadRequest("活动已过期");
            }
            //查询该用户是否抽过奖
            var queryLuckRecordResult = commandBus.ObjectExcute(new QueryPhoneLuckRecord(cellphone, ActivityNew.ActivityNo));
            var queryLuckRecordData = JsonConvert.DeserializeObject(queryLuckRecordResult.Data.ToString(), typeof(bool?)) as bool?;
            if (queryLuckRecordData == null)
            {
                Warning("无法获取抽奖信息.{0}".FormatWith(CurrentUser.Identifier));
                return BadRequest("无法获取抽奖信息");
            }
            return this.Ok(new
            {
                IsGet = (bool)queryLuckRecordData ? "true" : "false"
            });
        }

        /// <summary>
        ///     抽奖,获取用户奖品
        /// </summary>
        /// <returns>
        ///     Status[int]：状态(20=>可得奖，30=>得奖次数已满，40=>已过期)
        ///     AwardLevel[int]：奖品等级（20=>一等奖（0.6%），30=>二等奖（0.5%），40=>三等奖（0.4%），50=>四等奖（0.3%），60=>五等奖（0.2%）,70=>六等奖（0.1%）,-1=>谢谢参与）
        ///     UserDiceNum[int]: 用户点数（可抽奖情况下，返回1到6）
        ///     ServerDiceNum[int]： 系统点数（可抽奖情况下，返回1到6）
        /// </returns>
        [HttpPost, Route("BuildLuckRecord")]
        [ResponseType(typeof(BuildDiceLuckRecordResponse))]
        public async Task<IHttpActionResult> BuildLuckRecord(LuckUserRequest request)
        {
            return this.BadRequest("无法参加");
            if (DateTime.Now < ActivityNew.BeginTime || DateTime.Now > ActivityNew.EndTime)
            {
                return BadRequest("活动已过期");
            }
            UserInfo userInfo = await userInfoService.GetUserInfoAsync(CurrentUser.Identifier);
            if (userInfo != null && (request == null || request.Cellphone.IsNullOrEmpty()))
            {
                var queryLuckRecordResult = commandBus.ObjectExcute(new UseAward(userInfo.UserIdentifier, ActivityNew.ActivityNo, 10));
                var queryLuckRecordData = JsonConvert.DeserializeObject(queryLuckRecordResult.Data.ToString(), typeof(BuildLuckRecordResponse)) as BuildLuckRecordResponse;
                if (queryLuckRecordData == null || queryLuckRecordData.Status == 0) { return this.BadRequest("无法获取抽奖信息"); }
                else
                {
                    if (queryLuckRecordData.Status == 20)
                    {
                        return this.OK();
                    }
                    if (queryLuckRecordData.Status == 30)
                    {
                        return this.BadRequest("已使用");
                    }
                    if (queryLuckRecordData.Status == 40)
                    {
                        return this.BadRequest("未领取");
                    }
                    return BadRequest("无法获取抽奖信息");
                }
            }
            if (userInfo == null && request != null && request.Cellphone.IsNotNullOrEmpty())
            {
                var result = commandBus.ObjectExcute(new BuildPhoneLuckRecord(request.Cellphone, 10, ActivityNo));
                var buildLuckRecordResult = JsonConvert.DeserializeObject(result.Data.ToString(), typeof(BuildLuckRecordResponse)) as BuildLuckRecordResponse;
                if (buildLuckRecordResult == null || buildLuckRecordResult.Status == 0) { return this.BadRequest("无法获取抽奖信息"); }
                else
                {
                    if (buildLuckRecordResult.Status == 20)
                    {
                        return this.OK();
                    } if (buildLuckRecordResult.Status == 30)
                    {
                        return this.BadRequest("已领取");
                    }
                }
            }
            return BadRequest("无法获取抽奖信息");
        }

        private BuildLuckRecordResponse BuildLuckRecords(UserInfo userInfo)
        {
            BuildLuckRecord record = new BuildLuckRecord(userInfo.UserIdentifier, 10, ActivityNo, 20);

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
