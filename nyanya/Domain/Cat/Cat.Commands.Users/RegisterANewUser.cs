// FileInformation: nyanya/Cat.Commands.Users/RegisterANewUser.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:23 PM

using Domian.Commands;
using Infrastructure.Lib.Utility;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Cat.Commands.Users
{
    /// <summary>
    /// 注册新用户请求类
    /// </summary>
    [Route("/RegisterANewUser")]
    public class RegisterANewUser : Command
    {
        /// <summary>
        ///     初始化<see cref="RegisterANewUser" />类的新实例.
        /// </summary>
        public RegisterANewUser()
        {
        }

        /// <summary>
        ///     初始化<see cref="RegisterANewUser" />类的新实例.
        /// </summary>
        /// <param name="identifier">用户唯一标示符</param>
        public RegisterANewUser(string identifier)
            : base("USER_" + identifier)
        {
            this.UserIdentifier = identifier;
        }

        /// <summary>
        /// 用户手机号（唯一）
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        ///     客户端类型
        /// </summary>
        public long ClientType { get; set; }

        /// <summary>
        ///     活动编号
        /// </summary>
        public long ContractId { get; set; }

        /// <summary>
        ///     FlgMoreI1
        /// </summary>
        public long FlgMoreI1 { get; set; }

        /// <summary>
        ///     FlgMoreI2
        /// </summary>
        public long FlgMoreI2 { get; set; }

        /// <summary>
        ///     FlgMore1
        /// </summary>
        public string FlgMoreS1 { get; set; }

        /// <summary>
        ///     FlgMoreS2
        /// </summary>
        public string FlgMoreS2 { get; set; }

        /// <summary>
        /// 识别码
        /// </summary>
        public string IdentificionCode { get; set; }

        /// <summary>
        ///     邀请码
        /// </summary>
        public string InviteBy { get; set; }

        /// <summary>
        /// 注册的ip地址
        /// </summary>
        public string IpReg { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户唯一标示符
        /// </summary>
        public string UserIdentifier { get; set; }

        /// <summary>
        /// 用户类型 10金银猫 20兴业
        /// </summary>
        public int UserType { get; set; }
    }

    /// <summary>
    /// 注册新用户验证类
    /// </summary>
    public class RegisterANewUserValidator : AbstractValidator<RegisterANewUser>
    {
        /// <summary>
        ///     初始化<see cref="RegisterANewUserValidator" />类的新实例.
        /// </summary>
        public RegisterANewUserValidator()
        {
            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();
            this.RuleFor(c => c.Cellphone).Matches(RegexUtils.CellphoneRegex.ToString());

            this.RuleFor(c => c.Password).NotNull();
            this.RuleFor(c => c.Password).NotEmpty();
            this.RuleFor(c => c.Password).Matches(RegexUtils.PasswordRegex.ToString());

            this.RuleFor(c => c.IdentificionCode).NotNull();
            this.RuleFor(c => c.IdentificionCode).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
        }
    }
}
