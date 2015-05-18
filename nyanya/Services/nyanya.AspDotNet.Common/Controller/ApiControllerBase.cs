// FileInformation: nyanya/nyanya.AspDotNet.Common/ApiControllerBase.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/17   10:50 AM

using System;
using System.Web.Http;
using System.Web.Http.Tracing;
using Cat.Domain.Users.Services;
using Cat.Domain.Users.Services.Interfaces;
using Infrastructure.Lib.Extensions;

namespace nyanya.AspDotNet.Common.Controller
{
    public abstract class ApiControllerBase : ApiController
    {
        private CurrentUser currentUser;

        public ITraceWriter Trace
        {
            get { return this.Configuration.Services.GetTraceWriter(); }
        }

        protected CurrentUser CurrentUser
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

        /// <summary>
        ///     404
        /// </summary>
        /// <returns>应付测试</returns>
        protected IHttpActionResult CanNotFound()
        {
            return this.BadRequest("找不到您需要的信息");
        }

        /// <summary>
        ///     Oks this instance.
        /// </summary>
        /// <returns>200 with empty json</returns>
        protected IHttpActionResult OK()
        {
            return this.Ok(new Object());
        }

        protected void Warning(string message)
        {
            this.Trace.Warn(this.Request, "Warn", message);
        }

        private CurrentUser GetCurrentUser()
        {
            IUserService service = this.Configuration.DependencyResolver.GetService(typeof(UserService)) as UserService;
            return service != null ? service.GetCurrentUser(this.User) : new CurrentUser();
        }
    }
}