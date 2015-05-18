// FileInformation: nyanya/nyanya.AspDotNet.Common/PaginatedRequestCommand.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:18 AM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.AspDotNet.Common.RequestCommands
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