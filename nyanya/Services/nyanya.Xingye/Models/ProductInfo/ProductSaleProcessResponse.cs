// FileInformation: nyanya/nyanya.Xingye/ProductSaleProcessResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/01   10:48 AM

using Infrastructure.Cache.Couchbase;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     ProductShareCacheModelExtensions
    /// </summary>
    public static class ProductShareCacheModelExtensions
    {
        /// <summary>
        ///     To the product sale process response.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ProductSaleProcessResponse ToProductSaleProcessResponse(this ProductShareCacheModel model)
        {
            return new ProductSaleProcessResponse
            {
                Available = model.Available,
                Paid = model.Paid,
                Paying = model.Paying,
                Sum = model.Sum
            };
        }
    }

    /// <summary>
    ///     ProductSaleProcessResponse
    /// </summary>
    public class ProductSaleProcessResponse
    {
        /// <summary>
        ///     Gets or sets the available.
        /// </summary>
        /// <value>
        ///     The available.
        /// </value>
        public int Available { get; set; }

        /// <summary>
        ///     Gets or sets the paid.
        /// </summary>
        /// <value>
        ///     The paid.
        /// </value>
        public int Paid { get; set; }

        /// <summary>
        ///     Gets or sets the paying.
        /// </summary>
        /// <value>
        ///     The paying.
        /// </value>
        public int Paying { get; set; }

        /// <summary>
        ///     Gets or sets the sum.
        /// </summary>
        /// <value>
        ///     The sum.
        /// </value>
        public int Sum { get; set; }
    }
}