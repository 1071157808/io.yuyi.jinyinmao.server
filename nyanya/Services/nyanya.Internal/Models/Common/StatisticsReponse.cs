using Cat.Domain.Meow.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyanya.Internal.Models
{
    /// <summary>
    /// StatisticsReponse
    /// </summary>
    public class StatisticsReponse
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

    internal static partial class StatisticsReponseExtensions
    {
        #region Internal Methods

        internal static StatisticsReponse ToStatisticsReponse(this StatisticsResult statistics)
        {
            StatisticsReponse statisticsReponse = new StatisticsReponse
            {
                RegisterUserNum = statistics.RegisterUserNum,
                SuccessLoginNum = statistics.SuccessLoginNum,
                FailedLoginNum = statistics.FailedLoginNum,
                SuccessBankCardNum = statistics.SuccessBankCardNum,
                FailedBankCardNum = statistics.FailedBankCardNum,
                SuccessOrderNum = statistics.SuccessOrderNum,
                FailedOrderNum = statistics.FailedOrderNum,
                OnSaleProductNum = statistics.OnSaleProductNum
            };
            return statisticsReponse;
        }
        #endregion Internal Methods
    }
}