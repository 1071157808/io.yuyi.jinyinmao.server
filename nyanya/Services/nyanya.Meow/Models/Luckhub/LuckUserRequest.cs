using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyanya.Meow.Models.Luckhub
{
    public class LuckUserRequest
    {
        public string Cellphone { get; set; }

        /// <summary>
        ///     验证手机用TOKEN
        /// </summary>
        public string Token { get; set; }
    }
}