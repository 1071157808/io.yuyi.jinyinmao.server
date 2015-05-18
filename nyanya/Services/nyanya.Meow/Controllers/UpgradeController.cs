// FileInformation: nyanya/nyanya.Meow/VeriCodesController.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:26 PM

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Cat.Domain.Meow.Services.DTO;
using Cat.Domain.Meow.Services.Interfaces;
using nyanya.AspDotNet.Common.Controller;
using nyanya.Meow.Models;

namespace nyanya.Meow.Controllers
{
    /// <summary>
    ///     UpgradeController
    /// </summary>
    [RoutePrefix("Upgrade")]
    public class UpgradeController : ApiControllerBase
    {
        private readonly IUpgradeService upgradeService;

        /// <summary>
        ///     初始化升级服务实例 
        /// </summary>
        /// <param name="upgradeService">升级服务实例</param>
        public UpgradeController(IUpgradeService upgradeService)
        {
            this.upgradeService = upgradeService;
        }

        /// <summary>
        ///     获取升级信息
        /// </summary>
        /// <param name="request">
        ///     channel: app市场
        ///     source: 平台
        ///     v: 当前版本号
        /// </param>
        /// <returns>
        ///     Status: 升级状态
        ///     Url: 安装包url
        ///     Version: 当前版本号
        ///     Message: 升级提示信息
        /// </returns>
        [HttpGet, Route("Obtain")]
        public async Task<IHttpActionResult> Obtain([FromUri]ObtainRequest request)
        {
            UpgradeResult result = await this.upgradeService.GetUpgradeAsync(request.Channel, request.Source, request.V);
            return this.Ok(result);
        }
    }
}