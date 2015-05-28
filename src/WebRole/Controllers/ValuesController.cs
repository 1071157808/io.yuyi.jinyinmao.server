// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-28  12:42 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-28  1:17 AM
// ***********************************************************************
// <copyright file="ValuesController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Web.Http;

namespace WebRole.Controllers
{
    /// <summary>
    ///     ValuesController.
    /// </summary>
    public class ValuesController : ApiController
    {
        /// <summary>
        ///     Gets this instance.
        /// </summary>
        /// <remarks>
        ///     aaaaaaaaaaa
        /// </remarks>
        /// <returns>System.String.</returns>
        [Route("")]
        public string Get()
        {
            return "value";
        }
    }
}