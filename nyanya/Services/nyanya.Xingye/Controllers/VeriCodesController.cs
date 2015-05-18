// FileInformation: nyanya/nyanya.Xingye/VeriCodesController.cs
// CreatedTime: 2014/09/01   10:16 AM
// LastUpdatedTime: 2014/09/02   3:28 PM

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Xingye.Models;
using Xingye.Domain.Auth.Services.DTO;
using Xingye.Domain.Auth.Services.Interfaces;

namespace nyanya.Xingye.Controllers
{
    /// <summary>
    ///     手机校验码
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
        ///     发送校验码 -- 一天(自然天)同种类型的校验码只能发送5次（包括验证成功和验证失败）
        /// </summary>
        /// <param name="request">
        ///     校验码发送请求：
        ///     Cellphone[必填]: 手机号码
        ///     Type[必填]: 校验码用途类型。10 => 注册，20 => 重置登录密码，30 => 重置支付密码, 40 => 未激活用户注册
        /// </param>
        /// <returns>
        ///     RemainCount: 今天剩余发送次数，若为-1或者0，则今天不能再次发送该类型校验码
        ///     Successful: 本次发送结果
        /// </returns>
        /// <remarks>
        ///     2014-07-20 13:05 测试 by SiqiLu
        /// </remarks>
        [Route("Send")]
        [EmptyParameterFilter("request")]
        [ValidateModelState(Order = 1)]
        [ResponseType(typeof(SendVeriCodeResponse))]
        public async Task<IHttpActionResult> Send(SendVeriCodeRequest request)
        {
            SendVeriCodeResult result;
            result = await this.veriCodeService.SendAsync(request.Cellphone, request.Type);
            return this.Ok(result.ToSendVeriCodeResponse());
        }

        /// <summary>
        ///     验证校验码是否正确 -- 一个校验码只能验证失败3次，并且只能使用一次，校验码有效期为15分钟
        /// </summary>
        /// <param name="request">
        ///     校验码验证请求：
        ///     Cellphone[必填]: 手机号码
        ///     Code[必填]: 校验码，主要用于验证
        ///     Type[必填]: 校验码用途类型。10 => 注册，20 => 重置登录密码，30 => 重置支付密码, 40 => 未激活用户注册
        /// </param>
        /// <returns>
        ///     RemainCount: 剩余验证次数，若为-1，则该校验码已失效。校验码过期等其他非异常情况也会返回-1。若为0,则该校验码失效，不能再进行验证。
        ///     Successful: 本次验证结果
        ///     Token: 校验码口令，如果校验码验证失败，则为空
        /// </returns>
        /// ///
        /// <remarks>
        ///     2014-07-20 13:05 测试 by SiqiLu
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
