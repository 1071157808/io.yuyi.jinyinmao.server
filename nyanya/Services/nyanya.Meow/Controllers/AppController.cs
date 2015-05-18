// FileInformation: nyanya/nyanya.Meow/UserController.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:27 PM

using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using System.Web.Security;
using Cat.Commands.Users;
using Cat.Domain.Auth.Models;
using Cat.Domain.Auth.Services.DTO;
using Cat.Domain.Auth.Services.Interfaces;
using Cat.Domain.Meow.Services.DTO;
using Cat.Domain.Meow.Services.Interfaces;
using Cat.Domain.Users.Models;
using Cat.Domain.Users.ReadModels;
using Cat.Domain.Users.Services.DTO;
using Cat.Domain.Users.Services.Interfaces;
using Cat.Events.Users;
using Domian.Bus;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Meow.Filters;
using nyanya.Meow.Models;

namespace nyanya.Meow.Controllers
{
    /// <summary>
    ///     AppController
    /// </summary>
    [RoutePrefix("App")]
    public class AppController : ApiControllerBase
    {
        private readonly IUpgradeService upgradeService;

        /// <summary>
        ///     初始化升级服务实例 
        /// </summary>
        /// <param name="upgradeService">升级服务实例</param>
        public AppController(IUpgradeService upgradeService)
        {
            this.upgradeService = upgradeService;
        }

        /// <summary>
        ///     旧版获取升级信息
        /// </summary>
        /// <param name="request">
        ///     channel: app市场
        ///     source: 平台
        ///     v: 当前版本号
        /// </param>
        /// <returns>
        ///     data
        ///         Status: 升级状态 true|false
        ///         Url: 安装包url
        ///         Version: 当前版本号
        ///         Message: 升级提示信息
        /// </returns>
        [HttpGet, Route("Upgrade")]
        public async Task<IHttpActionResult> Upgrade([FromUri]ObtainRequest request)
        {
            UpgradeResult result = await this.upgradeService.GetUpgradeAsync(request.Channel, request.Source, request.V);
            return this.Ok(new {data = result});
        }

        /// <summary>
        ///     新版获取升级信息
        /// </summary>
        /// <param name="request">
        ///     channel: app市场
        ///     source: 平台
        ///     v: 当前版本号
        /// </param>
        /// <returns>
        ///     Status: 升级状态 0|1|2
        ///     Url: 安装包url
        ///     Version: 当前版本号
        ///     Message: 升级提示信息
        /// </returns>
        [HttpGet, Route("UpgradeEx")]
        public async Task<IHttpActionResult> UpgradeEx([FromUri]ObtainRequest request)
        {
            UpgradeExResult result = await this.upgradeService.GetUpgradeExAsync(request.Channel, request.Source, request.V);
            return this.Ok(result);
        }

        /// <summary>
        ///     用户扫描二维码
        /// </summary>
        /// <returns>
        ///     iphone跳转到市场
        ///     其他跳转到m版
        /// </returns>
        [HttpGet, Route("Scan")]
        public RedirectResult Scan()
        {
            if (HttpUtils.IsIphone(this.Request))
            {
                return this.Redirect(ConfigurationManager.AppSettings.Get("AppUrl"));
            }
            else
            {
                return this.Redirect(ConfigurationManager.AppSettings.Get("RootUrl"));
            }
        }
    }
}