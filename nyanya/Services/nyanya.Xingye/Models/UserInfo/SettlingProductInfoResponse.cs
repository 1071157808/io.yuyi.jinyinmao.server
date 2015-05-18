// FileInformation: nyanya/nyanya.Xingye/SettlingProductInfoResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   3:28 PM

using Infrastructure.Lib.Extensions;
using Xingye.Commands.Products;
using Xingye.Domain.Orders.Services.DTO;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     SettlingProductInfoResponse
    /// </summary>
    public class SettlingProductInfoResponse
    {
        #region Public Properties

        /// <summary>
        /// 额外收益
        /// </summary>
        public decimal ExtraInterest { get; set; }
        /// <summary>
        ///     Gets or sets the interest.
        /// </summary>
        /// <value>
        ///     The interest.
        /// </value>
        public decimal Interest { get; set; }

        /// <summary>
        ///     Gets or sets the principal.
        /// </summary>
        /// <value>
        ///     The principal.
        /// </value>
        public decimal Principal { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>
        ///     The product identifier.
        /// </value>
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets the name of the product.
        /// </summary>
        /// <value>
        ///     The name of the product.
        /// </value>
        public string ProductName { get; set; }

        /// <summary>
        ///     Gets or sets the product no.
        /// </summary>
        /// <value>
        ///     The product no.
        /// </value>
        public string ProductNo { get; set; }

        /// <summary>
        ///     Gets or sets the product number.
        /// </summary>
        /// <value>
        ///     The product number.
        /// </value>
        public int ProductNumber { get; set; }

        /// <summary>
        ///     Gets or sets the type of the product.
        /// </summary>
        /// <value>
        ///     The type of the product.
        /// </value>
        public ProductType ProductType { get; set; }

        /// <summary>
        ///     Gets or sets the settle date.
        /// </summary>
        /// <value>
        ///     The settle date.
        /// </value>
        public string SettleDate { get; set; }

        #endregion Public Properties
    }

    internal static class SettlingProductInfoExtensions
    {
        #region Internal Methods

        internal static SettlingProductInfoResponse ToSettlingProductInfoResponse(this SettlingProductInfo info)
        {
            return new SettlingProductInfoResponse
            {
                ExtraInterest = decimal.Round(info.ExtraInterest,2),
                Interest = decimal.Round(info.Interest, 2),
                Principal = info.Principal.ToIntFormat(),
                ProductIdentifier = info.ProductIdentifier,
                ProductName = info.ProductName,
                ProductNo = info.ProductNo,
                ProductNumber = info.ProductNumber,
                ProductType = info.ProductType,
                SettleDate = info.SettleDate.ToMeowFormat()
            };
        }

        #endregion Internal Methods
    }
}