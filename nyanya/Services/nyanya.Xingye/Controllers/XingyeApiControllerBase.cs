// FileInformation: nyanya/nyanya.Xingye/XingyeApiControllerBase.cs
// CreatedTime: 2014/09/02   2:35 PM
// LastUpdatedTime: 2014/09/03   10:52 AM

using Infrastructure.Lib.Extensions;
using Xingye.Domain.Users.Services;
using Xingye.Domain.Users.Services.Interfaces;

namespace nyanya.AspDotNet.Common.Controller
{
    /// <summary>
    ///     XingyeApiControllerBase
    /// </summary>
    public abstract class XingyeApiControllerBase : ApiControllerBase
    {
        private CurrentUser currentUser;

        /// <summary>
        ///     Gets the current user.
        /// </summary>
        /// <value>
        ///     The current user.
        /// </value>
        protected new CurrentUser CurrentUser
        {
            get
            {
                if (this.currentUser.IsNull())
                {
                    this.currentUser = this.GetCurrentUser();
                }
                return this.currentUser;
            }
        }

        private CurrentUser GetCurrentUser()
        {
            IUserService service = this.Configuration.DependencyResolver.GetService(typeof(UserService)) as UserService;
            return service != null ? service.GetCurrentUser(this.User) : new CurrentUser();
        }
    }
}