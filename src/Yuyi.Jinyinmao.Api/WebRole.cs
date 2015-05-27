// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  7:18 PM
// ***********************************************************************
// <copyright file="WebRole.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.WindowsAzure.ServiceRuntime;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Api
{
    /// <summary>
    ///     WebRole.
    /// </summary>
    public class WebRole : RoleEntryPoint
    {
        /// <summary>
        ///     Called when [start].
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool OnStart()
        {
            Trace.TraceInformation("OrleansAzureWeb-OnStart");

            // For information on handling configuration changes see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            bool ok = base.OnStart();

            Trace.TraceInformation("OrleansAzureWeb-OnStart completed with OK=" + ok);

            return ok;
        }

        /// <summary>
        ///     Called when [stop].
        /// </summary>
        public override void OnStop()
        {
            Trace.TraceInformation("OrleansAzureWeb-OnStop");
            base.OnStop();
        }

        /// <summary>
        ///     Runs this instance.
        /// </summary>
        public override void Run()
        {
            Trace.TraceInformation("OrleansAzureWeb-Run");
            try
            {
                base.Run();
            }
            catch (Exception exc)
            {
                Trace.TraceError("Run() failed with " + exc.GetExceptionString());
            }
        }

        /// <summary>
        ///     Roles the environment changing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoleEnvironmentChangingEventArgs" /> instance containing the event data.</param>
        private static void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}