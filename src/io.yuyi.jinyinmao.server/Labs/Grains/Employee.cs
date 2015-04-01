// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-03-31  10:12 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-01  12:00 AM
// ***********************************************************************
// <copyright file="Employee.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using GrainInterface;
using Orleans;
using Orleans.Providers;

namespace Grains
{
    [StorageProvider(ProviderName = "AzureStore")]
    public class Employee : Grain<IEmployeeState>, IEmployee
    {
        #region IEmployee Members

        public Task<int> GetLevel()
        {
            return Task.FromResult(State.Level);
        }

        public Task<IManager> GetManager()
        {
            return Task.FromResult(State.Manager);
        }

        public Task Greeting(IEmployee from, string message)
        {
            Console.WriteLine("{0} said: {1}", @from.GetPrimaryKey(), message);
            return TaskDone.Done;
        }

        public Task Promote(int newLevel)
        {
            State.Level = newLevel;
            State.WriteStateAsync();
            return TaskDone.Done;
        }

        public Task SetManager(IManager manager)
        {
            State.Manager = manager;
            State.WriteStateAsync();
            return TaskDone.Done;
        }

        #endregion IEmployee Members
    }
}
