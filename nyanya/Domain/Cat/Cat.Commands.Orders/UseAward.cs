using Domian.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Commands.Orders
{
    /// <summary>
    /// 用户中奖纪录请求类
    /// </summary>
    public class UseAward : ObjectCommand
    {
        /// <summary>
        ///     初始化<see cref="UseAward" />类的新实例.
        /// </summary>
        /// <param name="cellphone">用户手机号码</param>
        /// <param name="activityNo">活动编号</param>
        /// <param name="awardLevel">指定奖品</param>
        public UseAward(string cellphone, string activityNo, int awardLevel)
            : base("UseAward_" + cellphone)
        {
            Cellphone = cellphone;
            ActivityNo = activityNo;
            FixedAwardLevel = awardLevel;
        }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityNo { get; set; }

        /// <summary>
        /// 指定奖品
        /// </summary>
        public int FixedAwardLevel { get; set; }
    }
}
