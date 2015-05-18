// FileInformation: nyanya/Cat.Events.Users/RegisteredANewUser.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/12   10:09 AM

using System;
using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;

namespace Cat.Events.Users
{
    /// <summary>
    ///     注册新用户
    /// </summary>
    public class RegisteredANewUser : Event
    {
        /// <summary>
        ///     初始化<see cref="RegisteredANewUser" />类的新实例. 
        /// </summary>
        public RegisteredANewUser()
        {
        }
        /// <summary>
        ///     初始化<see cref="RegisteredANewUser" />类的新实例. 
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="sourceType">源类型</param>
        public RegisteredANewUser(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }
        /// <summary>
        ///     手机号
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        ///     用户标识
        /// </summary>
        public string UserIdentifier { get; set; }
    }
    /// <summary>
    ///     注册新用户（验证）
    /// </summary>
    public class RegisteredANewUserValidator : AbstractValidator<RegisteredANewUser>
    {
        /// <summary>
        ///     初始化<see cref="RegisteredANewUserValidator" />类的新实例. 
        /// </summary>
        public RegisteredANewUserValidator()
        {
            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();
            this.RuleFor(c => c.Cellphone).Matches(RegexUtils.CellphoneRegex.ToString());

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
            this.RuleFor(c => c.UserIdentifier).Length(10, 50);
        }
    }
}