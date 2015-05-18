// FileInformation: nyanya/Infrastructure.Data.EntityFramework.Extensions/PaginatedList.cs
// CreatedTime: 2014/03/17   12:01 PM
// LastUpdatedTime: 2014/03/17   2:28 PM

using System;
using System.Collections.Generic;

namespace Infrastructure.Data.EntityFramework.Extensions
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(int pageIndex, int pageSize, int totalCount, IEnumerable<T> source)
        {
            this.AddRange(source);

            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public bool HasNextPage
        {
            get { return this.PageIndex < this.TotalPageCount; }
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPageCount { get; set; }
    }
}