// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : HomeController.cs
// Created          : 2015-08-03  8:01 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-03  8:58 AM
// ***********************************************************************
// <copyright file="HomeController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Web.Http;

namespace Yuyi.Jinyinmao.Update.Controllers
{
    /// <summary>
    ///     HomeController.
    /// </summary>
    public class HomeController : ApiController
    {
        /// <summary>
        ///     Check app version.
        /// </summary>
        /// <param name="source">The source.</param>
        [HttpGet, HttpPost, HttpOptions, Route("V1/app/upgradeex")]
        public IHttpActionResult Index([FromUri] string source)
        {
            source = source.ToUpperInvariant();
            string version = "4.1";
            string message = "新版本发布，请至金银猫官网进行升级";
            string url = "https://cdn.jinyinmao.com.cn/publicfiles/App/Android/jinyinmao_v4.1.apk";

            if (!string.IsNullOrEmpty(source) && source.StartsWith("I", StringComparison.InvariantCulture))
            {
                version = "4.1";
                message = "您需要升级后才能继续使用";
                url = "https://itunes.apple.com/cn/app/jin-yin-mao/id837965884?mt=8";
            }

            return this.Ok(new { Status = 1, Url = url, Version = version, Message = message });
        }
    }
}