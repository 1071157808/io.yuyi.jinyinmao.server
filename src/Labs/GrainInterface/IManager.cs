// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-03-31  10:02 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-31  10:02 PM
// ***********************************************************************
// <copyright file="IManager.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterface
{
    public interface IManager : IGrain
    {
        Task AddDirectReport(IEmployee employee);

        Task<IEmployee> AsEmployee();

        Task<List<IEmployee>> GetDirectReports();
    }
}
