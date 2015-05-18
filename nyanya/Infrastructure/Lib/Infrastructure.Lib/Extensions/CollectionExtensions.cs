using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Infrastructure.Lib.Data;
using Infrastructure.Lib.Filter;

namespace Infrastructure.Lib.Extensions
{
    /// <summary>
    /// 集合扩展方法类
    /// </summary>
    public static class CollectionExtensions
    {
        #region IQueryable的扩展

        /// <summary>
        /// 根据第三方条件是否为真来决定是否执行指定条件的查询
        /// </summary>
        /// <param name="source"> 要查询的源 </param>
        /// <param name="predicate"> 查询条件 </param>
        /// <param name="condition"> 第三方条件 </param>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <returns> 查询的结果 </returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            source.CheckNotNull("source");
            predicate.CheckNotNull("predicate");

            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 把<see cref="IQueryable{T}"/>集合按指定字段与排序方式进行排序
        /// </summary>
        /// <param name="source">要排序的数据集</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="sortDirection">排序方向</param>
        /// <typeparam name="T">动态类型</typeparam>
        /// <returns>排序后的数据集</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source,
            string propertyName,
            ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            source.CheckNotNull("source");
            propertyName.CheckNotNullOrEmpty("propertyName");

            return QueryablePropertySorter<T>.OrderBy(source, propertyName, sortDirection);
        }

        /// <summary>
        /// 把<see cref="IQueryable{T}"/>集合按指定字段排序条件进行排序
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要排序的数据集</param>
        /// <param name="sortCondition">列表字段排序条件</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, SortCondition sortCondition)
        {
            source.CheckNotNull("source");
            sortCondition.CheckNotNull("sortCondition");

            return source.OrderBy(sortCondition.SortField, sortCondition.ListSortDirection);
        }

        /// <summary>
        /// 把<see cref="IQueryable{T}"/>集合按指定字段排序条件集合进行排序
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要排序的数据集</param>
        /// <param name="sortConditions">列表字段排序条件集合</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, SortCondition[] sortConditions)
        {
            source.CheckNotNull("source");
            sortConditions.CheckNotNull("source");

            var count = 0;
            IOrderedQueryable<T> orderSource = null;
            foreach (var sortCondition in sortConditions)
            {
                orderSource = count == 0
                    ? QueryablePropertySorter<T>.OrderBy(source, sortCondition.SortField, sortCondition.ListSortDirection)
                    : QueryablePropertySorter<T>.ThenBy(orderSource, sortCondition.SortField, sortCondition.ListSortDirection);
                count++;
            }

            source = orderSource.IsNull() ? source : orderSource;

            return source;
        }

        /// <summary>
        /// 把<see cref="IOrderedQueryable{T}"/>集合继续按指定字段排序方式进行排序
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要排序的数据集</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source,
            string propertyName,
            ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            source.CheckNotNull("source");
            propertyName.CheckNotNullOrEmpty("propertyName");

            return QueryablePropertySorter<T>.ThenBy(source, propertyName, sortDirection);
        }

        /// <summary>
        /// 把<see cref="IOrderedQueryable{T}"/>集合继续指定字段排序方式进行排序
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要排序的数据集</param>
        /// <param name="sortCondition">列表字段排序条件</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, SortCondition sortCondition)
        {
            source.CheckNotNull("source");
            sortCondition.CheckNotNull("sortCondition");

            return source.ThenBy(sortCondition.SortField, sortCondition.ListSortDirection);
        }

        /// <summary>
        /// 把<see cref="IOrderedQueryable{T}"/>集合继续指定字段排序方式进行排序
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要排序的数据集</param>
        /// <param name="sortCondition">列表字段排序条件</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, SortCondition[] sortConditions)
        {
            source.CheckNotNull("source");
            sortConditions.CheckNotNull("source");

            var count = 0;
            IOrderedQueryable<T> orderSource = null ;
            foreach (var sortCondition in sortConditions)
            {
                orderSource = count == 0
                    ? QueryablePropertySorter<T>.ThenBy(source, sortCondition.SortField, sortCondition.ListSortDirection)
                    : QueryablePropertySorter<T>.ThenBy(orderSource, sortCondition.SortField, sortCondition.ListSortDirection);
                count++;
            }

            source = orderSource.IsNull() ? source : orderSource;

            return source;
        }

        #endregion
    }
}
