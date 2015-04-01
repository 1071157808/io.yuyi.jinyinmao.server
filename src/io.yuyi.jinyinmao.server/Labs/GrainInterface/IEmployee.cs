// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-03-31  6:20 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-31  10:02 PM
// ***********************************************************************
// <copyright file="IEmployee.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace GrainInterface
{
    public interface IEmployee : IGrain
    {
        Task<int> GetLevel();

        Task<IManager> GetManager();

        Task Greeting(IEmployee from, string message);

        Task Promote(int newLevel);

        Task SetManager(IManager manager);
    }
}
