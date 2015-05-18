// FileInformation: nyanya/nyanya.Internal/CommandActions.cs
// CreatedTime: 2014/08/27   5:45 PM
// LastUpdatedTime: 2014/09/01   4:32 PM

using Cat.Commands.Orders;
using Cat.Commands.Products;
using Cat.Commands.Users;
using Domian.Commands;
using System.Threading.Tasks;

namespace nyanya.Internal.Service
{
    /// <summary>
    ///     CommandHandlerService
    /// </summary>
    public partial class CommandHandlerService
    {
        /// <summary>
        ///     Anies the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<CommandResult> Any(LaunchBAProduct command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     Anies the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<CommandResult> Any(LaunchTAProduct command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<CommandResult> Any(SetZCBRedeemBillsResult command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     Anies the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<CommandResult> Any(BAProductUnShelves command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     Anies the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<CommandResult> Any(RegisterANewUser command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     Anies the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<CommandResult> Any(SetPaymentPassword command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     Anies the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<CommandResult> Any(SignUpPayment command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     Anies the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<CommandResult> Any(AddBankCard command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     Anies the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<CommandResult> Any(BuildInvestingOrder command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     Anies the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<CommandResult> Any(ChangeLoginPassword command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     Anies the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<CommandResult> Any(ProductRepay command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     资产包产品上架
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<CommandResult> Any(LaunchZCBProduct command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     资产包产品可售份额更新
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<CommandResult> Any(ZCBUpdateShareCount command)
        {
            return await this.Handler(command);
        }

        /// <summary>
        ///     取回金额
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<ObjectCommandResult> Any(BuildRedeemPrincipal command)
        {
            return await this.ResultHandler(command);
        }
    }
}