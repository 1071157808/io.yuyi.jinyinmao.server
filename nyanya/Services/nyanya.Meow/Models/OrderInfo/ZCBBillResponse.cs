using Cat.Domain.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Lib.Extensions;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     ZCBBillResponse
    /// </summary>
    public class ZCBBillResponse
    {
        /// <summary>
        ///     流水标示字段
        /// </summary>
        public string BillIdentifier { get; set; }

        /// <summary>
        ///     项目唯一标示
        /// </summary>
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        ///     交易类型 10认购 20提现
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        ///     交易金额
        /// </summary>
        public decimal Principal { get; set; }

        /// <summary>
        ///     银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        ///     开户行城市全称，如 上海|上海
        /// </summary>
        public string BankCardCity { get; set; }


        /// <summary>
        ///     流水状态 （10付款中 20认购成功 30认购失败 40取现已申请 50取现成功 60提现失败）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///     流水描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     预计提现到账时间
        /// </summary>
        public string DalayDate { get; set; }

        /// <summary>
        ///     协议名称
        /// </summary>
        public string AgreementName { get; set; }


    }

    internal static partial class ZCBBillResponseExtensions
    {
        #region Internal Methods
        internal static ZCBBillResponse ToZCBBillResponse(this ZCBBill zcbBill)
        {
            ZCBBillResponse item = new ZCBBillResponse
            {
                BillIdentifier = zcbBill.OrderIdentifier,
                ProductIdentifier = zcbBill.ProductIdentifier,
                CreateTime = zcbBill.CreateTime.ToMeowFormat(),
                Type = zcbBill.Type,
                Principal = zcbBill.Principal,
                BankCardNo = zcbBill.BankCardNo,
                BankName = zcbBill.BankName,
                BankCardCity = zcbBill.City,
                Status = (int)zcbBill.Status,
                Remark = zcbBill.Remark,
                DalayDate = zcbBill.DelayDate.ToMeowFormat(),
                AgreementName = zcbBill.AgreementName
            };
            return item;
        }
        #endregion Internal Methods
    }
}