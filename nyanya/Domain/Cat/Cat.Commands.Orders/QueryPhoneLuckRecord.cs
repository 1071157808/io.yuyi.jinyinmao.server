using Domian.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Commands.Orders
{
    public class QueryPhoneLuckRecord : ObjectCommand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userIdentifier">手机号</param>
        /// <param name="activityNo">活动编号</param>
        public QueryPhoneLuckRecord(string cellphone, string activityNo)
            : base("QueryLuckRecord_" + cellphone)
        {
            Cellphone = cellphone;
            ActivityNo = activityNo;
        }

        /// <summary>
        ///手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityNo { get; set; }
    }
}