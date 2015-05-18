using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Lib.Data
{
    /// <summary>
    /// 列表字段排序条件
    /// </summary>
    public class SortCondition
    {
        /// <summary>
        /// 构造一个排序字段名称和排序方式的排序条件
        /// </summary>
        /// <param name="sortField">字段名称</param>
        /// <param name="listSortDirection">排序方式</param>
        public SortCondition(string sortField, ListSortDirection listSortDirection = ListSortDirection.Ascending)
        {
            SortField = sortField;
            ListSortDirection = listSortDirection;
        }

        /// <summary>
        /// 获取或设置 排序字段名称
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 获取或设置 排序方向
        /// </summary>
        public ListSortDirection ListSortDirection { get; set; }
    }
}
