// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-03-31  10:19 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-04  10:40 PM
// ***********************************************************************
// <copyright file="Manager.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrainInterface;
using Orleans;
using Orleans.Providers;

namespace Grains
{
    [StorageProvider(ProviderName = "AzureStore")]
    public class Manager : Grain<IManagerState>, IManager
    {
        private IEmployee me;

        #region IManager Members

        public Task AddDirectReport(IEmployee employee)
        {
            State.Reports.Add(employee);
            State.WriteStateAsync();
            employee.SetManager(this);
            employee.Greeting(me, "Welcome to my team!");
            this.DeactivateOnIdle();
            return TaskDone.Done;
        }

        public Task<IEmployee> AsEmployee()
        {
            State.WriteStateAsync();
            return Task.FromResult(this.me);
        }

        public Task<List<IEmployee>> GetDirectReports()
        {
            return Task.FromResult(State.Reports);
        }

        #endregion IManager Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            Guid key = this.GetPrimaryKey();
            this.me = EmployeeFactory.GetGrain(key);
            return base.OnActivateAsync();
        }
    }
}
