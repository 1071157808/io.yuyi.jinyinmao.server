// FileInformation: nyanya/Services.WebAPI.Common/IPaginatedDto.cs
// CreatedTime: 2014/03/30   11:03 PM
// LastUpdatedTime: 2014/03/30   11:04 PM

using System.Collections.Generic;

namespace Services.WebAPI.Common.Dtos
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