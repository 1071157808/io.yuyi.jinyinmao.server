// FileInformation: nyanya/Services.WebAPI.V1.nyanya/TokenAuthorize.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/27   5:21 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Security;
using Infrastructure.Lib.Utility;
using Services.WebAPI.Common.Filters;

namespace Services.WebAPI.V1.nyanya.Filters
{
    /// <summary>
    ///     TokenAuthorizeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TokenAuthorizeAttribute : OrderedActionFilterAttribute
    {
        /// <summary>
        ///     The check permission
        /// </summary>
        private bool checkPermission;

        /// <summary>
        ///     The roles
        /// </summary>
        private string roles;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TokenAuthorizeAttribute" /> class.
        /// </summary>
        /// <param name="refreshToken">if set to <c>true</c> [refresh token].</param>
        public TokenAuthorizeAttribute(bool refreshToken = true)
        {
            this.RefreshToken = refreshToken;
            this.checkPermission = false;
            this.roles = "";
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [allow local].
        /// </summary>
        /// <value><c>true</c> if [allow local]; otherwise, <c>false</c>.</value>
        public bool AllowLocal { get; set; }

        /// <summary>
        ///     Gets or sets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public string Roles
        {
            get
            {
                return this.roles;
            }
            set
            {
                this.roles = value;
                this.checkPermission = true;
            }
        }

        /// <summary>
        ///     Gets or sets the permissible users.
        /// </summary>
        /// <value>The permissible users.</value>
        private List<string> PermissibleUsers { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [refresh token].
        /// </summary>
        /// <value><c>true</c> if [refresh token]; otherwise, <c>false</c>.</value>
        private bool RefreshToken { get; set; }

        /// <summary>
        ///     Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        private string UserName { get; set; }

        /// <summary>
        ///     Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string token;
            if (this.RefreshToken)
            {
                this.RefreshToken = actionContext.Request.RequestUri.Scheme.ToUpper() == "HTTPS";
            }

            if (this.AllowLocal && (actionContext.RequestContext.IsLocal || this.IpIsAuthorized(actionContext.Request)))
            {
                return;
            }
            if (!this.IsValid(actionContext, out token) || !this.IsAuthorized())
            {
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }
            if (this.RefreshToken && !String.IsNullOrWhiteSpace(token))
            {
                FormsAuthentication.SetAuthCookie(token, true);
            }
        }

        /// <summary>
        ///     Configrates the authorization.
        /// </summary>
        private void ConfigrateAuthorization()
        {
            this.PermissibleUsers = new List<string>();

            string[] authorizedRoles = this.Roles.Split(new[] { ',' });

            //if (authorizedRoles.Any(r => r.ToUpper() == "DEVELOPER"))
            //{
            //    this.PermissibleUsers.AddRange(ApplicationConfig.DevAccounts.Administrators);
            //    this.PermissibleUsers.AddRange(ApplicationConfig.DevAccounts.Developers);
            //    return;
            //}

            //if (authorizedRoles.Any(r => r.ToUpper() == "ADMINISTRATOR"))
            //{
            //    this.PermissibleUsers.AddRange(ApplicationConfig.DevAccounts.Administrators);
            //}
            this.PermissibleUsers.Add("15800780728");
        }

        /// <summary>
        ///     Formats the error message.
        /// </summary>
        /// <returns>string</returns>
        private string FormatErrorMessage()
        {
            return "请先登录。";
        }

        /// <summary>
        ///     Processes requests that fail authorization. This default implementation creates a new
        ///     response with the Unauthorized status code. Override this method to provide your own
        ///     handling for unauthorized requests.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        private void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext", "actionContext can not be null");
            }

            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, this.FormatErrorMessage());
        }

        /// <summary>
        ///     Determines whether the client ip is authorized.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///     bool
        /// </returns>
        private bool IpIsAuthorized(HttpRequestMessage request)
        {
            string ip = HttpUtils.GetUserHostAddress(request);
            return !String.IsNullOrEmpty(ip) && (ip == "::1" || ip.StartsWith("172.30") || ip.StartsWith("10.1"));
        }

        /// <summary>
        ///     Determines whether this instance is authorized.
        /// </summary>
        /// <returns>bool</returns>
        private bool IsAuthorized()
        {
            if (!this.checkPermission) return true;

            this.ConfigrateAuthorization();
            return this.PermissibleUsers.Contains(this.UserName);
        }

        /// <summary>
        ///     Determines whether the specified action context is valid.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <param name="newToken">The new token.</param>
        /// <returns>bool</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     actionContext;actionContext can not be null
        /// </exception>
        private bool IsValid(HttpActionContext actionContext, out string newToken)
        {
            newToken = "";

            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext", "actionContext can not be null");
            }

            IPrincipal user = actionContext.ControllerContext.RequestContext.Principal;
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            // Token 格式检验，必须由3部分组成
            if (String.IsNullOrWhiteSpace(user.Identity.Name) || user.Identity.Name.Split(new[] { ',' }).Count() != 3)
            {
                return false;
            }

            string[] tokenContents = user.Identity.Name.Split(new[] { ',' });

            // Identifier
            string guid = tokenContents[0];
            if (String.IsNullOrWhiteSpace(guid) || guid.Length != 32)
            {
                return false;
            }

            // 用户名检验，必须是手机号格式
            string cellphone = tokenContents[1];
            if (String.IsNullOrWhiteSpace(cellphone))
            {
                return false;
            }

            //Match m = this.regex.Match(cellphone);
            //// We are looking for an exact match, not just a search hit. This matches what the
            //// RegularExpressionValidator control does
            //if (!m.Success || m.Index != 0 || m.Length != cellphone.Length)
            //{
            //    return false;
            //}

            // Token有效期时间
            long expireDateTime;
            if (!long.TryParse(tokenContents[2], out expireDateTime))
            {
                return false;
            }

            // Token 有效期已过
            if (DateTime.FromBinary(expireDateTime) < DateTime.Now)
            {
                return false;
            }

            if (this.RefreshToken)
            {
                DateTime newExpiryTime = DateTime.Now.AddMinutes(30);
                newToken = string.Format("{0},{1},{2}", guid, cellphone, newExpiryTime.ToBinary());
            }

            this.UserName = cellphone;
            return true;
        }
    }
}