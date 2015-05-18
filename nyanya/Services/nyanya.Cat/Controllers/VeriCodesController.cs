// FileInformation: nyanya/nyanya.Cat/VeriCodesController.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   11:25 AM

using Cat.Domain.Auth.Models;
using Cat.Domain.Auth.Services.DTO;
using Cat.Domain.Auth.Services.Interfaces;
using Infrastructure.Lib.Extensions;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Cat.Models;
using nyanya.Cat.Models.VeriCodes;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VerifyTpye = global::Cat.Domain.Auth.Models.VeriCode.VeriCodeType;

namespace nyanya.Cat.Controllers
{
    /// <summary>
    ///     手机验证码
    /// </summary>
    [RoutePrefix("VeriCodes")]
    public class VeriCodesController : ApiControllerBase
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
        ///     发送验证码 -- 一天(自然天)同种类型的验证码发送次数由数据库控制（包括验证成功和验证失败）
        /// </summary>
        /// <param name="request">
        ///     验证码发送请求：
        ///     Cellphone[必填]: 手机号码
        ///     Token[]:  验证用令牌
        ///     Type[必填]: 验证码用途类型。10 => 注册，20 => 重置登录密码，30 => 重置支付密码
        /// </param>
        /// <returns>
        ///     RemainCount: 今天剩余发送次数，若为-1或者0，则今天不能再次发送该类型验证码
        ///     Successful: 本次发送结果
        /// </returns>
        /// <remarks>
        ///     2014-07-20 13:05 测试 by SiqiLu
        /// </remarks>
        [Route("Send")]
        [EmptyParameterFilter("request")]
        [ValidateModelState(Order = 1)]
        [ResponseType(typeof(SendVeriCodeResponse))]
        public async Task<IHttpActionResult> Send(SendVeriCodeTokenRequest request)
        {
            SendVeriCodeResult result;

            if (this.CurrentUser.Identifier.IsNullOrEmpty() || request.Cellphone != this.CurrentUser.Cellphone)
            {
                if (request.Token.IsNullOrEmpty())
                {
                    return this.BadRequest("缺少必填项");
                }
                UseVeriCodeResult usedResult = await this.veriCodeService.UseAsync(request.Token, VeriCode.VeriCodeType.VeriImage);
                if (!usedResult.Result)
                {
                    return this.BadRequest("操作超时，请重新获取图形验证码");
                }

                result = await this.veriCodeService.SendWithTokenAsync(request.Cellphone, request.Token, request.Type);
            }
            else
            {
                result = await this.veriCodeService.SendAsync(request.Cellphone, request.Type);
            }
            return this.Ok(result.ToSendVeriCodeResponse());
        }

        /// <summary>
        ///     发送图形验证码
        /// </summary>
        /// <param name="width">width[必填]:  客户端图片宽度</param>
        /// <returns>
        ///     responseMsg: 验证码图片请求
        /// </returns>
        [HttpGet, Route("SendGraphic")]
        [RangeFilter("width", 50, 150)]
        [ResponseType(typeof(HttpResponseMessage))]
        public async Task<HttpResponseMessage> SendGraphic(int width = 100)
        {
            string identifier = HttpContext.Current.Session["IdentifierVeriImage"] == null ? string.Empty : HttpContext.Current.Session["IdentifierVeriImage"].ToString();

            SendVeriCodePhoneResult result = await this.veriCodeService.SendGraphicAsync(identifier, width);

            if (HttpContext.Current.Session["IdentifierVeriImage"] == null)
            {
                HttpContext.Current.Session["IdentifierVeriImage"] = result.Identifier;
            }

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            result.VerifyImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

            var responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
            responseMsg.Content = new ByteArrayContent(stream.ToArray());
            responseMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("image/Jpeg");

            return responseMsg;
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

        /// <summary>
        ///     验证图片验证码是否正确
        /// </summary>
        /// <param name="request">
        ///     验证码验证请求：
        ///     Cellphone[必填]: 手机号码
        ///     Result[必填]: 验证码，主要用于验证
        /// </param>
        /// <returns>
        ///     RemainCount: 剩余验证次数，若为-1，则该验证码已失效。验证码过期等其他非异常情况也会返回-1。若为0,则该验证码失效，不能再进行验证。
        ///     Successful: 本次验证结果
        ///     Token: 验证码口令，如果验证码验证失败，则为空
        /// </returns>
        /// ///
        /// <remarks>
        ///     2014-07-20 13:05 测试 by SiqiLu
        /// </remarks>
        [Route("VerifyGraphic")]
        [EmptyParameterFilter("request")]
        [ValidateModelState(Order = 1)]
        [ResponseType(typeof(VerifyVeriCodeResponse))]
        public async Task<IHttpActionResult> VerifyGraphicCode(VerifyVeriCodePhoneRequest request)
        {
            string identifier = HttpContext.Current.Session["IdentifierVeriImage"] == null ? string.Empty : HttpContext.Current.Session["IdentifierVeriImage"].ToString();

            VerifyVeriCodeResult result = await this.veriCodeService.VerifyGraphicAsync(request.Cellphone, identifier, request.Result);

            return this.Ok(result.ToVerifyVeriCodeResponse());
        }
    }
}
