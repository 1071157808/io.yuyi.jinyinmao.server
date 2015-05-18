using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xingye.Domain.Products.Models
{
    public partial class TradeAcceptanceProduct : Product
    {
        /// <summary>
        /// 担保方信息
        /// </summary>
        public string SecuredpartyInfo { get; set; }

        /// <summary>
        /// 质押借款协议名
        /// </summary>
        public string PledgeAgreementName { get; set; }

        /// <summary>
        /// 委托协议名
        /// </summary>
        public string ConsignmentAgreementName { get; set; }
        /// <summary>
        /// 担保方
        /// </summary>
        public string Securedparty { get; set; }

        /// <summary>
        /// 商承票号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 融资企业营业执照编号
        /// </summary>
        public string EnterpriseLicense { get; set; }

        /// <summary>
        /// 融资企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 融资用途
        /// </summary>
        public string Usage { get; set; }

        /// <summary>
        /// 付款方
        /// </summary>
        public string Drawee { get; set; }

        /// <summary>
        /// 付款方信息
        /// </summary>
        public string DraweeInfo { get; set; }

        /// <summary>
        /// 融资方信息
        /// </summary>
        public string EnterpriseInfo { get; set; }
    }
}
