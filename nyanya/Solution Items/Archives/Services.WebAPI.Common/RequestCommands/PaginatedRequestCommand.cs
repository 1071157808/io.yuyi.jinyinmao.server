// FileInformation: nyanya/Services.WebAPI.Common/PaginatedRequestCommand.cs
// CreatedTime: 2014/03/30   11:02 PM
// LastUpdatedTime: 2014/03/30   11:03 PM

using Services.WebAPI.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Services.WebAPI.Common.RequestCommands
{
    public class PaginatedRequestCommand : IRequestCommand
    {
        /// <summary>
        ///     查询的页数，从1开始计数
        /// </summary>
        [Minimum(1)]
        [Required]
        public int PageIndex { get; set; }

        /// <summary>
        ///     一页数据数量
        /// </summary>
        [Minimum(1)]
        [Maximum(20)]
        [Required]
        public int PageSize { get; set; }
    }
}