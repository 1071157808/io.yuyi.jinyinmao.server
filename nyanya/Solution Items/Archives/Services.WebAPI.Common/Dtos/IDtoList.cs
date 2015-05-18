// FileInformation: nyanya/Services.WebAPI.Common/IDtoList.cs
// CreatedTime: 2014/03/30   11:03 PM
// LastUpdatedTime: 2014/03/30   11:04 PM

using System.Collections.Generic;

namespace Services.WebAPI.Common.Dtos
{
    public interface IDtoList<TDto> where TDto : IDto
    {
        List<TDto> Items { get; set; }
    }

    public interface IDynamicDtoList
    {
        List<dynamic> Items { get; set; }
    }
}