// FileInformation: nyanya/Cat.Events.Users/PasswordChanged.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/12   10:08 AM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Users
{
    /// <summary>
    ///     修改密码
    /// </summary>
    public class PasswordChanged : Event
    {
        /// <summary>
        ///     初始化<see cref="PasswordChanged" />类的新实例. 
        /// </summary>
        public PasswordChanged()
        {
        }
        /// <summary>
        ///     初始化<see cref="PasswordChanged" />类的新实例. 
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="sourceType">源类型</param>
        public PasswordChanged(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }
        /// <summary>
        ///     用户标识
        /// </summary>
        public string UserIdentifier { get; set; }
    }
    /// <summary>
    ///     修改密码（验证）
    /// </summary>
    public class PasswordChangedValidator : AbstractValidator<PasswordChanged>
    {
        /// <summary>
        ///     初始化<see cref="PasswordChangedValidator" />类的新实例. 
        /// </summary>
        public PasswordChangedValidator()
        {
            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
            this.RuleFor(c => c.UserIdentifier).Length(10, 50);
        }
    }
}