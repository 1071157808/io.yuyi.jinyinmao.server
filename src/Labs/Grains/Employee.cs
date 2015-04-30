// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-03-31  10:12 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-04  10:52 PM
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
            this.DeactivateOnIdle();
            return Task.FromResult(this.State.Level);
        }

        public Task<IManager> GetManager()
        {
            return Task.FromResult(this.State.Manager);
        }

        public Task Greeting(IEmployee from, string message)
        {
            Console.WriteLine("{0} said: {1}", @from.GetPrimaryKey(), message);
            return TaskDone.Done;
        }

        public Task Promote(int newLevel)
        {
            this.State.Level = newLevel;
            this.State.WriteStateAsync();
            this.DeactivateOnIdle();
            return TaskDone.Done;
        }

        public Task SetManager(IManager manager)
        {
            this.State.Manager = manager;
            this.State.WriteStateAsync();
            return TaskDone.Done;
        }

        #endregion IEmployee Members
    }
}
