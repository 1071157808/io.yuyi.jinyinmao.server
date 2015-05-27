// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:00 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-25  9:08 PM
// ***********************************************************************
// <copyright file="WorkerRole.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace Yuyi.Jinyinmao.Silo
{
    /// <summary>
    ///     WorkerRole.
    /// </summary>
    public class WorkerRole : RoleEntryPoint
    {
        /// <summary>
        ///     The orleans azure silo
        /// </summary>
        private AzureSilo orleansAzureSilo;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WorkerRole" /> class.
        /// </summary>
        public WorkerRole()
        {
            Trace.TraceInformation("OrleansAzureSilos-Constructor called");
        }

        /// <summary>
        ///     Called when [start].
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool OnStart()
        {
            Trace.TraceInformation("OrleansAzureSilos-OnStart called");

            Trace.TraceInformation("OrleansAzureSilos-OnStart Initializing config");

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            SetupEnvironmentChangeHandlers();

            bool ok = base.OnStart();

            Trace.TraceInformation("OrleansAzureSilos-OnStart called base.OnStart ok=" + ok);

            return ok;
        }

        /// <summary>
        ///     Called when [stop].
        /// </summary>
        public override void OnStop()
        {
            Trace.TraceInformation("OrleansAzureSilos-OnStop called");
            if (this.orleansAzureSilo != null)
            {
                this.orleansAzureSilo.Stop();
            }
            RoleEnvironment.Changing -= RoleEnvironmentChanging;
            base.OnStop();
            Trace.TraceInformation("OrleansAzureSilos-OnStop finished");
        }

        /// <summary>
        ///     Runs this instance.
        /// </summary>
        public override void Run()
        {
            Trace.TraceInformation("OrleansAzureSilos-Run entry point called");

            Trace.TraceInformation("OrleansAzureSilos-OnStart Starting Orleans silo");

            ClusterConfiguration config = new ClusterConfiguration();
            config.StandardLoad();

            // It is IMPORTANT to start the silo not in OnStart but in Run.
            // Azure may not have the firewalls open yet (on the remote silos) at the OnStart phase.
            this.orleansAzureSilo = new AzureSilo();
            bool ok = this.orleansAzureSilo.Start(RoleEnvironment.DeploymentId, RoleEnvironment.CurrentRoleInstance, config);

            Trace.TraceInformation("OrleansAzureSilos-OnStart Orleans silo started ok=" + ok, "Information");

            this.orleansAzureSilo.Run(); // Call will block until silo is shutdown
        }

        /// <summary>
        ///     Roles the environment changing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoleEnvironmentChangingEventArgs" /> instance containing the event data.</param>
        private static void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            int i = 1;
            foreach (var c in e.Changes)
            {
                Trace.TraceInformation("RoleEnvironmentChanging: #{0} Type={1} Change={2}", i++, c.GetType().FullName, c);
            }

            // If a configuration setting is changing);
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        /// <summary>
        ///     Setups the environment change handlers.
        /// </summary>
        private static void SetupEnvironmentChangeHandlers()
        {
            // For information on handling configuration changes see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
        }
    }
}