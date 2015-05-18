// FileInformation: nyanya/Cqrs.Domain.Order/OrderCommandsHandler.cs
// CreatedTime: 2014/08/04   12:35 AM
// LastUpdatedTime: 2014/08/06   10:22 AM

using Cat.Commands.Orders;
using Cat.Domain.Orders.Models;
using Domian.Commands;
using Domian.Config;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using System;
using System.Threading.Tasks;

namespace Cat.Domain.Orders.CommandHandlers
{
    public class OrderCommandsHandler : CommandHandlerBase, ICommandHandler<BuildInvestingOrder>,
        IResultCommandHandler<BuildRedeemPrincipal>, ICommandHandler<SetZCBRedeemBillsResult>
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

        public async Task Handler(SetZCBRedeemBillsResult command)
        {
            await this.DoAsync(async c =>
             {
                 foreach (var sn in c.SNList)
                 {
                     ZCBBill bill = new ZCBBill(sn);
                     try
                     {
                         await bill.SetZCBRedeemBillResult();
                     }
                     catch (Exception e)
                     {
                         Logger.Error("后台打款成功后前台设置取回状态失败，批次号（SN）：" + sn + ". 异常信息:" + e);
                         throw;
                     }
                 }
             }, command, "0014", "SetZCBRedeemBillsResult Failed."); ;
        }

        public async Task<ObjectCommandResult> ResultHandler(BuildRedeemPrincipal command)
        {
            return await this.DoAsyncWithResult(async c =>
           {
               var result = new ObjectCommandResult(command.CommandId);
               ZCBBill bill = new ZCBBill(c.UserIdentifier, c.ProductIdentifier);
               result.Data = await bill.BuildRedeemPrincipalAsync(c);
               return result;
           }, command, "0013", "RedeemPrincipal Failed.{0}".FmtWith(command.UserIdentifier));
        }

        public Task Handler(BuildRedeemPrincipal command)
        {
            throw new NotImplementedException();
        }
    }
}