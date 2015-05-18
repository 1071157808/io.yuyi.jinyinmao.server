using Domian.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Commands.Orders
{
    public class OpenUserAward : ObjectCommand
    {
        public OpenUserAward(string key, string cellphone)
            : base("ShareKey_" + key)
        {
            ShareKey = key;
            Cellphone = cellphone;
        }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string ShareKey { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string Cellphone { get; set; }
    }
}