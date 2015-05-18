// FileInformation: nyanya/nyanya.Cat/TAProductInfoResponse.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/17   12:28 PM

using System;
using Cat.Commands.Products;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Infrastructure.Lib.Extensions;
using nyanya.Cat.Helper;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     商票信息
    /// </summary>
    public class TAProductInfoResponse : ProductInfoResponse
    {
        /// <summary>
        ///     商承票号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        ///     委托协议名
        /// </summary>
        public string ConsignmentAgreementName { get; set; }

        /// <summary>
        ///     付款方
        /// </summary>
        public string Drawee { get; set; }

        /// <summary>
        ///     付款方信息
        /// </summary>
        public string DraweeInfo { get; set; }

        /// <summary>
        ///     融资方信息
        /// </summary>
        public string EnterpriseInfo { get; set; }

        /// <summary>
        ///     融资企业营业执照编号
        /// </summary>
        public string EnterpriseLicense { get; set; }

        /// <summary>
        ///     融资企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        ///     担保方式
        /// </summary>
        public GuaranteeMode GuaranteeMode { get; set; }

        /// <summary>
        ///     质押借款协议名
        /// </summary>
        public string PledgeAgreementName { get; set; }

        /// <summary>
        ///     担保方
        /// </summary>
        public string Securedparty { get; set; }

        /// <summary>
        ///     担保方信息
        /// </summary>
        public string SecuredpartyInfo { get; set; }

        /// <summary>
        ///     融资用途
        /// </summary>
        public string Usage { get; set; }

        /// <summary>
        ///     产品分类（10金银猫产品 30施秉金鼎）
        /// </summary>
        public ProductCategory ProductCategory { get; set; }
    }

    internal static class TAProductWithSaleInfoExtensions
    {
        internal static TAProductInfoResponse ToTAProductInfoResponse(this ProductWithSaleInfo<TAProductInfo> info)
        {
            bool soldOut = info.ProductInfo.SoldOut || info.SumShareCount == 0 || (info.PaidShareCount == info.SumShareCount && info.AvailableShareCount == 0 && info.PayingShareCount == 0);
            string soldOutTime = "";
            if (soldOut)
            {
                soldOutTime = (info.ProductInfo.SoldOutTime ?? DateTime.Now).ToMeowFormat();
            }

            TAProductInfoResponse product = new TAProductInfoResponse
            {
                AvailableShareCount = info.AvailableShareCount,
                BillNo = info.ProductInfo.BillNo,
                ConsignmentAgreementName = info.ProductInfo.ConsignmentAgreementName,
                CurrentValueDate = info.ProductInfo.CurrentValueDate.ToMeowFormat(),
                Drawee = info.ProductInfo.Drawee,
                DraweeInfo = info.ProductInfo.DraweeInfo,
                EndorseImageLink = info.ProductInfo.EndorseImageLink,
                EndorseImageThumbnailLink = info.ProductInfo.EndorseImageThumbnailLink,
                EndSellTime = info.ProductInfo.EndSellTime.ToMeowFormat(),
                EnterpriseInfo = info.ProductInfo.EnterpriseInfo,
                EnterpriseLicense = info.ProductInfo.EnterpriseLicense,
                EnterpriseName = info.ProductInfo.EnterpriseName,
                FinancingSum = info.ProductInfo.FinancingSum.ToIntFormat(),
                FinancingSumCount = info.ProductInfo.FinancingSumCount,
                GuaranteeMode = info.ProductInfo.GuaranteeMode,
                LaunchTime = info.ProductInfo.LaunchTime.ToMeowFormat(),
                MaxShareCount = info.ProductInfo.MaxShareCount,
                MinShareCount = info.ProductInfo.MinShareCount,
                OnPreSale = info.ProductInfo.OnPreSale,
                OnSale = info.ProductInfo.OnSale,
                PaidShareCount = info.PaidShareCount,
                PayingShareCount = info.PayingShareCount,
                Period = info.ProductInfo.Period,
                PledgeAgreementName = info.ProductInfo.PledgeAgreementName,
                PreEndSellTime = info.ProductInfo.PreEndSellTime.ToMeowFormat(),
                PreSale = info.ProductInfo.PreSale,
                PreStartSellTime = info.ProductInfo.PreStartSellTime.ToMeowFormat(),
                ProductIdentifier = info.ProductInfo.ProductIdentifier,
                ProductName = info.ProductInfo.ProductName,
                ProductNo = info.ProductInfo.ProductNo,
                ProductNumber = info.ProductInfo.ProductNumber,
                Repaid = info.ProductInfo.Repaid,
                RepaymentDeadline = info.ProductInfo.RepaymentDeadline.ToMeowFormat(),
                Securedparty = info.ProductInfo.Securedparty,
                SecuredpartyInfo = info.ProductInfo.SecuredpartyInfo,
                ServerTime = DateTime.Now.AddSeconds(2).ToMeowFormat(),
                SettleDate = info.ProductInfo.SettleDate.ToMeowFormat(),
                SoldOut = soldOut,
                SoldOutTime = soldOutTime,
                StartSellTime = info.ProductInfo.StartSellTime.ToMeowFormat(),
                SumShareCount = info.SumShareCount,
                UnitPrice = info.ProductInfo.UnitPrice.ToIntFormat(),
                Usage = info.ProductInfo.Usage,
                ValueDate = info.ProductInfo.ValueDate.ToMeowFormat(),
                ValueDateMode = info.ProductInfo.ValueDateMode,
                Yield = info.ProductInfo.Yield.RoundScale(1, 2),
                ProductCategory = info.ProductInfo.ProductCategory
            };

            product.ShowingStatus = ProductResponseHelper.GetProductShowingStatus(info.ProductInfo, soldOut);

            return product;
        }
    }
}