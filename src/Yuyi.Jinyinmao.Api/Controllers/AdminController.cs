// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-10  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-10  9:21 AM
// ***********************************************************************
// <copyright file="AdminController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using System.Web.Http;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     AdminController.
    /// </summary>
    [Route("Admin"), HMACAuthentication, IpAuthorize]
    public class AdminController : ApiControllerBase
    {
        /// <summary>
        ///     The user service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AdminController" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public AdminController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        ///     重新刷新用户数据
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        /// <response code="200"></response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="401">未授权</response>
        /// <response code="500"></response>
        [Route("User/Reload/{userIdentifier:length(32)}")]
        public async Task<IHttpActionResult> ReloadUserData(string userIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            await this.userService.ReloadDataAsync(userId);

            return this.Ok();
        }
    }
}