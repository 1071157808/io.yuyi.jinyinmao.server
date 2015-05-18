// FileInformation: nyanya/Domain.Users.Contract/AddTheUserValidator.cs
// CreatedTime: 2014/07/01   1:32 PM
// LastUpdatedTime: 2014/07/01   2:47 PM

using Domain.Users.Contract.Commands;
using ServiceStack.FluentValidation;

namespace Domain.Users.Contract.CommandsValidation
{
    public class AddTheUserValidator : AbstractValidator<AddTheUser>
    {
        public AddTheUserValidator()
        {
            this.RuleFor(addTheUser => addTheUser.Password).NotNull();
            this.RuleFor(addTheUser => addTheUser.Password).NotEmpty();
            this.RuleFor(addTheUser => addTheUser.Password.Length).GreaterThanOrEqualTo(6);
            this.RuleFor(addTheUser => addTheUser.Password.Length).LessThanOrEqualTo(18);

            //通配未验证
            //RuleFor(addTheUser => addTheUser.Password).Matches("^[a-zA-Z\\d~!@#$%^&*_]{6,18}$");
        }
    }
}