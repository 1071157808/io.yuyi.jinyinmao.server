using Cat.Domain.Meow.Services.DTO;
using Domian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Meow.Services.Interfaces
{
    public interface IStatisticsService : IDomainService
    {
        /// <summary>
        /// 获取某时间段内的统计信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        Task<StatisticsResult> GetStatisticsAsync(DateTime startTime,DateTime endTime);
    }
}
