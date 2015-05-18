// FileInformation: nyanya/Domian/IPaginatedDto.cs
// CreatedTime: 2014/07/28   1:09 AM
// LastUpdatedTime: 2014/07/28   2:44 AM

using System.Collections.Generic;

namespace Domian.DTO
{
    public interface IPaginatedDto<out TDto>
    {
        bool HasNextPage { get; }

        IEnumerable<TDto> Items { get; }

        int PageIndex { get; }

        int PageSize { get; }

        int TotalCount { get; }

        int TotalPageCount { get; }
    }
}