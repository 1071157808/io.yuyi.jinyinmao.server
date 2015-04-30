// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-13  12:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-13  12:28 AM
// ***********************************************************************
// <copyright file="CheckCellphoneResponse.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Service.Dtos;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     Class CheckCellphoneResponse.
    /// </summary>
    public class CheckCellphoneResponse : IResponse
    {
        /// <summary>
        ///     手机号是否可以注册
        /// </summary>
        [Required, JsonProperty("result")]
        public bool Result { get; set; }
    }

    internal static class CheckCellphoneResultEx
    {
        internal static CheckCellphoneResponse ToResponse(this CheckCellphoneResult result)
        {
            return new CheckCellphoneResponse
            {
                Result = result.Result
            };
        }
    }
}
