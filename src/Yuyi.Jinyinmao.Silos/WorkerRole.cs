// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-21  12:16 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-21  12:53 PM
// ***********************************************************************
// <copyright file="WorkerRole.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Diagnostics;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using Orleans.Runtime.Host;

namespace OrleansXO.WorkerRole
{
    /// <summary>
    ///     WorkerRole.
    /// </summary>
    public class WorkerRole : RoleEntryPoint
    {
        /// <summary>
        ///     The silo
        /// </summary>
        private AzureSilo silo;

        /// <summary>
        ///     Called when [start].
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            silo = new AzureSilo();

            return silo.Start(RoleEnvironment.DeploymentId, RoleEnvironment.CurrentRoleInstance);
        }

        /// <summary>
        ///     Called when [stop].
        /// </summary>
        public override void OnStop()
        {
            silo.Stop();
        }

        /// <summary>
        ///     Runs this instance.
        /// </summary>
        public override void Run()
        {
            Trace.TraceInformation("=============================Silo {0} has started=============================", RoleEnvironment.DeploymentId);
            silo.Run();
            Trace.TraceInformation("=============================Silo {0} has stoped=============================", RoleEnvironment.DeploymentId);
        }
    }
}
