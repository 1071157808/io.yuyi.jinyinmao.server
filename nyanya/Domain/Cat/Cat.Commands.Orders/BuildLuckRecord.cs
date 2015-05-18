using System;
using Domian.Commands;
namespace Cat.Commands.Orders
{
    /// <summary>
    /// 生成抽奖记录
    /// </summary>
    public class BuildLuckRecord : ObjectCommand
    {
        /// <summary>
        ///     初始化<see cref="BuildLuckRecord" />类的新实例.
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="systemSource">系统来源</param>
        /// <param name="activityNo">活动编号</param>
        public BuildLuckRecord(string userIdentifier, int systemSource, string activityNo, int fixedAwardLevel = -1)
            : base("USER_" + userIdentifier)
        {
            UserIdentifier = userIdentifier;
            SystemSource = systemSource;
            ActivityNo = activityNo;
            FixedAwardLevel = fixedAwardLevel;
        }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserIdentifier { get; set; }

        /// <summary>
        /// 系统来源( 10=>金银猫 )
        /// </summary>
        public int SystemSource { get; set; }

        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityNo { get; set; }

        /// <summary>
        /// 指定奖品
        /// </summary>
        public int FixedAwardLevel { get; set; }
    }
    /// <summary>
    /// 生成抽奖记录结果
    /// </summary>
    public class BuildLuckRecordResult 
    {
        /// <summary>
        /// 状态(10=>没有资格，20=>有资格未抽奖，30=>有资格已抽奖，40=>已过期，50=>奖品无效)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 奖品等级（10=>特等奖，20=>一等奖，30=>二等奖，40=>三等奖，50=>四等奖，60=>五等奖）
        /// </summary>
        public int AwardLevel { get; set; }
    }
}