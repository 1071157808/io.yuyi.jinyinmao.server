// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  12:59 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  11:39 PM
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
    public class WorkerRole : RoleEntryPoint
    {
        private const string DATA_CONNECTION_STRING_KEY = "DataConnectionString";
        private AzureSilo orleansAzureSilo;

        public WorkerRole()
        {
            Trace.WriteLine("OrleansAzureSilos-Constructor called");
        }

        public override bool OnStart()
        {
            Trace.WriteLine("OrleansAzureSilos-OnStart called", "Information");

            Trace.WriteLine("OrleansAzureSilos-OnStart Initializing config", "Information");

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            SetupEnvironmentChangeHandlers();

            bool ok = base.OnStart();

            Trace.WriteLine("OrleansAzureSilos-OnStart called base.OnStart ok=" + ok, "Information");

            return ok;
        }

        public override void OnStop()
        {
            Trace.WriteLine("OrleansAzureSilos-OnStop called", "Information");
            if (this.orleansAzureSilo != null)
            {
                this.orleansAzureSilo.Stop();
            }
            RoleEnvironment.Changing -= RoleEnvironmentChanging;
            base.OnStop();
            Trace.WriteLine("OrleansAzureSilos-OnStop finished", "Information");
        }

        public override void Run()
        {
            Trace.WriteLine("OrleansAzureSilos-Run entry point called", "Information");

            Trace.WriteLine("OrleansAzureSilos-OnStart Starting Orleans silo", "Information");

            ClusterConfiguration config = new ClusterConfiguration();
            config.StandardLoad();

            // It is IMPORTANT to start the silo not in OnStart but in Run.
            // Azure may not have the firewalls open yet (on the remote silos) at the OnStart phase.
            this.orleansAzureSilo = new AzureSilo();
            bool ok = this.orleansAzureSilo.Start(RoleEnvironment.DeploymentId, RoleEnvironment.CurrentRoleInstance, config);

            Trace.WriteLine("OrleansAzureSilos-OnStart Orleans silo started ok=" + ok, "Information");

            this.orleansAzureSilo.Run(); // Call will block until silo is shutdown
        }

        private static void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            int i = 1;
            foreach (var c in e.Changes)
            {
                Trace.WriteLine(string.Format("RoleEnvironmentChanging: #{0} Type={1} Change={2}", i++, c.GetType().FullName, c));
            }

            // If a configuration setting is changing);
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        private static void SetupEnvironmentChangeHandlers()
        {
            // For information on handling configuration changes see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
        }
    }
}