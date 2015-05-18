using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyanya.Cat.Models.Luckhub
{
    /// <summary>
    ///
    /// </summary>
    public class UserLuckStatusSumResponse
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<UserLuckStatuResponse> UserLuckStatuResponses { get; set; }
    }
}