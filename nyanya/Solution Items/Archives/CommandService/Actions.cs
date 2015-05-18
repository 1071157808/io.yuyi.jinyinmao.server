// FileInformation: nyanya/CommandService/Actions.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/27   12:42 PM

using System.Threading.Tasks;
using Cqrs.Commands.Order;
using Cqrs.Commands.Products;
using Cqrs.Commands.User;
using Cqrs.Domain.Commands;

namespace CommandService
{
    /// <summary>
    ///     CommandHandlerService
    /// </summary>
    public partial class CommandHandlerService
    {
        #region Public Methods

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

        #endregion Public Methods
    }
}