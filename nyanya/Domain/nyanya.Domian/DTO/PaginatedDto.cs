// FileInformation: nyanya/Domian/PaginatedDto.cs
// CreatedTime: 2014/07/28   1:10 AM
// LastUpdatedTime: 2014/07/28   1:35 AM

using System;
using System.Collections.Generic;
using System.Linq;

namespace Domian.DTO
{
    public static class PaginatedListExtensions
    {
        public static IPaginatedDto<TDto> ToPaginatedDto<TDto, TEntity>(this IPaginatedDto<TEntity> source, Func<TEntity, TDto> selector)
        {
            return new PaginatedDto<TDto>(source.PageIndex, source.PageSize, source.TotalCount, source.Items.Select(selector));
        }

        public static IPaginatedDto<TDto> ToPaginatedDto<TDto, TEntity>(this IPaginatedDto<TEntity> source, IEnumerable<TDto> items)
        {
            return new PaginatedDto<TDto>(source.PageIndex, source.PageSize, source.TotalCount, items);
        }
    }

    public class PaginatedDto<TDto> : List<TDto>, IPaginatedDto<TDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedDto{TDto}"/> class.
        /// Only for document.
        /// </summary>
        public PaginatedDto()
        {
            this.Items = new List<TDto>();
        }

        public PaginatedDto(int pageIndex, int pageSize, int totalCount, IEnumerable<TDto> source)
        {
            this.AddRange(source);

            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        #region IPaginatedDto<TDto> Members

        /// <summary>
        ///     是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get { return this.PageIndex < this.TotalPageCount; }
        }

        /// <summary>
        ///     产品数据
        /// </summary>
        public IEnumerable<TDto> Items
        {
            get { return this; }
            set
            {
                this.Clear();
                this.AddRange(value);
            }
        }

        /// <summary>
        ///     当前页码
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        ///     页面大小
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        ///     总数据数量
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        ///     总页数
        /// </summary>
        public int TotalPageCount { get; private set; }

        #endregion IPaginatedDto<TDto> Members

        public static PaginatedDto<TDto> LoadData<TEntity>(PaginatedDto<TEntity> source, Func<TEntity, TDto> selector)
        {
            return new PaginatedDto<TDto>
            {
                PageIndex = source.PageIndex,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                TotalPageCount = source.TotalPageCount,
                Items = source.Select(selector)
            };
        }
    }
}