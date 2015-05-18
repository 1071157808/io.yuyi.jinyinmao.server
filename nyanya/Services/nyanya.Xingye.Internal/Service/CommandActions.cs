// FileInformation: nyanya/nyanya.Xingye.Internal/CommandActions.cs
// CreatedTime: 2014/09/03   10:05 AM
// LastUpdatedTime: 2014/09/03   10:52 AM

using System.Threading.Tasks;
using Domian.Commands;
using Xingye.Commands.Orders;
using Xingye.Commands.Products;
using Xingye.Commands.Users;

namespace nyanya.Xingye.Internal.Service
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

        public async Task<CommandResult> Any(TempRegisterANewUser command)
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
    }
}