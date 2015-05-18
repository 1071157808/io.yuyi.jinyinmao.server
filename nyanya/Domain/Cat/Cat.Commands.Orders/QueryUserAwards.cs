using Domian.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Commands.Orders
{
    public class QueryUserAwards : ObjectCommand
    {
        public QueryUserAwards(string cellphone)
            : base("ShareKey_" + cellphone)
        {
            this.Cellphone = cellphone;
        }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string Cellphone { get; set; }
    }
}