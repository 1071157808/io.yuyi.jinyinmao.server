// FileInformation: nyanya/nyanya.Meow/UserInvestingInfoResponse.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:28 PM

using System;
using Cat.Domain.Orders.Services.DTO;
using Infrastructure.Lib.Extensions;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     UserInvestingInfoResponse
    /// </summary>
    public class UserInvestingInfoResponse
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserInvestingInfoResponse" /> class.
        /// </summary>
        public UserInvestingInfoResponse()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserInvestingInfoResponse" /> class.
        /// </summary>
        /// <param name="maxIncomeSpeed">The maximum income speed.</param>
        /// <param name="userIncomeSpeed">The user income speed.</param>
        /// <param name="investingInfo">The investing information.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public UserInvestingInfoResponse(decimal maxIncomeSpeed, decimal userIncomeSpeed, InvestingInfo investingInfo)
        {
            this.ExpectedInterest = decimal.Round(investingInfo.Interest, 2, MidpointRounding.AwayFromZero);
            this.IncomePerMinute = decimal.Round((userIncomeSpeed / (360 * 24 * 60)), 10);
            this.InvestingPrincipal = investingInfo.Principal.ToIntFormat();
            this.TotalInterest = decimal.Round(investingInfo.TotalInterest, 2, MidpointRounding.AwayFromZero);
            this.DefeatedPercent = (userIncomeSpeed > maxIncomeSpeed) ? 99
                : decimal.ToInt32((Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(userIncomeSpeed / maxIncomeSpeed))) * 100));
        }

        /// <summary>
        ///     打败的百分比
        /// </summary>
        public int DefeatedPercent { get; set; }

        /// <summary>
        ///     预期收益
        /// </summary>
        public decimal ExpectedInterest { get; set; }

        /// <summary>
        ///     每分钟赚取的收益
        /// </summary>
        public decimal IncomePerMinute { get; set; }

        /// <summary>
        ///     在投金额
        /// </summary>
        public decimal InvestingPrincipal { get; set; }

        /// <summary>
        ///     已获收益
        /// </summary>
        public decimal TotalInterest { get; set; }
    }
}