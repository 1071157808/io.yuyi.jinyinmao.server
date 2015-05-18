// FileInformation: nyanya/Cat.Commands.Users/ChangeLoginPassword.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:23 PM

using Domian.Commands;
using Infrastructure.Lib.Utility;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Cat.Commands.Users
{
    /// <summary>
    /// 修改登录密码请求类
    /// </summary>
    [Route("/ChangeLoginPassword")]
    public class ChangeLoginPassword : Command
    {
        /// <summary>
        ///     初始化<see cref="ChangeLoginPassword" />类的新实例.
        /// </summary>
        public ChangeLoginPassword()
        {
        }

        /// <summary>
        ///     初始化<see cref="ChangeLoginPassword" />类的新实例.
        /// </summary>
        /// <param name="identifier">用户唯一标示符</param>
        public ChangeLoginPassword(string identifier)
            : base("USER_" + identifier)
        {
            this.UserIdentifier = identifier;
            this.Salt = identifier;
        }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 加密码
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// 用户唯一标示符
        /// </summary>
        public string UserIdentifier { get; set; }
    }

    /// <summary>
    /// 修改登录密码（验证）
    /// </summary>
    public class ChangeLoginPasswordValidator : AbstractValidator<ChangeLoginPassword>
    {
        /// <summary>
        ///     初始化<see cref="ChangeLoginPasswordValidator" />类的新实例.
        /// </summary>
        public ChangeLoginPasswordValidator()
        {
            this.RuleFor(c => c.Password).NotNull();
            this.RuleFor(c => c.Password).NotEmpty();
            this.RuleFor(c => c.Password).Matches(RegexUtils.PasswordRegex.ToString());

            this.RuleFor(c => c.Salt).NotNull();
            this.RuleFor(c => c.Salt).NotEmpty();
            this.RuleFor(c => c.Salt).Length(10, 80);

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
            this.RuleFor(c => c.UserIdentifier).Length(10, 50);
        }
    }
}