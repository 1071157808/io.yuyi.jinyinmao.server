using Domian.Commands;
using System;

namespace Cat.Commands.Orders
{
    /// <summary>
    /// 查询抽奖记录
    /// </summary>
    public class QueryLuckRecord : ObjectCommand
    {
        /// <summary>
        ///     初始化<see cref="QueryLuckRecord" />类的新实例.
        /// </summary>
        /// <param name="cellphone">用户手机</param>
        /// <param name="activityNo">活动编号</param>
        public QueryLuckRecord(string cellphone, string activityNo)
            : base("QueryLuckRecord_" + cellphone)
        {
            Cellphone = cellphone;
            ActivityNo = activityNo;
        }

        /// <summary>
        /// 用户手机
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityNo { get; set; }
    }

    /// <summary>
    /// 查询抽奖记录结果
    /// </summary>
    public class QueryLuckRecordResult
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