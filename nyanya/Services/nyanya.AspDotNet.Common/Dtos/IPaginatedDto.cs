// FileInformation: nyanya/nyanya.AspDotNet.Common/IPaginatedDto.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:10 AM

using System.Collections.Generic;

namespace nyanya.AspDotNet.Common.Dtos
{
    public interface IPaginatedDto<out TDto> where TDto : IDto
    {
        bool HasNextPage { get; set; }

        IEnumerable<TDto> Items { get; }

        int PageIndex { get; set; }

        int PageSize { get; set; }

        int TotalCount { get; set; }

        int TotalPageCount { get; set; }
    }
}