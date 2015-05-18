// FileInformation: nyanya/Cat.Events.Users/UserSignInSucceeded.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Users
{
    /// <summary>
    ///     用户注册成功
    /// </summary>
    public class UserSignInSucceeded : Event
    {
        /// <summary>
        ///     初始化<see cref="UserSignInSucceeded" />类的新实例. 
        /// </summary>
        public UserSignInSucceeded()
        {
        }

        /// <summary>
        ///     初始化<see cref="UserSignInSucceeded" />类的新实例. 
        /// </summary>
        /// <param name="userIdentifier">用户标识.</param>
        /// <param name="sourceType">源类型.</param>
        public UserSignInSucceeded(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }
        /// <summary>
        ///     Token
        /// </summary>
        public string AmpAuthToken { get; set; }
        /// <summary>
        ///     CatToken
        /// </summary>
        public string GoldCatAuthToken { get; set; }
        /// <summary>
        ///     用户标识
        /// </summary>
        public string UserIdentifier { get; set; }
    }
    /// <summary>
    ///     用户注册成功（验证）
    /// </summary>
    public class UserSignInSucceededValidator : AbstractValidator<UserSignInSucceeded>
    {
        /// <summary>
        ///     初始化<see cref="UserSignInSucceededValidator" />类的新实例. 
        /// </summary>
        public UserSignInSucceededValidator()
        {
            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
            this.RuleFor(c => c.AmpAuthToken).NotNull();
            this.RuleFor(c => c.AmpAuthToken).NotEmpty();
            this.RuleFor(c => c.GoldCatAuthToken).NotNull();
            this.RuleFor(c => c.GoldCatAuthToken).NotEmpty();
        }
    }
}