// FileInformation: nyanya/Cat.Domain.Users/UserCommandsHandler.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:25 PM

using Cat.Commands.Users;
using Cat.Domain.Users.Models;
using Domian.Commands;
using Domian.Config;
using System;
using System.Threading.Tasks;

namespace Cat.Domain.Users.CommandHandlers
{
    public class UserCommandsHandler : CommandHandlerBase, ICommandHandler<RegisterANewUser>,
        ICommandHandler<ChangeLoginPassword>,
        ICommandHandler<SetPaymentPassword>,
        ICommandHandler<SignUpPayment>,
        ICommandHandler<AddBankCard>
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