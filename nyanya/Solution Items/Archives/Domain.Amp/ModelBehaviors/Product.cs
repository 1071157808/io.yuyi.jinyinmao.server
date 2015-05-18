// FileInformation: nyanya/Domain.Amp/Product.cs
// CreatedTime: 2014/03/30   8:11 PM
// LastUpdatedTime: 2014/04/27   2:56 PM

using System;
using Infrastructure.Cache.Couchbase;

namespace Domain.Amp.Models
{
    public partial class Product
    {
        #region SellingStatus enum

        public enum SellingStatus
        {
            /// <summary>
            ///     预售
            /// </summary>
            ForSale = 10, // 预售

            /// <summary>
            ///     在售
            /// </summary>
            OnSale = 20, // 在售

            /// <summary>
            ///     售罄,待起息
            /// </summary>
            Sold = 30, // 售罄

            /// <summary>
            ///     待还款
            /// </summary>
            ForRefund = 40, // 待还款

            /// <summary>
            ///     还款中
            /// </summary>
            Refunding = 50, // 还款中

            /// <summary>
            ///     完成
            /// </summary>
            Finished = 60 // 完成,已结束，募集结束
        }

        #endregion SellingStatus enum

        public int GetFundedNumber()
        {
            AmpProductCache cache = new AmpProductCache(this.ProductIdentifier);
            return cache.FundedNumber();
        }

        public int GetFundedPercentage()
        {
            int? totalNumber = this.TotalNumber;
            decimal fundedPercentage = totalNumber.HasValue ? ((decimal)this.GetFundedNumber() / totalNumber.Value * 100) : 100;
            return (int)Math.Floor(fundedPercentage);
        }

        public SellingStatus GetSellingStatus()
        {
            byte? status = this.SalesStatus;
            if (status == null) return SellingStatus.Finished;
            int salesStatus = status.Value;

            DateTime now = DateTime.Now;

            switch (salesStatus)
            {
                case 20:
                    if (now < this.PubBegin) return SellingStatus.ForSale;
                    if (now >= this.PubBegin && now <= this.PubEnd) return SellingStatus.OnSale;
                    if (now > this.PubEnd) return SellingStatus.Sold;
                    break;

                case 40:
                    if (this.RaiseStatus == 10) // 未募集结束
                    {
                        return now < this.ValueDay ? SellingStatus.Sold : SellingStatus.ForRefund;
                    }

                    if (this.RaiseStatus == 20) return SellingStatus.Finished; // 募集失败

                    if (this.RaiseStatus == 30 || this.RaiseStatus == 40) // 募集成功 || 开始起息
                    {
                        return now < this.SettleDay ? SellingStatus.ForRefund : SellingStatus.Refunding;
                    }

                    if (this.RaiseStatus == 50 || this.RaiseStatus == 60) // 已结息 || 已退款
                    {
                        return SellingStatus.Finished;
                    }
                    break;

                default:
                    return SellingStatus.Finished;
            }
            return SellingStatus.Finished;
        }
    }
}