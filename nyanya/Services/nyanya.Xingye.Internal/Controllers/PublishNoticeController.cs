// FileInformation: nyanya/nyanya.Xingye.Internal/PublishNoticeController.cs
// CreatedTime: 2014/09/11   15:47 PM
// LastUpdatedTime: 2014/09/11   15:47 PM

using System.Threading.Tasks;
using System.Web.Http;
using Domian.Bus;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Xingye.Internal.Filters;
using nyanya.Xingye.Internal.Models;
using Xingye.Commands.Products;
using Xingye.Domain.Products.Services.DTO;
using Xingye.Domain.Products.Services.Interfaces;
using Infrastructure.Cache.Couchbase;

namespace nyanya.Xingye.Internal.Controllers
{
    /// <summary>
    ///     PublishNoticeController
    /// </summary>
    [RoutePrefix("PublishNotice")]
    public class PublishNoticeController : ApiControllerBase
    {
        /// <summary>
        ///     发布公告
        /// </summary>
        /// <param name="request">
        ///     Content[string]: 公告内容
        ///     ExpireTime[string:yyyy-MM-dd HH:mm:ss]: 过期时间
        ///     Flag[bool]: 是否发布
        /// </param>
        /// <returns>200 | 400</returns>
        [Route("Public")]
        [IpAuthorize]
        [EmptyParameterFilter("request", Order = 1)]
        [ValidateModelState(Order = 2)]
        public IHttpActionResult Publish(NoticeRequest request)
        {
            NoticeCacheModel data = new NoticeCacheModel() { Content = request.Content, ExpireTime = request.ExpireTime, Flag = request.Flag };
            bool result = NoticeCache.SetNoticeCache(data);
            if (!result)
            {
                return this.BadRequest("发布公告失败");
            }
            return this.OK();
        }
    }
}