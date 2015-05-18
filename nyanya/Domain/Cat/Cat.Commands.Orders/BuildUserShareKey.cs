using Domian.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Commands.Orders
{
    public class BuildUserShareKey : ObjectCommand
    {
        public BuildUserShareKey(string key)
            : base("ShareKey_" + key)
        {
        }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string ShareKey { get; set; }
    }
}