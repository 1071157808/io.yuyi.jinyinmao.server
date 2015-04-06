// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  1:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  3:02 AM
// ***********************************************************************
// <copyright file="UserController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Web.Http;
using System.Web.Http.Description;
using Moe.AspNet.Filters;
using Yuyi.Jinyinmao.Api.Models.User;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     Class UserController.
    /// </summary>
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        /// <summary>
        ///     金银猫客户端注册接口
        /// </summary>
        /// <remarks>
        ///     在金银猫的客户端注册，包括PC网页、M版网页、iPhone、Android
        ///     &lt;br /&gt;
        ///     前置条件：已经通过验证码验证手机号码的真实性
        /// </remarks>
        /// <param name="request">
        ///     注册请求
        /// </param>
        /// <response code="200">注册成功</response>
        /// <response code="400"></response>
        [Route("SignUp"), ActionParameterRequired("request"), ActionParameterValidate(Order = 1), ResponseType(typeof(SignUpResponse))]
        public IHttpActionResult SignUp(SignUpRequest request)
        {
            return this.Ok(request);
        }
    }
}
