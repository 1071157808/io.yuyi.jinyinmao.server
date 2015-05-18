// FileInformation: nyanya/Cat.Domain.Products/ProductCommandsHandler.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:26 PM

using System.Threading.Tasks;
using Cat.Commands.Products;
using Cat.Domain.Products.Models;
using Domian.Commands;
using Domian.Config;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;

namespace Cat.Domain.Products.CommandHandlers
{
    public class ProductCommandsHandler : CommandHandlerBase,
        ICommandHandler<BAProductUnShelves>,
        ICommandHandler<LaunchBAProduct>,
        ICommandHandler<LaunchTAProduct>,
        ICommandHandler<ProductRepay>,
        ICommandHandler<LaunchZCBProduct>,
        ICommandHandler<ZCBUpdateShareCount>
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

        #region ICommandHandler<LaunchZCBProduct> Members

        public async Task Handler(LaunchZCBProduct command)
        {
            await this.DoAsync(async c =>
            {
                ZCBProduct product = new ZCBProduct(GuidUtils.NewGuidString());
                product.BuildZCBProduct(c);
                await product.LaunchAsync();
            }, command, "0011", "ZCBProduct Launch Failed.{0}".FmtWith(command.ProductNo));
        }
        #endregion

        #region ICommandHandler<ZCBUpdateShareCount> Members

        public async Task Handler(ZCBUpdateShareCount command)
        {
            await this.DoAsync(async c =>
                {
                    ZCBProduct product = new ZCBProduct(c.ProductIdentifier);
                    await product.UpdateZCBProductAsync(c);
                }, command, "0012", "ZCBUpdateShareCount Failed.{0}".FmtWith(command.ProductIdentifier));
        }
        #endregion ICommandHandler<ZCBUpdateShareCount> Members
    }
}