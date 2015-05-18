// FileInformation: nyanya/Xingye.Domain.Products/ProductCommandsHandler.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using System.Threading.Tasks;
using Domian.Commands;
using Domian.Config;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using Xingye.Commands.Products;
using Xingye.Domain.Products.Models;

namespace Xingye.Domain.Products.CommandHandlers
{
    public class ProductCommandsHandler : CommandHandlerBase,
        ICommandHandler<BAProductUnShelves>,
        ICommandHandler<LaunchBAProduct>,
        ICommandHandler<LaunchTAProduct>,
        ICommandHandler<ProductRepay>
    {
        public ProductCommandsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        #region ICommandHandler<BAProductUnShelves> Members

        public async Task Handler(BAProductUnShelves command)
        {
            await this.DoAsync(async c =>
            {
                Product product = new Product(c.ProductIdentifier);
                await product.UnShelvesAsync();
            }, command, "0003", "BAProduct UnShelves Failed.{0}".FmtWith(command.ProductIdentifier));
        }

        #endregion ICommandHandler<BAProductUnShelves> Members

        #region ICommandHandler<LaunchBAProduct> Members

        public async Task Handler(LaunchBAProduct command)
        {
            await this.DoAsync(async c =>
            {
                BankAcceptanceProduct product = new BankAcceptanceProduct(GuidUtils.NewGuidString());
                product.BuildBankAcceptanceProduct(c);
                await product.LaunchAsync();
            }, command, "0001", "BAProduct Launch Failed.{0}".FmtWith(command.ProductNo));
        }

        #endregion ICommandHandler<LaunchBAProduct> Members

        #region ICommandHandler<LaunchTAProduct> Members

        public async Task Handler(LaunchTAProduct command)
        {
            await this.DoAsync(async c =>
            {
                TradeAcceptanceProduct product = new TradeAcceptanceProduct(GuidUtils.NewGuidString());
                product.BuildTradeAcceptanceProduct(c);
                await product.LaunchAsync();
            }, command, "0002", "TAProduct Launch Failed.{0}".FmtWith(command.ProductNo));
        }

        #endregion ICommandHandler<LaunchTAProduct> Members

        #region ICommandHandler<ProductRepay> Members

        public async Task Handler(ProductRepay command)
        {
            await this.DoAsync(async c =>
            {
                Product product = new Product(c.ProductIdentifier);
                await product.RepayAsync();
            }, command, "0010", "Product Repay Failed.{0}".FmtWith(command.ProductIdentifier));
        }

        #endregion ICommandHandler<ProductRepay> Members
    }
}