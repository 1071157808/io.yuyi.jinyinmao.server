// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  1:00 AM
// ***********************************************************************
// <copyright file="MiscController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Moe.AspNet.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     Class MiscController.
    /// </summary>
    public class MiscController : ApiControllerBase
    {
        private readonly IVeriCodeService veriCodeService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MiscController" /> class.
        /// </summary>
        /// <param name="veriCodeService">The veri code service.</param>
        public MiscController(IVeriCodeService veriCodeService)
        {
            this.veriCodeService = veriCodeService;
        }

        /// <summary>
        ///     发送验证码
        /// </summary>
        /// <remarks>
        ///     一天(自然天)同种类型的验证码只能发送10次，防止恶意使用验证码
        /// </remarks>
        /// <param name="request">
        ///     验证码发送请求
        /// </param>
        /// <response code="200"></response>
        /// <response code="400"></response>
        /// <response code="401"></response>
        [HttpGet, Route("SendVeriCode"), ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(SendVeriCodeResponse))]
        public async Task<IHttpActionResult> SendAsync([FromUri] SendVeriCodeRequest request)
        {
            string cellphone = request.Cellphone;

            if (request.Type == VeriCodeType.ResetPaymentPassword)
            {
                if (this.CurrentUser == null)
                {
                    return this.Unauthorized();
                }

                cellphone = this.CurrentUser.Cellphone;
            }

            SendVeriCodeResult result = await this.veriCodeService.SendAsync(cellphone, request.Type, this.BuildArgs());

            return this.Ok(result.ToResponse());
        }

        /// <summary>
        ///     验证验证码是否正确
        /// </summary>
        /// <remarks>
        ///     验证码只能验证失败3次，并且只能使用一次，验证码有效期为30分钟
        /// </remarks>
        /// <param name="request">
        ///     验证码验证请求
        /// </param>
        /// <response code="200"></response>
        /// <response code="400"></response>
        [HttpGet, Route("VerifyVeriCode"), ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(VerifyVeriCodeResponse))]
        public async Task<IHttpActionResult> VerifyCodeAsync([FromUri] VerifyVeriCodeRequest request)
        {
            VerifyVeriCodeResult result = await this.veriCodeService.VerifyAsync(request.Cellphone, request.Code, request.Type);

            return this.Ok(result.ToResponse());
        }
    }
}