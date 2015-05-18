using Domian.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Commands.Orders
{
    public class BuildPhoneLuckRecord : ObjectCommand
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="cellphone">手机号</param>
        /// <param name="systemSource">系统来源</param>
        /// <param name="activityNo">活动编号</param>
        public BuildPhoneLuckRecord(string cellphone, int systemSource, string activityNo, int fixedAwardLevel = -1)
            : base("Phone_" + cellphone)
        {
            Cellphone = cellphone;
            SystemSource = systemSource;
            ActivityNo = activityNo;
            FixedAwardLevel = fixedAwardLevel;
        }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string Cellphone { get; set; }

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
    public class BuildPhoneLuckRecordResult
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