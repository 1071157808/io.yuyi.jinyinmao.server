// FileInformation: nyanya/nyanya.AspDotNet.Common/IDtoList.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:10 AM

using System.Collections.Generic;

namespace nyanya.AspDotNet.Common.Dtos
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