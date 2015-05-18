using Cat.Domain.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Lib.Extensions;

namespace nyanya.Cat.Models
{
    /// <summary>
    /// RedeemParametersResponse
    /// </summary>
    public class RedeemParametersResponse
    {
        /// <summary>
        /// RedeemParametersResponse
        /// </summary>
        public RedeemParametersResponse()
        {
        }

        /// <summary>
        /// RedeemParametersResponse
        /// </summary>
        /// <param name="redeemCount"></param>
        /// <param name="todayIsInvesting"></param>
        /// <param name="investingAndUnRedeemPrincipal"></param>
        public RedeemParametersResponse(int redeemCount, int todayIsInvesting, decimal investingAndUnRedeemPrincipal)
        {
            this.RedeemCount = redeemCount;
            this.TodayIsInvesting = todayIsInvesting;
            this.InvestingAndUnRedeemPrincipal = investingAndUnRedeemPrincipal;
        }
        /// <summary>
        /// 用户当日已提现次数
        /// </summary>
        public int RedeemCount { get; set; }

        /// <summary>
        /// 今天是否有投资产品 10是 20否 
        /// </summary>
        public int TodayIsInvesting { get; set; }

        /// <summary>
        /// 今天投资金额和还未处理的提现金额的总和
        /// </summary>
        public decimal InvestingAndUnRedeemPrincipal { get; set; }
    }
}