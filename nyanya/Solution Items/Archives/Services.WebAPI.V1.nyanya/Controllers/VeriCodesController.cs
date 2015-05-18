// FileInformation: nyanya/Services.WebAPI.V1.nyanya/VeriCodesController.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/07/22   6:33 PM

using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Cqrs.Domain.Auth.Services.DTO;
using Cqrs.Domain.Auth.Services.Interfaces;
using Services.WebAPI.Common.Filters;
using Services.WebAPI.V1.nyanya.Models.VeriCodes;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     手机验证码
    /// </summary>
    [RoutePrefix("VeriCodes")]
    public class VeriCodesController : ApiController
    {
        private readonly IVeriCodeService veriCodeService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="VeriCodesController" /> class.
        /// </summary>
        /// <param name="veriCodeService">The veri code service.</param>
        public VeriCodesController(IVeriCodeService veriCodeService)
        {
            this.veriCodeService = veriCodeService;
        }

        // GET: api/VeriCodes/5
        /// <summary>
        ///     发送验证码 -- 一天(自然天)同种类型的验证码只能发送5次（包括验证成功和验证失败）
        /// </summary>
        /// <param name="request">
        ///     验证码发送请求：
        ///     Cellphone[必填]: 手机号码
        ///     Type[必填]: 验证码用途类型。10 => 注册，20 => 重置登录密码，30 => 重置支付密码
        /// </param>
        /// <returns>
        ///     RemainCount: 今天剩余发送次数，若为-1或者0，则今天不能再次发送该类型验证码
        ///     Successful: 本次发送结果
        /// </returns>
        /// <remarks>
        /// 2014-07-20 13:05 测试 by SiqiLu
        /// </remarks>
        [Route("Send")]
        [EmptyParameterFilter("request")]
        [ValidateModelState(Order = 1)]
        [ResponseType(typeof(SendVeriCodeResponse))]
        public async Task<IHttpActionResult> Send(SendVeriCodeRequest request)
        {
            SendVeriCodeResult result;
            try
            {
                result = await this.veriCodeService.SendAsync(request.Cellphone, request.Type);

            }
            catch (Exception e)
            {
                
                throw e;
            }

            return this.Ok(result.ToSendVeriCodeResponse());
        }

        /// <summary>
        ///     验证验证码是否正确 -- 一个验证码只能验证失败3次，并且只能使用一次，验证码有效期为15分钟
        /// </summary>
        /// <param name="request">
        ///     验证码验证请求：
        ///     Cellphone[必填]: 手机号码
        ///     Code[必填]: 验证码，主要用于验证
        ///     Type[必填]: 验证码用途类型。10 => 注册，20 => 重置登录密码，30 => 重置支付密码
        /// </param>
        /// <returns>
        ///     RemainCount: 剩余验证次数，若为-1，则该验证码已失效。验证码过期等其他非异常情况也会返回-1。若为0,则该验证码失效，不能再进行验证。
        ///     Successful: 本次验证结果
        ///     Token: 验证码口令，如果验证码验证失败，则为空
        /// </returns>
        /// /// <remarks>
        /// 2014-07-20 13:05 测试 by SiqiLu
        /// </remarks>
        [Route("Verify")]
        [EmptyParameterFilter("request")]
        [ValidateModelState(Order = 1)]
        [ResponseType(typeof(VerifyVeriCodeResult))]
        public async Task<IHttpActionResult> VerifyCode(VerifyVeriCodeRequest request)
        {
            VerifyVeriCodeResult result = await this.veriCodeService.VerifyAsync(request.Cellphone, request.Code, request.Type);

            return this.Ok(result.ToVerifyVeriCodeResponse());
        }
    }
}