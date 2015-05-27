// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-08  11:59 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-08  12:09 PM
// ***********************************************************************
// <copyright file="PaginatedResponse.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Moe.AspNet.Models;
using Moe.Lib;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     PaginatedResponse.
    /// </summary>
    /// <typeparam name="TResponse">The type of the t response.</typeparam>
    public class PaginatedResponse<TResponse> : IResponse where TResponse : IResponse
    {
        /// <summary>
        ///     是否还有下一页
        /// </summary>
        [Required, JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }

        /// <summary>
        ///     分页元素
        /// </summary>
        [Required, JsonProperty("items")]
        public List<TResponse> Items { get; set; }

        /// <summary>
        ///     页面索引
        /// </summary>
        [Required, JsonProperty("pageIndex")]
        public int PageIndex { get; set; }

        /// <summary>
        ///     一页的元素数量
        /// </summary>
        [Required, JsonProperty("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        ///     总数量
        /// </summary>
        [Required, JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        ///     总页数，如果有5页，则分别为第0页到第4页
        /// </summary>
        [Required, JsonProperty("totalPageCount")]
        public int TotalPageCount { get; set; }
    }

    internal static class PaginatedListEx
    {
        internal static PaginatedResponse<TResponse> ToResponse<TResponse>(this IPaginatedList<TResponse> paginatedList) where TResponse : IResponse => new PaginatedResponse<TResponse>
        {
            HasNextPage = paginatedList.HasNextPage,
            Items = paginatedList.Items.ToList(),
            PageIndex = paginatedList.PageIndex,
            PageSize = paginatedList.PageSize,
            TotalCount = paginatedList.TotalCount,
            TotalPageCount = paginatedList.TotalPageCount
        };
    }
}