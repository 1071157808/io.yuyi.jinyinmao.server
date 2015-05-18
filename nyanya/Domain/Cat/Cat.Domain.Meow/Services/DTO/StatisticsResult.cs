using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Meow.Services.DTO
{
    public class StatisticsResult
    {
        /// <summary>
        /// 用户注册成功数
        /// </summary>
        public int RegisterUserNum { get; set; }
        /// <summary>
        /// 成功登录数
        /// </summary>
        public int SuccessLoginNum { get; set; }
        /// <summary>
        /// 失败登录数
        /// </summary>
        public int FailedLoginNum { get; set; }
        /// <summary>
        /// 成功绑卡数
        /// </summary>
        public int SuccessBankCardNum { get; set; }
        /// <summary>
        /// 失败绑卡数
        /// </summary>
        public int FailedBankCardNum { get; set; }
        /// <summary>
        /// 成功订单数
        /// </summary>
        public int SuccessOrderNum { get; set; }
        /// <summary>
        /// 失败订单数（不包含“余额不足”）
        /// </summary>
        public int FailedOrderNum { get; set; }
        /// <summary>
        /// 在售产品数
        /// </summary>
        public int OnSaleProductNum { get; set; }
    }
}
