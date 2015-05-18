// FileInformation: nyanya/Services.WebAPI.Common/PaginatedDto.cs
// CreatedTime: 2014/03/30   11:03 PM
// LastUpdatedTime: 2014/03/31   10:29 AM

using Infrastructure.Data.EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.WebAPI.Common.Dtos
{
    public static class PaginatedListExtensions
    {
        public static PaginatedDto<TDto> ToPaginatedDto<TDto, TEntity>(this PaginatedList<TEntity> source, IEnumerable<TDto> items) where TDto : IDto
        {
            return new PaginatedDto<TDto>
            {
                PageIndex = source.PageIndex,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                TotalPageCount = source.TotalPageCount,
                HasNextPage = source.HasNextPage,
                Items = items
            };
        }

        public static PaginatedDto<TDto> ToPaginatedDto<TDto, TEntity>(this PaginatedList<TEntity> source, Func<TEntity, TDto> selector) where TDto : IDto
        {
            return new PaginatedDto<TDto>
            {
                PageIndex = source.PageIndex,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                TotalPageCount = source.TotalPageCount,
                HasNextPage = source.HasNextPage,
                Items = source.Select(selector)
            };
        }
    }

    public class PaginatedDto<TDto> : IPaginatedDto<TDto> where TDto : IDto
    {
        public static PaginatedDto<TDto> Empty
        {
            get
            {
                return new PaginatedDto<TDto>
                    {
                        HasNextPage = false,
                        Items = Enumerable.Empty<TDto>(),
                        PageIndex = 1,
                        PageSize = 10,
                        TotalPageCount = 1,
                        TotalCount = 0
                    };
            }
        }

        #region IPaginatedDto<TDto> Members

        /// <summary>
        ///     是否有下一页
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        ///     产品数据
        /// </summary>
        public IEnumerable<TDto> Items { get; set; }

        /// <summary>
        ///     当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     页面大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     总数据数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        ///     总页数
        /// </summary>
        public int TotalPageCount { get; set; }

        #endregion IPaginatedDto<TDto> Members

        public static PaginatedDto<TDto> LoadData<TEntity>(PaginatedList<TEntity> source, Func<TEntity, TDto> selector)
        {
            return new PaginatedDto<TDto>
            {
                PageIndex = source.PageIndex,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                TotalPageCount = source.TotalPageCount,
                HasNextPage = source.HasNextPage,
                Items = source.Select(selector)
            };
        }
    }
}