// FileInformation: nyanya/Services.WebAPI.V1.nyanya/AppController.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/07/26   6:43 PM

using System.Configuration;
using System.Web.Mvc;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     AppController
    /// </summary>
    public class AppController : Controller
    {
        /// <summary>
        ///     Gets the url to download app.
        /// </summary>
        /// <returns>url or web view</returns>
        public ActionResult Index()
        {
            string userAgent = this.Request.UserAgent ?? "";

            if (this.Request.Browser.IsMobileDevice || userAgent.ToLower().Contains("xiaomi"))
            {
                // ios 直接去市场
                if (userAgent.ToLower().Contains("ipad") || userAgent.ToLower().Contains("iphone"))
                {
                    return this.Redirect("https://itunes.apple.com/cn/app/jin-yin-mao/id837965884?mt=8");
                }

                // 安卓微信去下载页面
                if (userAgent.Contains("MicroMessenger"))
                {
                    return this.View();
                }

                // 直接使用我们的下载服务
                if (userAgent.ToLower().Contains("android"))
                {
                    return this.Redirect(ConfigurationManager.AppSettings.Get("AndroidAppDownloadUrl"));
                }

                // 其他手机去m版
                return this.Redirect("http://m.jinyinmao.com.cn/");
            }

            // 电脑用户去PC版宣传页
            return this.Redirect("http://www.jinyinmao.com.cn/amp/html/20140529/index.html");
        }
    }
}