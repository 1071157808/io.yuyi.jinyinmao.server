// FileInformation: nyanya/Services.WebAPI.Common/ApiControllerBase.cs
// CreatedTime: 2014/07/16   5:00 PM
// LastUpdatedTime: 2014/07/20   1:21 PM

using System;
using System.Web.Http;
using System.Web.Http.Tracing;
using Cqrs.Domain.Users.Services;
using Cqrs.Domain.Users.Services.Interfaces;
using Infrastructure.Lib.Extensions;

namespace Services.WebAPI.Common.Controller
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
        /// Oks this instance.
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