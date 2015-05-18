// FileInformation: nyanya/Xingye.Domain.Users/UserCommandsHandler.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using System.Threading.Tasks;
using Domian.Commands;
using Domian.Config;
using Xingye.Commands.Users;
using Xingye.Domain.Users.Models;

namespace Xingye.Domain.Users.CommandHandlers
{
    public class UserCommandsHandler : CommandHandlerBase, ICommandHandler<RegisterANewUser>,
        ICommandHandler<ChangeLoginPassword>,
        ICommandHandler<SetPaymentPassword>,
        ICommandHandler<SignUpPayment>,
        ICommandHandler<AddBankCard>,
        ICommandHandler<TempRegisterANewUser>
    {
        public UserCommandsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        #region ICommandHandler<AddBankCard> Members

        public async Task Handler(AddBankCard command)
        {
            await this.DoAsync(async c =>
            {
                User user = new User(c.UserIdentifier);
                await user.AddBankCardAsync(c);
            }, command, "0007");
        }

        #endregion ICommandHandler<AddBankCard> Members

        #region ICommandHandler<ChangeLoginPassword> Members

        public async Task Handler(ChangeLoginPassword command)
        {
            await this.DoAsync(async c =>
            {
                User user = new User(command.UserIdentifier);
                await user.ChangePasswordAsync(command.Password, command.Salt);
            }, command, "0009");
        }

        #endregion ICommandHandler<ChangeLoginPassword> Members

        #region ICommandHandler<RegisterANewUser> Members

        public async Task Handler(RegisterANewUser command)
        {
            await this.DoAsync(async c =>
            {
                User user = new User(c.UserIdentifier);
                await user.SignUpAsync(command);
            }, command, "0004");
        }

        #endregion ICommandHandler<RegisterANewUser> Members

        #region ICommandHandler<TempRegisterANewUser> Members

        public async Task Handler(TempRegisterANewUser command)
        {
            await this.DoAsync(async c =>
            {
                User user = new User(c.UserIdentifier);
                await user.TempSignUpAsync(command);
            }, command, "0007");
        }

        #endregion ICommandHandler<TempRegisterANewUser> Members

        #region ICommandHandler<SetPaymentPassword> Members

        public async Task Handler(SetPaymentPassword command)
        {
            await this.DoAsync(async c =>
            {
                User user = new User(c.UserIdentifier);
                await user.SetPaymentPasswordAsync(c.PaymentPassword);
            }, command, "0005");
        }

        #endregion ICommandHandler<SetPaymentPassword> Members

        #region ICommandHandler<SignUpPayment> Members

        public async Task Handler(SignUpPayment command)
        {
            await this.DoAsync(async c =>
            {
                User user = new User(c.UserIdentifier);
                await user.SignUpPaymentAsync(c);
            }, command, "0006");
        }

        #endregion ICommandHandler<SignUpPayment> Members
    }
}