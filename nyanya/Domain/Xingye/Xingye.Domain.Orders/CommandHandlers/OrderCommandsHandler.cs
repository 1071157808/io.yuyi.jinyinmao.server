// FileInformation: nyanya/Xingye.Domain.Orders/OrderCommandsHandler.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:31 PM

using System.Threading.Tasks;
using Domian.Commands;
using Domian.Config;
using Xingye.Commands.Orders;
using Xingye.Domain.Orders.Models;

namespace Xingye.Domain.Orders.CommandHandlers
{
    public class OrderCommandsHandler : CommandHandlerBase, ICommandHandler<BuildInvestingOrder>
    {
        public OrderCommandsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        #region ICommandHandler<BuildInvestingOrder> Members

        public async Task Handler(BuildInvestingOrder command)
        {
            await this.DoAsync(async c => { await Order.BuildOrderAsync(c); }, command, "0008");
        }

        #endregion ICommandHandler<BuildInvestingOrder> Members
    }
}