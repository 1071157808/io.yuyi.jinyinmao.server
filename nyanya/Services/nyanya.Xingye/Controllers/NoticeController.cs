// FileInformation: nyanya/nyanya.Xingye/NoticeController.cs
// CreatedTime: 2014/09/11   13:33 PM
// LastUpdatedTime: 2014/09/11   13:33 PM

using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using Domian.Bus;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Xingye.Filters;
using nyanya.Xingye.Models;
using Xingye.Commands.Users;
using Xingye.Domain.Auth.Models;
using Xingye.Domain.Auth.Services.DTO;
using Xingye.Domain.Auth.Services.Interfaces;
using Xingye.Domain.Users.Models;
using Xingye.Domain.Users.ReadModels;
using Xingye.Domain.Users.Services.DTO;
using Xingye.Domain.Users.Services.Interfaces;
using Xingye.Events.Users;
using Infrastructure.Cache.Couchbase;

namespace nyanya.Xingye.Controllers
{
    /// <summary>
    ///     NoticeController
    /// </summary>
    [RoutePrefix("Notice")]
    public class NoticeController : XingyeApiControllerBase
    {
        /// <summary>
        ///     获取公告内容信息
        /// </summary>
        /// <returns>
        ///     Content[string]: 公告内容
        /// </returns>
        [Route("Public")]
        [HttpGet]
        public IHttpActionResult GetPublicNotice()
        {
            NoticeCacheModel CacheResult = NoticeCache.GetNoticeCache();
            string result = "";
            if (CacheResult.Flag == true && DateTime.Parse(CacheResult.ExpireTime).CompareTo(DateTime.Now) > 0)
            {
                result = CacheResult.Content;
            }
            return this.Ok(new { Content = result });
        }
    }
}